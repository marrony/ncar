using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;
	public WheelCollider RearLeftWheel;
	public WheelCollider RearRightWheel;
	
	public float[] GearRatio;
	public float ReverseGearRatio = 0;
	public float DifferentialRatio = 1;
	public int CurrentGear = 0;
	
	public float EngineTorque = 600.0f;
	public float MaxEngineRPM = 3000.0f;
	public float MinEngineRPM = 1000.0f;
		
	public AudioSource engineAudio;
	public AudioSource windAudio;
	
	public Material rearLight;
	
	private float EngineRPM = 0.0f;
	private float time = 0;
	private float drag;
	private float dowforce;
	
	/*
	 * motor:wheel
	 * 1ª 3,596:1
	 * 2ª 2,378:1
	 * 3ª 1,531:1
	 * 4ª 1,000:1
	 * Ré 4,229:1
	 * Diferencial 3,92:1
	 */

	void Start () {
		Transform cg = transform.FindChild("CG");
		rigidbody.centerOfMass = cg.localPosition;
	}

	void Update () {
		time += Time.deltaTime;

		EngineRPM = EngineRPMForGear(CurrentGear);
		ShiftGears();
		
		float engineRpmAbs = Mathf.Abs(EngineRPM);
	
		engineAudio.pitch = Mathf.Min((engineRpmAbs / MaxEngineRPM) + 0.5f, 2.0f);
		
		windAudio.volume = rigidbody.velocity.magnitude/400;
		
		var verticalAxis = Input.GetAxis("Vertical");
		
		if(engineRpmAbs < MaxEngineRPM && verticalAxis >= 0) {
			if(EngineRPM < 0)
				Break(verticalAxis);
			else
				Accelerate(verticalAxis);
		}
		
		if(verticalAxis <= 0) {
			if(EngineRPM > 0)
				Break(-verticalAxis);
			else
				Accelerate(verticalAxis);
		}
		
		float v = rigidbody.velocity.magnitude/60f;
		
		var steerAngle = Mathf.Max(1f - v, 0.02f) * 30 * Input.GetAxis("Horizontal");
		FrontLeftWheel.steerAngle = steerAngle;
		FrontRightWheel.steerAngle = steerAngle;
	}
	
	void Accelerate(float verticalAxis) {
		var motorTorque = EngineTorque * verticalAxis;
		
		RearLeftWheel.brakeTorque = 0;
		RearRightWheel.brakeTorque = 0;
		RearLeftWheel.motorTorque = motorTorque;
		RearRightWheel.motorTorque = motorTorque;
		
		if(rearLight != null) {
			rearLight.SetColor("_Color", new Color(0.1f, 0f, 0f));
			//rearLight.SetFloat("_Shininess", 1f);
		}
	}
	
	void Break(float verticalAxis) {
		var brakeTorque = EngineTorque * 10 * verticalAxis;
		
		RearLeftWheel.brakeTorque = brakeTorque;
		RearRightWheel.brakeTorque = brakeTorque;
		
		if(rearLight != null && brakeTorque > 0.5) {
			rearLight.SetColor("_Color", new Color(1f, 0f, 0f));
			//rearLight.SetFloat("_Shininess", 0.01f);
		}
	}

	void FixedUpdate() {
		drag = calculateDrag(rigidbody.velocity.magnitude);
		rigidbody.AddForce(-rigidbody.velocity.normalized * drag);
		
		dowforce = calculateDownforce(rigidbody.velocity.magnitude, 4, 15 * Mathf.Deg2Rad);
		rigidbody.AddRelativeForce(new Vector3(0, -dowforce, 0));
	}

	private float WheelRPM() {
		return (RearLeftWheel.rpm + RearRightWheel.rpm) * 0.5f;
	}
	
	private float EngineRPMForGear(int gear) {
		float wheelRpm = WheelRPM();
		float ratio = wheelRpm >= 0 ? GearRatio[gear] : ReverseGearRatio;
		
		return WheelRPM() * ratio * DifferentialRatio;
	}
	
	private void ShiftGears() {
		if ( EngineRPM >= MaxEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for (int i = 0; i < GearRatio.Length; i ++ ) {
				if ( EngineRPMForGear(i) < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for (int j = GearRatio.Length-1; j >= 0; j -- ) {
				if ( EngineRPMForGear(j) > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}
	
	private static float calculateDrag (float v) {
		float area = 3f;
		float coefficient = 1.15f;
		return 0.5f * 1.204f * v*v * coefficient * area;
	}
	
	private static float calculateDownforce (float v, float wingArea, float angleOfAttack) {
		float airDensity = 1.1f;
		return 0.5f * wingArea * angleOfAttack * airDensity * v*v;
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 20), "KM/h: " + (rigidbody.velocity.magnitude * 3.6f));
		GUI.Label(new Rect(10, 30, 200, 20), "steer: " + (FrontLeftWheel.steerAngle));
		GUI.Label(new Rect(10, 50, 100, 20), "Gear: " + (CurrentGear + 1));
		
		GUI.Label(new Rect(10, 70, 100, 20), "RPM: " + RearLeftWheel.rpm);
		GUI.Label(new Rect(10, 90, 100, 20), "RPM: " + RearRightWheel.rpm);
		GUI.Label(new Rect(10, 110, 100, 20), "diff: " + (RearLeftWheel.rpm - RearRightWheel.rpm));
		
		GUI.Label(new Rect(10, 130, 100, 20), "time: " + time);
		GUI.Label(new Rect(10, 150, 200, 20), "dowforce (Kg): " + (dowforce/9.8f));
	}
	
} 
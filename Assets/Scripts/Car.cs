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
		
	private float EngineRPM = 0.0f;
	
	private float time = 0;
	
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
		//Transform cg = transform.FindChild("CG");
		rigidbody.centerOfMass = new Vector3(0, -0.7f, 0);

		float x = rigidbody.inertiaTensor.x;
		float y = rigidbody.inertiaTensor.y;
		float z = rigidbody.inertiaTensor.z;
		
		rigidbody.inertiaTensor = new Vector3(z, x, y);
	}

	void Update () {
		time += Time.deltaTime;
		
		rigidbody.drag = rigidbody.velocity.magnitude / 150f;
		
		EngineRPM = EngineRPMForGear(CurrentGear);
		ShiftGears();
	
		/*
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0 ;
		if ( audio.pitch > 2.0 ) 
			audio.pitch = 2.0;
		*/
		
		var motorTorque = (EngineTorque) * Input.GetAxis("Vertical");
		
		RearLeftWheel.motorTorque = motorTorque;
		RearRightWheel.motorTorque = motorTorque;
		
		var steerAngle = 20 * Input.GetAxis("Horizontal");
		FrontLeftWheel.steerAngle = steerAngle;
		FrontRightWheel.steerAngle = steerAngle;
	}
	
	private float WheelRPM() {
		return (RearLeftWheel.rpm + RearRightWheel.rpm) * 0.5f;
	}
	
	private float EngineRPMForGear(int gear) {
		float ratio = Input.GetAxis("Vertical") >= 0 ? GearRatio[gear] : ReverseGearRatio;
		
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
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 20), "KM/h: " + (rigidbody.velocity.magnitude * 3.6f));
		GUI.Label(new Rect(10, 30, 100, 20), "RPM: " + (EngineRPM));
		GUI.Label(new Rect(10, 50, 100, 20), "Gear: " + (CurrentGear + 1));
		
		GUI.Label(new Rect(10, 70, 100, 20), "RPM: " + RearLeftWheel.rpm);
		GUI.Label(new Rect(10, 90, 100, 20), "RPM: " + RearRightWheel.rpm);
		GUI.Label(new Rect(10, 110, 100, 20), "diff: " + (RearLeftWheel.rpm - RearRightWheel.rpm));
		
		GUI.Label(new Rect(10, 130, 100, 20), "time: " + time);
		GUI.Label(new Rect(10, 150, 1000, 20), "time: " + (rigidbody.inertiaTensor));
		 
	}
	
} 
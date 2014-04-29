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

	public bool Debug { get; set; }

	public GameObject turnLightRL;
	public GameObject turnLightRR;
	private MachineGun machineGun;
	
	private float EngineRPM = 0.0f;
	private float drag;
	private float dowforce;
	private float speed;
	private bool wrecked = false;
	private bool braking = false;
	private bool brakeLightOn = false;

	public CarControl Control { get; private set; }
	public MachineGunControl MachineGunControl 
	{
		get { return machineGun.Control; } 
	}

	/*
	 * motor:wheel
	 * 1ª 3,596:1
	 * 2ª 2,378:1
	 * 3ª 1,531:1
	 * 4ª 1,000:1
	 * Ré 4,229:1
	 * Diferencial 3,92:1
	 */

	void Start () 
	{
		Transform cg = transform.FindChild("CG");
		rigidbody.centerOfMass = cg.localPosition;

		/*turnLightRL = transform.Find("Model/maverick/RL_Turn_Light").gameObject;
		turnLightRR = transform.Find("Model/maverick/RR_Turn_Light").gameObject;*/
		machineGun = transform.Find("MachineGun").GetComponent<MachineGun>();
		Control = new CarControl();
	}
	
	private void DisableCar()
	{
		Accelerate(0);
		RearLeftWheel.brakeTorque = 65;
		RearRightWheel.brakeTorque = 65;
		FrontLeftWheel.brakeTorque = 65;
		FrontRightWheel.brakeTorque = 65;
		engineAudio.Stop();
		windAudio.Stop();
	}
	
	void Update () 
	{
		if(wrecked){
			DisableCar();
			return;
		}
		
		speed = Vector3.Dot(rigidbody.velocity, transform.forward);
		Control.Speed = speed;
		braking = false;
		UpdateSteer();

		EngineRPM = EngineRPMForGear(CurrentGear);
		ShiftGears();
		float engineRpmAbs = Mathf.Abs(EngineRPM);
		engineAudio.pitch = Mathf.Min(engineRpmAbs / MaxEngineRPM + MinEngineRPM / MaxEngineRPM, 2.0f);
		windAudio.volume = rigidbody.velocity.magnitude/400;

		switch (Control.Mode) {
		case TransmissionMode.Drive:
			Accelerate(Control.Accelerator);
			break;
		case TransmissionMode.Reverse:
			Accelerate(-Control.Accelerator);
			break;
		case TransmissionMode.Neutral:
			break;
		}
		
		if (Control.Brake > 0)
			Brake(Control.Brake);
		
		UpdateBrakeLight();
	}
	
	public void Wreck()
	{
		wrecked = true;
		machineGun.Wreck();
	}
	
	private void UpdateSteer()
	{
		FrontLeftWheel.steerAngle = Control.Steer;
		FrontRightWheel.steerAngle = Control.Steer;
	}

	private void Accelerate(float accelerator) 
	{
		var motorTorque = EngineTorque * accelerator;
		
		RearLeftWheel.brakeTorque = 0;
		RearRightWheel.brakeTorque = 0;
		RearLeftWheel.motorTorque = motorTorque;
		RearRightWheel.motorTorque = motorTorque;
	}
	
	private void Brake(float brake) 
	{
		var brakeTorque = EngineTorque * 10 * brake;
		
		RearLeftWheel.brakeTorque = brakeTorque;
		RearRightWheel.brakeTorque = brakeTorque;
		
		if (brakeTorque > 0.5) {
			braking = true;
		}
	}
	
	private void UpdateBrakeLight()
	{
		if(braking && !brakeLightOn) {
			turnLightRL.GetComponent<Light>().enabled = true;
			turnLightRR.GetComponent<Light>().enabled = true;
			brakeLightOn = true;
		}
		
		if (!braking && brakeLightOn) {
			turnLightRL.GetComponent<Light>().enabled = false;
			turnLightRR.GetComponent<Light>().enabled = false;
			brakeLightOn = false;
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
	
	void OnGUI() 
	{
		if (Debug)
			ShowDebugInfo ();
	}
	
	private void ShowDebugInfo ()
	{
		GUI.Label (new Rect (10, 10, 100, 20), "KM/h: " + (speed * 3.6f));
		GUI.Label (new Rect (10, 30, 200, 20), "steer: " + (FrontLeftWheel.steerAngle));
		GUI.Label (new Rect (10, 50, 100, 20), "Gear: " + (CurrentGear + 1));
		GUI.Label (new Rect (10, 70, 100, 20), "RPM: " + RearLeftWheel.rpm);
		GUI.Label (new Rect (10, 90, 100, 20), "RPM: " + RearRightWheel.rpm);
		GUI.Label (new Rect (10, 110, 100, 20), "diff: " + (RearLeftWheel.rpm - RearRightWheel.rpm));
		GUI.Label (new Rect (10, 130, 200, 20), "dowforce (Kg): " + (dowforce / 9.8f));
	}
} 

public enum TransmissionMode { Drive, Neutral, Reverse };

public class CarControl
{
	private float steer = 0;
	public float Steer 
	{ 
		get { return steer; }
		set { steer = Mathf.Clamp(value, -40, 40); }
	}
	
	private float brake = 0;
	public float Brake 
	{ 
		get { return brake; }
		set { brake = Mathf.Clamp(value, 0, 1); }
	}
	
	private float accelerator = 0;
	public float Accelerator
	{ 
		get { return accelerator; }
		set { accelerator = Mathf.Clamp(value, 0, 1); }
	}
	
	public float Speed { get; internal set; }
	public TransmissionMode Mode { get; set; }
}
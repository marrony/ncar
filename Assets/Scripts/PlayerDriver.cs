using UnityEngine;
using System.Collections;

public class PlayerDriver : MonoBehaviour {

	private Car car;

	void Start () 
	{
		car = GetComponent<Car>();
		car.gameObject.AddComponent("AudioListener");
	}

	void Update () 
	{
		UpdateSteer ();
		UpdateAcceleratorAndBrake ();
		UpdateWeapons ();
	}

	private void UpdateSteer ()
	{
		float v = car.Control.Speed/60f;
		float steerAngle = Mathf.Max(1f - v, 0.01f) * 26 * Input.GetAxis("Horizontal");
		
		car.Control.Steer = steerAngle;
	}

	private void UpdateAcceleratorAndBrake ()
	{
		CarControl carControl = car.Control;
		float verticalAxis = Input.GetAxis("Vertical");
		float absVerticalAxis = Mathf.Abs(verticalAxis);
		
		carControl.Accelerator = 0;
		carControl.Brake = 0;
		carControl.Mode = TransmissionMode.Neutral;

		if (verticalAxis > 0) {
			if (carControl.Speed > -1) {
				carControl.Mode = TransmissionMode.Drive;
				carControl.Accelerator = absVerticalAxis;
			} else {
				carControl.Brake = absVerticalAxis;
			}
		} else {
			if (carControl.Speed > 1) {
				carControl.Brake = absVerticalAxis;
			} else {
				carControl.Mode = TransmissionMode.Reverse;
				carControl.Accelerator = absVerticalAxis;
			}
		}
	}

	void UpdateWeapons ()
	{
		car.MachineGunControl.Shooting = Input.GetKey(KeyCode.Space);
	}
}

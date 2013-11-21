using UnityEngine;
using System.Collections;

public class PlayerDriver : MonoBehaviour {

	private CarControl control;

	void Start () 
	{
		control = GetComponent<Car>().Control;
	}

	void Update () 
	{
		UpdateSteer ();
		UpdateAcceleratorAndBrake ();
	}

	private void UpdateSteer ()
	{
		float v = control.Speed/60f;
		float steerAngle = Mathf.Max(1f - v, 0.01f) * 26 * Input.GetAxis("Horizontal");
		
		control.Steer = steerAngle;
	}

	private void UpdateAcceleratorAndBrake ()
	{
		float verticalAxis = Input.GetAxis("Vertical");
		float absVerticalAxis = Mathf.Abs(verticalAxis);
		
		control.Accelerator = 0;
		control.Brake = 0;
		control.Mode = TransmissionMode.Neutral;

		if (verticalAxis > 0) {
			if (control.Speed > -1) {
				control.Mode = TransmissionMode.Drive;
				control.Accelerator = absVerticalAxis;
			} else {
				control.Brake = absVerticalAxis;
			}
		} else {
			if (control.Speed > 1) {
				control.Brake = absVerticalAxis;
			} else {
				control.Mode = TransmissionMode.Reverse;
				control.Accelerator = absVerticalAxis;
			}
		}
	}
}

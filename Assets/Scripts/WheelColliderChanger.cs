using UnityEngine;
using System.Collections;

public class WheelColliderChanger : MonoBehaviour {
	public float slowSpeedsFrontForwardStiffness;
	public float slowSpeedsFrontSidewaysStiffness;

	public float slowSpeedsRearForwardStiffness;
	public float slowSpeedsRearSidewaysStiffness;

	public float highSpeedsFrontForwardStiffness;
	public float highSpeedsFrontSidewaysStiffness;

	public float highSpeedsRearForwardStiffness;
	public float highSpeedsRearSidewaysStiffness;

	public float speedThreshold = 50f;

	public WheelCollider[] frontColliders;
	public WheelCollider[] rearColliders;


	void Update () {		
		updateColliders(frontColliders, 
		                calculateStiffness(slowSpeedsFrontForwardStiffness, highSpeedsFrontForwardStiffness),
		                calculateStiffness(slowSpeedsFrontSidewaysStiffness, highSpeedsFrontSidewaysStiffness));

		updateColliders(rearColliders, 
		                calculateStiffness(slowSpeedsRearForwardStiffness, highSpeedsRearForwardStiffness),
		                calculateStiffness(slowSpeedsRearSidewaysStiffness, highSpeedsRearSidewaysStiffness));
	}

	float calculateStiffness(float slowStiffness, float highStiffness){
		float speed = Mathf.Abs(gameObject.GetComponentInChildren<Car>().Control.Speed * 3.6f);

		if(speed > speedThreshold)
			return highStiffness;

		return Mathf.Lerp(slowStiffness, highStiffness, Mathf.Sqrt((float) (speed / speedThreshold)));
	}

	void updateColliders(WheelCollider[] colliders, float forwardStiffness, float sidewaysStiffness){
		foreach (WheelCollider collider in colliders) {			
			collider.forwardFriction = cloneCurve(collider.forwardFriction, forwardStiffness);			
			collider.sidewaysFriction = cloneCurve(collider.sidewaysFriction, sidewaysStiffness);			
		}
	}

	WheelFrictionCurve cloneCurve(WheelFrictionCurve curve, float newStiffness){
		WheelFrictionCurve newCurve = new WheelFrictionCurve();			
		newCurve.extremumSlip = curve.extremumSlip;			
		newCurve.extremumValue = curve.extremumValue;			
		newCurve.asymptoteSlip = curve.asymptoteSlip;			
		newCurve.asymptoteValue = curve.asymptoteValue;			
		newCurve.stiffness = newStiffness;			
		return newCurve;
	}
}

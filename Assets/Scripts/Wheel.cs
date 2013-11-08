using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {

	public GameObject slipPrefab;
	public GameObject model;
	public float stressThreshold = 120000;
	public float stressTolerance = 100;

	private WheelCollider wheelCollider;
	private float rotationValue = 0.0f;
	private float originalSidewaysStiffness;
	private bool stressed = false;
	private WheelHit wheelHit;
	
	void Start() 
	{
		wheelCollider = GetComponent<WheelCollider>();
		originalSidewaysStiffness = wheelCollider.sidewaysFriction.stiffness;
	}
	
	void Update () 
	{
		UpdateModel();
		DebugWheel();
	}

	void FixedUpdate() 
	{
		bool touched = wheelCollider.GetGroundHit(out wheelHit);
		if (touched) {
			ResolveStress();
		}
	}

	private void UpdateModel ()
	{
		RaycastHit hit;
		Vector3 ColliderCenterPoint = wheelCollider.transform.TransformPoint(wheelCollider.center);
		
		if (Physics.Raycast(ColliderCenterPoint, -wheelCollider.transform.up, out hit, wheelCollider.suspensionDistance + wheelCollider.radius)) {
			model.transform.position = hit.point + (wheelCollider.transform.up * wheelCollider.radius);
		}else{
			model.transform.position = ColliderCenterPoint - (wheelCollider.transform.up * wheelCollider.suspensionDistance);
		}
		
		model.transform.rotation = wheelCollider.transform.rotation * 
			Quaternion.Euler( rotationValue, wheelCollider.steerAngle, 0 );
		
		rotationValue += wheelCollider.rpm * (360/60) * Time.deltaTime;
		
		WheelHit CorrespondingGroundHit ;
		wheelCollider.GetGroundHit(out CorrespondingGroundHit);
		
		if (Mathf.Abs( CorrespondingGroundHit.sidewaysSlip) > 5.0f) {
			if (slipPrefab) {
				Instantiate(slipPrefab, CorrespondingGroundHit.point, Quaternion.identity);
			}
		}
	}

	private void ResolveStress ()
	{
		float wheelLoad = wheelHit.sidewaysSlip * wheelHit.force;
		bool remainsStressed = Mathf.Abs(wheelLoad) > stressThreshold;
		
		if (remainsStressed) {
			stressed = true;
			float stressFactor = (Mathf.Abs(wheelLoad) - stressThreshold) / stressTolerance;
			
			WheelFrictionCurve curve = wheelCollider.sidewaysFriction;
			curve.stiffness = originalSidewaysStiffness * Mathf.Max(0.1f, 1 - stressFactor);
			wheelCollider.sidewaysFriction = curve;
		}
		
		if (stressed && !remainsStressed) {
			stressed = false;
			WheelFrictionCurve curve = wheelCollider.sidewaysFriction;
			curve.stiffness = originalSidewaysStiffness;
			wheelCollider.sidewaysFriction = curve;
		}
	}
	
	private void DebugWheel() 
	{
		Debug.DrawLine(wheelHit.point, wheelHit.point + new Vector3(0, wheelHit.force * 0.0004f, 0));
	}

}

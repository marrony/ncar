using UnityEngine;
using System.Collections;

public class WheelAlignment : MonoBehaviour {

	public WheelCollider CorrespondingCollider;
	public GameObject SlipPrefab;
	private float RotationValue = 0.0f;
	

	void Update () {
		RaycastHit hit;
		Vector3 ColliderCenterPoint = CorrespondingCollider.transform.TransformPoint(CorrespondingCollider.center);
		
		// now cast a ray out from the wheel collider's center the distance of the suspension, if it hit something, then use the "hit"
		// variable's data to find where the wheel hit, if it didn't, then se tthe wheel to be fully extended along the suspension.
		if ( Physics.Raycast(ColliderCenterPoint, -CorrespondingCollider.transform.up, out hit, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius)) {
			transform.position = hit.point + (CorrespondingCollider.transform.up * CorrespondingCollider.radius);
		}else{
			transform.position = ColliderCenterPoint - (CorrespondingCollider.transform.up * CorrespondingCollider.suspensionDistance);
		}
		
		// now set the wheel rotation to the rotation of the collider combined with a new rotation value. This new value
		// is the rotation around the axle, and the rotation from steering input.
		transform.rotation = CorrespondingCollider.transform.rotation * 
			Quaternion.Euler( RotationValue, CorrespondingCollider.steerAngle, 0 );
		
		// increase the rotation value by the rotation speed (in degrees per second)
		RotationValue += CorrespondingCollider.rpm * ( 360/60 ) * Time.deltaTime;

		// define a wheelhit object, this stores all of the data from the wheel collider and will allow us to determine
		// the slip of the tire.
		WheelHit CorrespondingGroundHit ;
		CorrespondingCollider.GetGroundHit(out CorrespondingGroundHit );
		
		// if the slip of the tire is greater than 2.0, and the slip prefab exists, create an instance of it on the ground at
		// a zero rotation.
		if ( Mathf.Abs( CorrespondingGroundHit.sidewaysSlip ) > 2.0f ) {
			if ( SlipPrefab ) {
				Instantiate( SlipPrefab, CorrespondingGroundHit.point, Quaternion.identity );
			}
		}
		
	}
}

var WheelL : WheelCollider; 
var WheelR : WheelCollider; 
var AntiRoll = 5000.0;

function FixedUpdate () 
{ 
    var hit : WheelHit;
    var travelL = 1.0; 
    var travelR = 1.0;

	var wheelLPosition = WheelL.transform.position + WheelL.transform.rotation * WheelL.center;
	var wheelRPosition = WheelR.transform.position + WheelR.transform.rotation * WheelR.center;
	
    var groundedL = WheelL.GetGroundHit(hit);   
    if (groundedL) 
        travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) 
                  / WheelL.suspensionDistance;

    var groundedR = WheelR.GetGroundHit(hit); 
    if (groundedR) 
        travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) 
                  / WheelR.suspensionDistance;

    var antiRollForce = (travelL - travelR) * AntiRoll;					
	
	if(rayCastWillNotHitGround(transform) || rayCastWillNotHitGround(WheelL.transform) || rayCastWillNotHitGround(WheelR.transform))
		return;	
			
    if (groundedL)
    	rigidbody.AddForceAtPosition(WheelL.transform.up * -antiRollForce, wheelLPosition); 	
    
    if (groundedR)
        rigidbody.AddForceAtPosition(WheelR.transform.up * antiRollForce, wheelRPosition);     
		
	DebugWheel(wheelLPosition, Mathf.Abs(antiRollForce));
    DebugWheel(wheelRPosition, Mathf.Abs(antiRollForce));
}

function DebugWheel(ntransform, force) 	{		
	Debug.DrawLine(ntransform, ntransform + new Vector3(0, force * .1f, 0), Color.red);
}

function rayCastWillNotHitGround(transformToCheck){
	var hits : RaycastHit[];
	hits = Physics.RaycastAll (transformToCheck.position, transformToCheck.up * -1, 1.0);	
	for (var i = 0;i < hits.Length; i++) {
		var hit : RaycastHit = hits[i];
		if(!hit.collider.GetType().Name.Equals("TerrainCollider"))			
			return true;
	}
	
	return false;
}
var WheelL : WheelCollider; 
var WheelR : WheelCollider; 
var AntiRoll = 5000.0;

function FixedUpdate () 
{ 
    var hit : WheelHit;
    var travelL = 1.0; 
    var travelR = 1.0;

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
    	rigidbody.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position); 	
    
    if (groundedR)
        rigidbody.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);     
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
using UnityEngine;
using System.Collections;

public class IADriver : MonoBehaviour 
{
	public Waypoints waypoints;
	public GameObject playerCar;
	public GameObject sphere;
	
	private Car car;
	private CarControl control;
	private MachineGunControl machineGunControl;

	void Start () 
	{
		car = GetComponent<Car>();
		machineGunControl = car.MachineGunControl;
		control = car.Control;
	}

	void Update() {
		Waypoints waypoint = findClosestWaypoint();
		
		waypoint = waypoint.next[0]; //fix it
		
		Vector3 waypointPosition = waypoint.transform.position;
		Vector3 carPosition = car.transform.position;
		Vector3 directionToWaypoint = waypointPosition - carPosition;
		
		control.Steer = Vector3.Dot(car.transform.right, directionToWaypoint.normalized) * 40f;
		
		sphere.transform.position = waypointPosition;

		float angleFromPlayer = AngleFrom(playerCar.transform.position);
		machineGunControl.Shooting = (angleFromPlayer >= 80f && angleFromPlayer <= 90f);

		float angleFromWaypoint = AngleFrom(waypointPosition);

		if(angleFromWaypoint >= 0 && angleFromWaypoint <= 180) {
			control.Mode = TransmissionMode.Drive;
			control.Accelerator = Mathf.Sin(angleFromWaypoint * Mathf.Deg2Rad) * 0.9f;
		} else {
			control.Steer = -control.Steer;
			control.Mode = TransmissionMode.Reverse;
			control.Accelerator = 1.0f;
		}
	}

	private float AngleFrom(Vector3 position)
	{
		Vector3 carPosition = car.transform.position;
		Vector3 direction = car.transform.InverseTransformDirection(position - carPosition);
		
		return 360f - (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 270f);
	}

	private Waypoints findClosestWaypoint() {
		ArrayList list = new ArrayList();
		Stack stack = new Stack();
		
		Vector3 carPosition = car.transform.position;
		float minDistance = float.MaxValue;
		Waypoints found = null;

		if(waypoints)
			stack.Push(waypoints);
		
		while(stack.Count > 0) {
			Waypoints actual = (Waypoints)stack.Pop();
			list.Add(actual);
			
			foreach(Waypoints w in actual.next) {
				if(!list.Contains(w))
					stack.Push(w);
			}
			
			Vector3 waypointPosition = actual.transform.position;
			
			float distance = Vector3.Distance(carPosition, waypointPosition);
			
			if(distance < minDistance) {
				minDistance = distance;
				found = actual;
			}
		}
		
		return found;
	}
}

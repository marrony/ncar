using UnityEngine;
using System.Collections;

public class IADriver : MonoBehaviour 
{
	public Waypoints waypoints;
	public GameObject sphere;
	
	private Car car;
	private CarControl control;

	void Start () 
	{
		car = GetComponent<Car>();
		control = car.Control;
		control.Accelerator = 0.5f;
	}
	
	void Update() {
		control.Accelerator = 0.7f;
		Waypoints waypoint = findClosestWaypoint();
		
		waypoint = waypoint.next[0]; //fix it
		
		Vector3 waypointPosition = waypoint.transform.position;
		Vector3 carPosition = car.transform.position;
		Vector3 directionToWaypoint = waypointPosition - carPosition;
		
		control.Steer = Vector3.Dot(car.transform.right, directionToWaypoint.normalized) * 40f;
		
		sphere.transform.position = waypointPosition;
	}
	
	private Waypoints findClosestWaypoint() {
		ArrayList list = new ArrayList();
		Stack stack = new Stack();
		
		Vector3 carPosition = car.transform.position;
		float minDistance = float.MaxValue;
		Waypoints found = null;

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

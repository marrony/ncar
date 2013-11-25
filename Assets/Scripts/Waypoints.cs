using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {
	
	public Waypoints[] next;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos() {
		if(next.Length == 0)
			return;

		//Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.color = new Color(1, 0, 0, .5f);
		Gizmos.DrawWireCube(transform.position, Vector3.one);
		
		foreach(Waypoints ways in next) {
			if(ways) {
				Gizmos.DrawLine(transform.position, ways.transform.position);
			}
		}
	}
}

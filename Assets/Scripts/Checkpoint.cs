using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	void Start () 
	{
		gameObject.AddComponent<BoxCollider>();
		collider.isTrigger = true;
	}

	void OnTriggerEnter (Collider other) 
	{
		Debug.Log("Checkpoint!");
	}
	
	void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.color = new Color(0, 0, 1, .5f);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		
		Gizmos.color = new Color(0, 0, 1, .1f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}

}

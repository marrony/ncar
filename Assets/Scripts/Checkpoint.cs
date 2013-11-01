using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {
	
	public Checkpoint next;
	
	public event OnCheckpointEnterEventHandler OnCheckpointEnter = delegate {};
	
	private HashSet<GameObject> waitingFor = new HashSet<GameObject>();
	
	void Start () 
	{
		gameObject.AddComponent<BoxCollider>();
		collider.isTrigger = true;
	}

	void OnTriggerEnter (Collider collider) 
	{
		GameObject gameObject = collider.transform.root.gameObject;
		
		if (!waitingFor.Contains(gameObject))
			return;
		
		waitingFor.Remove(gameObject);
		next.WaitFor(gameObject);
		OnCheckpointEnter(gameObject);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.color = new Color(0, 0, 1, .5f);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		
		Gizmos.color = new Color(0, 0, 1, .1f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
	
	public void WaitFor(GameObject gameObject) 
	{
		waitingFor.Add(gameObject);
	}

}

public delegate void OnCheckpointEnterEventHandler(GameObject gameObject);
using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float velocity;	
	
	// Update is called once per frame
	void Update () {		
		transform.Translate(new Vector3(0, 0, velocity * Time.deltaTime));
	}
	
	void OnTriggerEnter (Collider other) {
		DestroyObject(gameObject);
	}
}

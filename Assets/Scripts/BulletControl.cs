using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float velocity;
	public int timeOut;
	
	// Update is called once per frame
	void Update () {		
		transform.Translate(new Vector3(0, 0, velocity * Time.deltaTime));
	}
	
	void Awake (){
		Invoke ("DestroyNow", timeOut);
	}
	
	void DestroyNow (){
		DestroyObject(gameObject);
	}
}

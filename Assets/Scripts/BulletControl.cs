using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float velocity;	
	public float damage;
	public GameObject shooter {get; set; } 
	
	// Update is called once per frame
	void Update () {		
		transform.Translate(new Vector3(0, 0, velocity * Time.deltaTime));
	}
	
	void OnTriggerEnter (Collider other) {
		GameObject rootGameObject = findRootGameObject(other.gameObject);
		if(shooter == rootGameObject)
		   return;

		CarDamage carDamage = rootGameObject.GetComponent<CarDamage>();		
		if(carDamage == null)
			return;

		DestroyObject(gameObject);				
		carDamage.Inflict(damage);		
	}
	
	private GameObject findRootGameObject(GameObject obj){				
		if(obj.transform.parent != null)
			return findRootGameObject(obj.transform.parent.gameObject);
		return obj;
	}
}

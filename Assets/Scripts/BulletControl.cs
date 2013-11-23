using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float velocity;	
	public float damage;

	void Update () {		
		transform.Translate(new Vector3(0, 0, velocity * Time.deltaTime));
	}

	void OnCollisionEnter(Collision other) {
		DestroyObject(gameObject);
		
		CarDamage carDamage = findRootGameObject(other.gameObject).GetComponent<CarDamage>();		
		if(carDamage == null)
			return;
				
		carDamage.Inflict(damage);		
	}
	
	private GameObject findRootGameObject(GameObject obj){				
		if(obj.transform.parent != null)
			return findRootGameObject(obj.transform.parent.gameObject);
		return obj;
	}
}

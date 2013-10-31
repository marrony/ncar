using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {
	public Transform fireOrigin;
	public GameObject bullet;
	public float rateOfFire;
	private float currentTime = 0;
		
	void Update () {
		currentTime += Time.deltaTime;
		
		if(currentTime >= rateOfFire && Input.GetKey(KeyCode.LeftControl)){
			GameObject newBullet = Instantiate(bullet, fireOrigin.position, fireOrigin.rotation) as GameObject;						
			currentTime = 0;			
		}
	}
	
}

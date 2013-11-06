using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {
	public Transform fireOrigin;
	public GameObject bullet;
	public float rateOfFire;
	public ParticleSystem particles;
	private float currentTime = 0;	
		
	void Update () {
		currentTime += Time.deltaTime;
		
		if(currentTime >= rateOfFire && Input.GetKey(KeyCode.LeftControl)){
			Instantiate(bullet, fireOrigin.position, fireOrigin.rotation);						
			particles.Stop();
			particles.Play();
			currentTime = 0;			
		}
	}
	
}

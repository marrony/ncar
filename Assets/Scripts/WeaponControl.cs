using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {
	public Transform fireOrigin;
	public GameObject bullet;
	public float rateOfFire;
	public ParticleSystem particles;
	public AudioSource soundFX;
	private float currentTime = 0;	
		
	void Update () {
		currentTime += Time.deltaTime;
		
		if(currentTime >= rateOfFire && Input.GetKey(KeyCode.LeftControl)){
			Instantiate(bullet, fireOrigin.position, fireOrigin.rotation);						
			
			startParticles();
			startSound();
			
			currentTime = 0;			
		}
	}
	
	private void startParticles(){
		particles.Stop();
		particles.Play();
	}
	
	private void startSound(){
		AudioSource fireSound = Instantiate(soundFX) as AudioSource;			
		fireSound.Play();
	}
}

using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour 
{
	public Transform fireOrigin;
	public GameObject bullet;
	public float rateOfFire;
	public ParticleSystem particles;
	public AudioSource soundFX;

	private float currentTime = 0;
	private bool wrecked = false;

	public MachineGunControl Control { get; private set; }

	void Start()
	{
		Control = new MachineGunControl();
	}
		
	void Update () 
	{
		if(wrecked)
			return; 
		
		currentTime += Time.deltaTime;
		
		if(currentTime >= rateOfFire && Control.Shooting) {
			Instantiate(bullet, fireOrigin.position, fireOrigin.rotation);						
			
			StartParticles();
			StartSound();
			
			currentTime = 0;			
		}
	}
	
	public void Wreck()
	{
		wrecked = true;
	}
	
	private void StartParticles()
	{
		particles.Stop();
		particles.Play();
	}
	
	private void StartSound()
	{
		AudioSource fireSound = Instantiate(soundFX) as AudioSource;			
		fireSound.Play();
	}
}

public class MachineGunControl 
{
	public bool Shooting { get; set; }
}

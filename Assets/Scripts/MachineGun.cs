using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour 
{
	public GameObject bullet;
	public float rateOfFire;
	public ParticleSystem particles;
	public AudioSource soundFX;

	private Transform fireOrigin;
	private float currentTime = 0;
	private bool wrecked = false;

	public MachineGunControl Control { get; private set; }

	void Start()
	{
		Control = new MachineGunControl();
		fireOrigin = transform.Find("FireOrigin");
	}
		
	void Update () 
	{
		if(wrecked || bullet == null || fireOrigin == null)
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
		AudioSource fireSound = Instantiate(soundFX, fireOrigin.position, fireOrigin.rotation) as AudioSource;			
		fireSound.transform.position = fireOrigin.position;
		fireSound.Play();
	}
}

public class MachineGunControl 
{
	public bool Shooting { get; set; }
}

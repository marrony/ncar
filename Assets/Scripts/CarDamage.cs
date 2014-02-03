using UnityEngine;
using System.Collections;

public class CarDamage : MonoBehaviour 
{
	public float health;
	public ParticleSystem wreckParticles;
	public ParticleSystem damageParticles;

	private float maxHealth;
	private Car car;

	void Start() 
	{
		car = GetComponent<Car>();
		maxHealth = health;
		damageParticles.maxParticles = 0;
		damageParticles.Play();
	}
	
	void Update()
	{
		if(health > 0 || wreckParticles.isPlaying)
			return;		

		damageParticles.Stop();
		wreckParticles.Play();
		car.Wreck();
	}
	
	public void Inflict(float damage)
	{
		health -= damage;		
		damageParticles.maxParticles = Mathf.Max((int) (10 * (1 - health / (maxHealth / 2))), 0);
	}
}

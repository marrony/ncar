using UnityEngine;
using System.Collections;

public class CarDamage : MonoBehaviour 
{
	public float health;
	public ParticleSystem particles;

	private Car car;

	void Start() 
	{
		car = GetComponent<Car>();
	}
	
	void Update()
	{
		if(health > 0 || particles.isPlaying)
			return;		
		
		particles.Play();
		car.Wreck();
	}
	
	public void Inflict(float damage)
	{
		health -= damage;		
	}
}

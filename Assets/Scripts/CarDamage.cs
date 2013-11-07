using UnityEngine;
using System.Collections;

public class CarDamage : MonoBehaviour {
	public float health;
	public ParticleSystem particles;
	
	void Update(){
		if(health > 0 || particles.isPlaying)
			return;		
		
		particles.Play();
		GetComponent<Car>().Wreck();
		GetComponent<WeaponControl>().Wreck();
	}
	
	public void Inflict(float damage){
		health -= damage;		
	}
}

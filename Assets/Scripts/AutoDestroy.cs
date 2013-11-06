using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	public int timeOut;
	
	void Awake (){
		Invoke ("DestroyNow", timeOut);
	}
	
	void DestroyNow (){
		DestroyObject(gameObject);
	}	
}

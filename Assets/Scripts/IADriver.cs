using UnityEngine;
using System.Collections;

public class IADriver : MonoBehaviour 
{
	
	private CarControl control;

	void Start () 
	{
		control = GetComponent<Car>().Control;
		control.Accelerator = 1; //pushing like a hell
	}

}

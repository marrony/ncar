using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject maverick;

	void Start () {
		GameObject newMaverick = Instantiate(maverick, 
			new Vector3(200, 2, 260), 
			Quaternion.Euler(0, 230, 0)) as GameObject;
		
		mainCamera.target = newMaverick.transform;
	}

}

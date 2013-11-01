using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject maverick;
	public Checkpoint startLine;
	
	private int lap = 0;

	void Start () {
		GameObject playerCar = Instantiate(maverick, 
			new Vector3(200, 2, 260), 
			Quaternion.Euler(0, 230, 0)) as GameObject;
		
		mainCamera.target = playerCar.transform;
		
		startLine.WaitFor(playerCar);
		startLine.OnCheckpointEnter += (gameObject) => { 
			if (gameObject == playerCar)
				lap++; 
		};
	}
	
	void OnGUI() 
	{
		GUI.Label(new Rect(Screen.width/2, 10, 80, 20), "Lap " + lap);
	}

}

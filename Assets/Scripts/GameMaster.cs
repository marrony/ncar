using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject maverick;
	public Checkpoint startLine;
	
	public event OnLapChangeEventHandler OnLapChange = delegate {};
	
	private GameObject playerCar;
	private int lap = 0;

	void Start () {
		playerCar = Instantiate(maverick, 
			new Vector3(200, 2, 260), 
			Quaternion.Euler(0, 230, 0)) as GameObject;
		
		mainCamera.target = playerCar.transform;
		
		startLine.WaitFor(playerCar);
		startLine.OnCheckpointEnter += OnCheckpointEnter;
	}
	
	private void OnCheckpointEnter (GameObject gameObject)
	{
		if (gameObject == playerCar) {
			lap++; 
			OnLapChange(lap);
		}
	}
	
}

public delegate void OnLapChangeEventHandler(int newLap);
using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject maverick;
	public Checkpoint startLine;
	
	public event OnLapChangeEventHandler OnLapChange = delegate {};
	public event OnBestLapEventHandler OnBestLap = delegate {};
	
	private GameObject playerCar;
	private int lap = 0;
	private float lapBegin = 0;
	private float bestLap = float.PositiveInfinity;

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
			calculeteLapTime();
			lap++;
			OnLapChange(lap);
		}
	}
	
	private void calculeteLapTime ()
	{
		float lapTime = Time.realtimeSinceStartup - lapBegin;
		if (lapTime < bestLap && lap > 0) {
			bestLap = lapTime;
			OnBestLap(bestLap);
		}
		lapBegin = Time.realtimeSinceStartup;
	}
}

public delegate void OnLapChangeEventHandler(int newLap);
public delegate void OnBestLapEventHandler(float bestLap);
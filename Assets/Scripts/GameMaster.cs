using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject[] carsAvailable; //improve this reference... not a good idea to keep all cars reference 
	public GameObject maverick;
	public Checkpoint startLine;
	public Waypoints waypoints;
	
	public GameObject sphere;

	public event OnLapChangeEventHandler OnLapChange = delegate {};
	public event OnBestLapEventHandler OnBestLap = delegate {};
	public event OnRaceFinishesEventHandler OnRaceFinishes = delegate {};
	
	private GameObject playerCar;
	private int lap = 0;
	private int laps = 3;
	private float lapBegin = 0;
	private float bestLap = float.PositiveInfinity;

	void Start() {
		maverick = carsAvailable[CarSelectionCameraControl.targetIdx];
		CreatePlayerCar();
		CreateIACars();
	}

	private void CreatePlayerCar ()
	{
		Vector3 startingPosition = new Vector3(startLine.transform.position.x, startLine.transform.position.y, startLine.transform.position.z);
		startingPosition.x += 5;
		startingPosition.y -= 5;

		playerCar = Instantiate(maverick, 
        	startingPosition, 
			startLine.transform.rotation) as GameObject;
		
		playerCar.AddComponent<PlayerDriver>();

		mainCamera.target = playerCar.transform;
		
		startLine.WaitFor(playerCar);
		startLine.OnCheckpointEnter += OnStartLineEnter;

		Car car = playerCar.GetComponent<Car>();
		car.Debug = true;
	}
	
	private void CreateIACars ()
	{
		Vector3 startingPosition = new Vector3(startLine.transform.position.x, startLine.transform.position.y, startLine.transform.position.z);
		startingPosition.z += 5;
		startingPosition.y -= 5;
		startingPosition.x += 6;

		GameObject car = Instantiate(maverick, 
             startingPosition, 
            startLine.transform.rotation) as GameObject;
		
		IADriver iaDriver = car.AddComponent<IADriver>();
		iaDriver.waypoints = waypoints;
		iaDriver.playerCar = playerCar;
		iaDriver.sphere = sphere;
	}
	
	private void OnStartLineEnter (GameObject gameObject)
	{
		if (gameObject == playerCar)
			OnNewLap ();
	}

	private void OnNewLap ()
	{
		CalculeteLapTime();
		
		if (lap == laps) {
			RaceFinished();
		} else {
			lap++;
			OnLapChange(lap);
		}
	}
	
	private void CalculeteLapTime ()
	{
		float lapTime = Time.realtimeSinceStartup - lapBegin;
		if (lapTime < bestLap && lap > 0) {
			bestLap = lapTime;
			OnBestLap(bestLap);
		}
		lapBegin = Time.realtimeSinceStartup;
	}
	
	private void RaceFinished() 
	{
		RaceResult result = new RaceResult(1);
		OnRaceFinishes(result);
	}

	private void Update(){
		if(Input.GetKeyDown(KeyCode.Escape))
			FadeEffect.LoadLevel(0,1,1);
	}
}

public delegate void OnLapChangeEventHandler(int newLap);
public delegate void OnBestLapEventHandler(float bestLap);
public delegate void OnRaceFinishesEventHandler(RaceResult result);

public struct RaceResult 
{
	public readonly int playerPosition;
	
	public RaceResult(int playerPosition)
	{
		this.playerPosition = playerPosition;
	}	
}

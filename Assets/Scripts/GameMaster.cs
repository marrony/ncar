﻿using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	
	public SmoothFollow mainCamera;
	public GameObject maverick;
	public Checkpoint startLine;
	
	public event OnLapChangeEventHandler OnLapChange = delegate {};
	public event OnBestLapEventHandler OnBestLap = delegate {};
	public event OnRaceFinishesEventHandler OnRaceFinishes = delegate {};
	
	private GameObject playerCar;
	private int lap = 0;
	private int laps = 3;
	private float lapBegin = 0;
	private float bestLap = float.PositiveInfinity;

	void Start () 
	{
		CreatePlayerCar();
		CreateIACars();
	}

	private void CreatePlayerCar ()
	{
		playerCar = Instantiate(maverick, 
			new Vector3(200, 2, 260), 
			Quaternion.Euler(0, 230, 0)) as GameObject;
		
		playerCar.AddComponent<PlayerDriver>();
		
		mainCamera.target = playerCar.transform;
		
		startLine.WaitFor(playerCar);
		startLine.OnCheckpointEnter += OnStartLineEnter;
	}
	
	private void CreateIACars ()
	{
		GameObject car = Instantiate(maverick, 
			new Vector3(204, 2, 260), 
			Quaternion.Euler(0, 230, 0)) as GameObject;
		
		car.AddComponent<IADriver>();
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
﻿using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;
	public WheelCollider RearLeftWheel;
	public WheelCollider RearRightWheel;
	
	public float[] GearRatio;
	public int CurrentGear = 0;
	
	public float EngineTorque = 600.0f;
	public float MaxEngineRPM = 3000.0f;
	public float MinEngineRPM = 1000.0f;
		
	private float EngineRPM = 0.0f;

	void Start () {
		//Transform cg = transform.FindChild("CG");
		rigidbody.centerOfMass = new Vector3(0, -0.7f, 0);
	}

	void Update () {
		rigidbody.drag = rigidbody.velocity.magnitude / 250;
		
		EngineRPM = (RearLeftWheel.rpm + RearRightWheel.rpm) * GearRatio[CurrentGear];
		ShiftGears();
	
		/*
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0 ;
		if ( audio.pitch > 2.0 ) 
			audio.pitch = 2.0;
		*/

		var motorTorque = EngineTorque * Input.GetAxis("Vertical") - (CurrentGear * EngineTorque * .1f) ;
		
		if(Mathf.Abs(EngineRPM) > MaxEngineRPM + 100)
			motorTorque = 0;	
		
		RearLeftWheel.motorTorque = motorTorque;
		RearRightWheel.motorTorque = motorTorque;
		
		var steerAngle = 20 - (int) (rigidbody.velocity.magnitude * 0.2f);
		if(steerAngle < 5)
			steerAngle = 5;
				
		FrontLeftWheel.steerAngle = steerAngle * Input.GetAxis("Horizontal");
		FrontRightWheel.steerAngle = steerAngle * Input.GetAxis("Horizontal");
	}
	
	private void ShiftGears() {
		if ( EngineRPM >= MaxEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for (int i = 0; i < GearRatio.Length; i ++ ) {
				if ( (RearRightWheel.rpm + RearLeftWheel.rpm) * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for (int j = GearRatio.Length-1; j >= 0; j -- ) {
				if ( (RearRightWheel.rpm + RearLeftWheel.rpm) * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 20), "KM/h: " + (rigidbody.velocity.magnitude * 3.6f));
		GUI.Label(new Rect(10, 30, 100, 20), "RPM: " + (EngineRPM));
		GUI.Label(new Rect(10, 50, 100, 20), "Gear: " + (CurrentGear));
	}
	
} 
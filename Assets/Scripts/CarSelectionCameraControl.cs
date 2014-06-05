using UnityEngine;
using System;
using System.Collections;


public class CarSelectionCameraControl : MonoBehaviour {
	public static int targetIdx;
	public GameObject[] objects;
	public float[] distances;
	public float[] rotations;
	public float[] heights;

	void Start () {
		targetIdx = 1;
	}
	
	void LateUpdate () {
		if (objects == null || objects.Length == 0)
			return;

		float wantedHeight = objects[targetIdx].transform.position.y + heights[targetIdx];		
		float currentHeight = transform.position.y;

		Vector3 distanceDifference = Quaternion.Euler(0, rotations[targetIdx], 0) * Vector3.forward * distances[targetIdx];		

		float currentX = transform.position.x;
		float wantedX = objects[targetIdx].transform.position.x - distanceDifference.x;

		float currentZ = transform.position.z;
		float wantedZ = objects[targetIdx].transform.position.z - distanceDifference.z;

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 2 * Time.deltaTime);
		currentX = Mathf.Lerp(currentX, wantedX, 2 * Time.deltaTime);
		currentZ = Mathf.Lerp(currentZ, wantedZ, 2 * Time.deltaTime);

		transform.position = new Vector3(currentX , currentHeight, currentZ);

		Quaternion targetRotation = Quaternion.LookRotation(objects[targetIdx].transform.position - transform.position);		
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			targetIdx -= targetIdx > 0 ? 1 : 0;		

		if(Input.GetKeyDown(KeyCode.RightArrow))
			targetIdx += targetIdx < objects.Length - 1 ? 1 : 0;		

		if(Input.GetKeyDown(KeyCode.Return) && targetIdx != 1)
			FadeEffect.LoadLevel(TrackSelection.targetIdx + 2,1,1);
	}
}

using UnityEngine;
using System.Collections;

public class TrackSelection : MonoBehaviour {
	public static int targetIdx;
	public GameObject selector;
	public Transform[] positions;

	void Start () {
		targetIdx = 0;
	}
	
	void LateUpdate () {
		if (selector == null || positions == null || positions.Length == 0)
			return;

		float currentX = selector.transform.position.x;
		float wantedX = positions[targetIdx].position.x;

		currentX = Mathf.Lerp(currentX, wantedX, 4 * Time.deltaTime);

		Vector3 curPosition = selector.transform.position;

		selector.transform.position = new Vector3(currentX, curPosition.y, curPosition.z);

	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			targetIdx -= targetIdx > 0 ? 1 : 0;		
		
		if(Input.GetKeyDown(KeyCode.RightArrow))
			targetIdx += targetIdx < positions.Length - 1 ? 1 : 0;		
		
		if(Input.GetKeyDown(KeyCode.Return))
			FadeEffect.LoadLevel(1,.5f,1f);
	}
}

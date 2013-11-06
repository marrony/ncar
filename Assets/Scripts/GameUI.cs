using UnityEngine;
using System;
using System.Collections;

public class GameUI : MonoBehaviour {
	
	public GameMaster gameMaster;
	public Font font;
		
	private int lap;
	private TimeSpan bestLap = TimeSpan.Zero;
	private bool showRaceResult = false;
	private RaceResult result;

	private Rect hudRect;
	private Rect lapRect;
	private Rect bestLabelRect;
	private Rect bestTimeRect;
	private Rect resultRect;
	private GUIStyle hudStyle;
	private GUIStyle titleStyle;
	private GUIStyle labelStyle;
	private GUIStyle valueStyle;
	private GUIStyle bestTimeStyle;
	
	private float bestTimeHighlight = 0;

	void Start () 
	{
		gameMaster.OnLapChange += (newLap) => lap = newLap;
		gameMaster.OnBestLap += (bestLap) => { 
			this.bestLap = TimeSpan.FromSeconds(bestLap);
			bestTimeHighlight = 1;
		};
		gameMaster.OnRaceFinishes += (result) => {
			showRaceResult = true;
			this.result = result;
		};
		
		hudRect = new Rect(Screen.width - 130, 10, 120, 85);
		lapRect = new Rect(hudRect.x + 15, hudRect.y + 5, 80, 20);
		bestLabelRect = new Rect(hudRect.x + 15, hudRect.y + 40, 80, 20);
		bestTimeRect = new Rect(hudRect.x + 15, hudRect.y + 57, 80, 20);
		resultRect = new Rect(Screen.width/2 - 50, 100, 100, 30);
		
		hudStyle = new GUIStyle();
		hudStyle.normal.background = createFlatTexture(new Color(0, 0, 0, 0.4f));
		
		titleStyle = new GUIStyle();
		titleStyle.normal.textColor = Color.white;
		titleStyle.fontSize = 26;
		titleStyle.font = font;
		
		labelStyle = new GUIStyle(titleStyle);
		labelStyle.normal.textColor = Color.gray;
		labelStyle.fontSize = 14;
		
		valueStyle = new GUIStyle(titleStyle);
		valueStyle.fontSize = 16;
		
		bestTimeStyle = new GUIStyle(valueStyle);
	}
	
	void Update() 
	{
		if (bestTimeHighlight > 0) {
			bestTimeHighlight -= Time.deltaTime * 0.8f;
			bestTimeStyle.normal.textColor = Color.Lerp(valueStyle.normal.textColor, Color.red, bestTimeHighlight);
		}
	}
	
	void OnGUI() 
	{
		GUI.Box(hudRect, GUIContent.none, hudStyle);
		GUI.Label(lapRect, "Lap " + lap, titleStyle);
		GUI.Label(bestLabelRect, "Best Lap", labelStyle);
		GUI.Label(bestTimeRect, formatTimeSpan(bestLap), bestTimeStyle);
		
		if (showRaceResult) {
			string message = result.playerPosition == 1 ? "You Win!" : "Game Over";
			GUI.Label(resultRect, message, titleStyle);
		}
			
	}
	
	private static Texture2D createFlatTexture(Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		return texture;
	}
	
	private static String formatTimeSpan(TimeSpan ts) 
	{
		return string.Format("{0}'{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
	}
}

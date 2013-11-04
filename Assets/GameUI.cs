using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {
	
	public GameMaster gameMaster;
	public Font font;
	
	private int lap;
	private Rect hudRect;
	private Rect lapRect;
	private GUIStyle hudStyle;
	private GUIStyle titleStyle;

	void Start () 
	{
		gameMaster.OnLapChange += (newLap) => lap = newLap;
		
		hudRect = new Rect(Screen.width - 130, 10, 120, 45);
		lapRect = new Rect(hudRect.x + 15, hudRect.y + 5, 80, 20);

		hudStyle = new GUIStyle();
		hudStyle.normal.background = createFlatTexture(new Color(0, 0, 0, 0.4f));
		
		titleStyle = new GUIStyle();
		titleStyle.normal.textColor = Color.white;
		titleStyle.fontSize = 26;
		titleStyle.font = font;
	}
	
	void OnGUI() 
	{
		GUI.Box(hudRect, GUIContent.none, hudStyle);
		GUI.Label(lapRect, "Lap " + lap, titleStyle);
	}
	
	private static Texture2D createFlatTexture(Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		return texture;
	}
}

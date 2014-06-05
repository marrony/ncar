using UnityEngine;
using System.Collections;
 
public class FadeEffect : MonoBehaviour
{
	private static FadeEffect m_Instance = null;
	private Material m_Material = null;
	private int m_LevelIndex = 0;
	private bool m_Fading = false;
 
	private static FadeEffect Instance {
		get {
			if (m_Instance == null) {
				m_Instance = (new GameObject ("FadeEffect")).AddComponent<FadeEffect> ();
			}
			return m_Instance;
		}
	}

	public static bool Fading {
		get { return Instance.m_Fading; }
	}
 
	private void Awake ()
	{
		DontDestroyOnLoad (this);
		m_Instance = this;
		m_Material = new Material ("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
	}
 
	private void DrawQuad (float aAlpha)
	{
		Color newColor = Color.black;
		newColor.a = aAlpha;
		m_Material.SetPass (0);
		GL.Color (newColor);
		GL.PushMatrix ();
		GL.LoadOrtho ();
		GL.Begin (GL.QUADS);
		GL.Vertex3 (0, 0, -1);
		GL.Vertex3 (0, 1, -1);
		GL.Vertex3 (1, 1, -1);
		GL.Vertex3 (1, 0, -1);
		GL.End ();
		GL.PopMatrix ();
	}
 
	private IEnumerator Fade (float aFadeOutTime, float aFadeInTime)
	{
		float t = 0.0f;
		while (t<1.0f) {
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01 (t + Time.deltaTime / aFadeOutTime);
			DrawQuad (t);
		}
		Application.LoadLevel (m_LevelIndex);
		while (t>0.0f) {
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01 (t - Time.deltaTime / aFadeInTime);
			DrawQuad (t);
		}
		m_Fading = false;
	}

	private void StartFade (float aFadeOutTime, float aFadeInTime)
	{
		m_Fading = true;
		StartCoroutine (Fade (aFadeOutTime, aFadeInTime));
	}
 
	public static void LoadLevel (int aLevelIndex, float aFadeOutTime, float aFadeInTime)
	{
		if (Fading)
			return;
		Instance.m_LevelIndex = aLevelIndex;
		Instance.StartFade (aFadeOutTime, aFadeInTime);
	}
}
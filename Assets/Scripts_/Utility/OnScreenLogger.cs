using UnityEngine;
using System.Collections;

public class OnScreenLogger : MonoBehaviour {

	public static OnScreenLogger Instance;
	public string streamingError = "";

	private Vector2 scrollPosition = Vector2.zero;
	bool isDebugging = false;

	void Awake () {
		
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		else {
			DestroyImmediate (this);
		}
	}
	
	void Start ()
	{
		Application.RegisterLogCallback (HandleLog);
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
        //if (type == LogType.Error ||
        //    type == LogType.Exception ||
        //    type == LogType.Log)
		{
			if (streamingError.Length > 8000)
			{
				streamingError = streamingError.Remove (0, 2000);
			}
			streamingError += "\n" + logString + "\n >>> " + stackTrace;
		}
	}
	bool cameraZoom;
	void OnGUI ()
	{
		isDebugging = GUI.Toggle (new Rect (0, Screen.height - 50, 100, 50), isDebugging, "Debug");

		if (! isDebugging)
			return;

		GUILayout.BeginArea (new Rect  (10, 10, 400, 400));
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (400), GUILayout.Height (400));
		GUILayout.TextField ("\nLogs\n : " + streamingError, "Label");
		GUILayout.EndScrollView ();
		GUILayout.EndArea ();
	}

}


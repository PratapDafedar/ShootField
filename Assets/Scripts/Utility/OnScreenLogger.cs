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
		}
		else {
			DestroyImmediate (this);
		}
		Application.RegisterLogCallback (HandleLog);
	}
	
	void HandleLog(string logString, string stackTrace, LogType type)
	{
        if (streamingError.Length > 5000)
            streamingError = streamingError.Remove(0, 1000);
        //if (type == LogType.Error ||
        //    type == LogType.Exception)
		{
			streamingError += "\n" + logString + "\n >>> " + stackTrace;
		}
	}
	
	void OnGUI ()
	{
		isDebugging = GUI.Toggle (new Rect (0, Screen.height - 25, 100, 25), isDebugging, "Debug");

		if (! isDebugging)
			return;

		GUILayout.BeginArea (new Rect  ((Screen.width - (Screen.width / 2)), 10, (Screen.width / 2) - 20, Screen.height * 0.8f));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width((Screen.width / 2) - 20), GUILayout.Height(Screen.height * 0.8f));
		GUILayout.TextField ("\nLogs\n : " + streamingError, "Label");
		GUILayout.EndScrollView ();
		GUILayout.EndArea ();
	}

}


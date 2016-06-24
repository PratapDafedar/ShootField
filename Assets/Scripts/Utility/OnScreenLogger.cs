using UnityEngine;
using System.Collections;

public class OnScreenLogger : MonoBehaviour {

	public static OnScreenLogger Instance;
	public string streamingError = "";

	private Vector2 scrollPosition = Vector2.zero;
	bool isDebugging = false;

	void Awake () 
    {		
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		else {
			DestroyImmediate (this);
		}

		StandaloneWindowsUtility.SetWindowPosition (0, 150);
	}
	
	void Start ()
	{
		Application.logMessageReceived += HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
//		if (type == LogType.Error ||
//		    type == LogType.Exception ||
//            type == LogType.Warning )
		{
			streamingError += "\n" + logString;// + "\n >>> " + stackTrace;
		}

        if (streamingError.Length > 10000)
        {
            streamingError = streamingError.Remove(0, 3000);
        }
	}
	
	void OnGUI ()
	{
		isDebugging = GUI.Toggle (new Rect (Screen.width * 0.01f, Screen.height - 25, 100, 25), isDebugging, "Debug");

		if (! isDebugging)
			return;

		GUILayout.BeginArea (new Rect  (10, 10, Screen.width / 2f, Screen.height * 0.8f));
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (Screen.width / 2f), GUILayout.Height (Screen.height * 0.8f));
		GUILayout.TextField ("\nLogs\n : " + streamingError, "Label");
		GUILayout.EndScrollView ();
		GUILayout.EndArea ();

		if (GUI.Button (new Rect (Screen.width * 0.1f, Screen.height - 50, 80, 40), "clear")) {
			streamingError = "";
		}
	}

}


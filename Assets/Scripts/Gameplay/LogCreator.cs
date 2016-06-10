using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogCreator : MonoBehaviour 
{
	private static LogCreator Instance = null;

	public InputField logInput;
	public GameObject logClone;
	public Transform parentTransform;
	private string err = "Command Not Found";

	void Awake()
	{
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy (this);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow) && logInput.isFocused) {
			SendLog ();
		}
	}

	public void SendLog()
	{
		string enteredStr = logInput.text;
		CloneLog (enteredStr);
	}

	public void CloneLog(string contentStr)
	{
		if (contentStr.Length > 0) {
			GameObject cloneObj = Instantiate (logClone);
		
			if (contentStr.Length > 100) {
				float contentHeight = contentStr.Length / 100;
				contentHeight = Mathf.Ceil (contentHeight);
				cloneObj.GetComponent<LayoutElement> ().minHeight = contentHeight * 25f;
			}

			cloneObj.GetComponent<Text> ().text = contentStr;
			cloneObj.transform.SetParent (parentTransform, false);

			logInput.text = "";
			if (!contentStr.Equals (err)) {
				RunCommand (contentStr);
			}
		}
	}

	public void RunCommand(string commandToExec)
	{
		switch (commandToExec) 
		{
		case "clr":
			foreach (Transform child in parentTransform)
			{
				if (parentTransform.childCount > 0) {
					Destroy (child.gameObject);
				}
			}
			break;

		default:
			CloneLog (err);
			break;	
		}
	}

	public static void PrintLog(string sentStr)
	{
		Instance.CloneLog (sentStr);
	}
}
using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MGameMenu))]
public class MGameMenuEditor : Editor {
	private MGameMenu edit;		

	public void OnEnable(){
		edit = (MGameMenu) target;		
	}
	
	public override void OnInspectorGUI () {
		edit.useExit = EditorGUILayout.Toggle("Use exit button: ", edit.useExit);
		edit.useChat = EditorGUILayout.Toggle("Use chat: ", edit.useChat);
		edit.usePlayerList = EditorGUILayout.Toggle("Use player list: ", edit.usePlayerList);	
		
	}
}

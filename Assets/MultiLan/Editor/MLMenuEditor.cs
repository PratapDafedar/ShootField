using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MLMenu))]
public class MLMenuEditor : Editor {
	private MLMenu edit;	
	
	public void OnEnable(){
		edit = (MLMenu) target;	
	}//OnEnable
	
	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("NETWORK GAME PARAMETERS", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);		
		edit.defaultPlayerName = EditorGUILayout.TextField("Default player name: ", edit.defaultPlayerName);
		edit.useJoinIP = EditorGUILayout.Toggle("Use \"join form IP\" menu" , edit.useJoinIP);
		if(!edit.useJoinIP){
			edit.useNetworkGames = true;
		}
		
		edit.useNetworkGames = EditorGUILayout.Toggle("Use \"view games\" menu" , edit.useNetworkGames);
		if(!edit.useNetworkGames){
			edit.useJoinIP = true;
		}	
	}//OnInspectorGUI	
	
}//MLMenuEditor

using UnityEngine;
using UnityEditor; 
using System.Collections;
using System;

[CustomEditor(typeof(MWaitRoom))]
public class MWaitRoomEditor : Editor {		
	private MWaitRoom edit;	
		
	public void OnEnable(){
		edit = (MWaitRoom) target;	
	}
	
	public override void OnInspectorGUI () {	
		EditorGUILayout.LabelField("Global parameters", EditorStyles.boldLabel);		
		edit.useLoadTimer  = EditorGUILayout.Toggle("Use load timer:", edit.useLoadTimer);	
		if(edit.useLoadTimer){
			edit.loadTimerTime = EditorGUILayout.IntField("Timer time:", edit.loadTimerTime);
			edit.useLoadChatTimer  = EditorGUILayout.Toggle("Display on chat:", edit.useLoadChatTimer);	
		}					
	}
}

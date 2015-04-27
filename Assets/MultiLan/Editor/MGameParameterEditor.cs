using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MGameParameter))]
public class MGameParameterEditor : Editor {
	private MGameParameter edit;	
	
	public void OnEnable(){
		edit = (MGameParameter)target;	
	}//OnEnable
	
	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("GLOBALS PARAMETERS", EditorStyles.boldLabel);
		edit.canJoinStartedGame = EditorGUILayout.Toggle("Can join started game" , edit.canJoinStartedGame);
		edit.useFilters = EditorGUILayout.Toggle("Use filters" , edit.useFilters);
		edit.viewFullGames = EditorGUILayout.Toggle("Display full games" , edit.viewFullGames);
		edit.displayPlayerName = EditorGUILayout.Toggle("Display player name" , edit.displayPlayerName);
		if(edit.displayPlayerName){
			edit.playerNameVisibility = EditorGUILayout.FloatField("Player name visibility", edit.playerNameVisibility);
		}
		edit.defaultPort = EditorGUILayout.IntField("Default port", edit.defaultPort);
		edit.sendRate = EditorGUILayout.IntField("Sendrate", edit.sendRate);
		edit.rebuildTime = EditorGUILayout.IntField("Rebuild (sec)", edit.rebuildTime);
		edit.rebuildWaitPlayers = EditorGUILayout.IntField("Wailt players (sec)", edit.rebuildWaitPlayers);
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("PLAYERS NUMBER", EditorStyles.boldLabel);
		edit.minPlayers = EditorGUILayout.IntField("Min players", edit.minPlayers);
		edit.maxPlayers = EditorGUILayout.IntField("Max players", edit.maxPlayers);
		edit.pairPlayers = EditorGUILayout.Toggle("Only pairs players", edit.pairPlayers);
		edit.persPlayers = EditorGUILayout.Toggle("Use personnal options", edit.persPlayers);
		if(edit.persPlayers){
			if(GUILayout.Button("Add option", GUILayout.Width(100))){	
				AddPlayerOption();
			}
			EditorGUILayout.Space();
			if(edit.persPlayersOptions.Length == 0){
				edit.persPlayersOptions = new int[1];
			}
			for(int i = 0; i < edit.persPlayersOptions.Length; i++){
				edit.persPlayersOptions[i] = EditorGUILayout.IntField("Option " + (i+1), edit.persPlayersOptions[i]);
				if(GUILayout.Button("Remove option", GUILayout.Width(100))){	
					RemovePlayerOption(i);
				}
			}			
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("MAPS", EditorStyles.boldLabel);
		edit.displayMapScreen  = EditorGUILayout.Toggle("Display maps screen :", edit.displayMapScreen);	
		if(edit.displayMapScreen){
			edit.mapScreenX = EditorGUILayout.IntField("Maps screen width:", edit.mapScreenX);
			edit.mapScreenY = EditorGUILayout.IntField("Maps screen height:", edit.mapScreenY);
		}
		if(GUILayout.Button("Add map", GUILayout.Width(100))){	
				AddMap();
			}
			EditorGUILayout.Space();
		for(int i = 0; i < edit.maps.Length; i++){
			edit.maps[i] = EditorGUILayout.TextField("Map " + (i+1), edit.maps[i]);
			edit.mapsScreen[i] = EditorGUILayout.ObjectField(edit.mapsScreen[i], typeof(Texture), false, GUILayout.Width(100)) as Texture;
			if(GUILayout.Button("Remove map", GUILayout.Width(100))){	
				RemoveMap(i);
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("CALL INTERVALS [MO]", EditorStyles.boldLabel);
		edit.gameCallInterval = EditorGUILayout.IntField("Game calls interval (sec): ", edit.gameCallInterval);
		EditorGUILayout.LabelField("(The interval between the messages sent by the game's host)");

		EditorGUILayout.Space();
		edit.playerCallInterval = EditorGUILayout.IntField("Player calls interval (sec): ", edit.playerCallInterval);
		EditorGUILayout.LabelField("(The interval between the messages sent by any online player)");
		
	}//OnInspectorGUI	
	
	private void RemovePlayerOption(int index){
		int[] playerOptions = new int[edit.persPlayersOptions.Length-1];
		int j = 0;
		for(int i = 0; i < edit.persPlayersOptions.Length; i++){
			if(i != index){
				playerOptions[j] = edit.persPlayersOptions[i];
				j++;
			}
			
		}
		edit.persPlayersOptions = playerOptions;
	}//RemovePlayerOption
	
	private void AddPlayerOption(){
		int[] playerOptions = new int[edit.persPlayersOptions.Length+1];
		for(int i = 0; i < edit.persPlayersOptions.Length; i++){
			playerOptions[i] = edit.persPlayersOptions[i];			
		}
		edit.persPlayersOptions = playerOptions;
	}//AddPlayerOption
	
	private void RemoveMap(int index){
		string[] maps = new string[edit.maps.Length-1];
		Texture[] mapsScreen = new Texture[edit.mapsScreen.Length-1];
		int j = 0;
		for(int i = 0; i < edit.maps.Length; i++){
			if(i != index){
				maps[j] = edit.maps[i];
				mapsScreen[j] = edit.mapsScreen[i];
				j++;
			}
			
		}
		edit.maps = maps;
		edit.mapsScreen = mapsScreen;
	}//RemoveMap
	
	
	private void AddMap(){
		string[] maps = new string[edit.maps.Length+1];
		Texture[] mapsScreen = new Texture[edit.mapsScreen.Length+1];
		for(int i = 0; i < edit.maps.Length; i++){
			maps[i] = edit.maps[i];		
			mapsScreen[i] = edit.mapsScreen[i];		
		}
		edit.maps = maps;
		edit.mapsScreen = mapsScreen;
	}//AddMap
	
}//MGameParameterEditor


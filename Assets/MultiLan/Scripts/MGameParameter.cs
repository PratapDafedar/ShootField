using UnityEngine;
using System.Collections;
using System;

public class MGameParameter : MonoBehaviour {
	// Global parameters
	public bool canJoinStartedGame=true;
	public bool displayPlayerName=true;
	public float playerNameVisibility = 0.5f;
	public int defaultPort = 25565; // default port	
	public int rebuildTime=60;
	public int rebuildWaitPlayers=5;
	public int gameCallInterval=10;
	public int sendRate = 60;
	public int playerCallInterval=60;
	public bool useFilters = true;
	public bool viewFullGames = true;
	
	// Players numbers	
	public int minPlayers = 1; // Maxiumum players
	public int maxPlayers = 20; // Maxiumum players
	public bool pairPlayers = true; // Does players number must be pair ?
	public bool persPlayers = false; // User personal players number options
	public int[] persPlayersOptions;
	public int[] playerOptions;
	
	//--- Maps
	public string[] maps=new string[1];
	public Texture[] mapsScreen;
	public bool displayMapScreen = true;
	public int mapScreenX = 150;
	public int mapScreenY = 150;
	
	void Awake(){
		DontDestroyOnLoad(this);
        this.name = "LevelOption";
		DefinePlayersNbr();
	}//Awake	
	
	public void DefinePlayersNbr(){
		if(persPlayersOptions != null && persPlayers){
			if(persPlayersOptions.Length > 0){
				Array.Sort(persPlayersOptions);
				minPlayers = persPlayersOptions[0];
				maxPlayers = persPlayersOptions[persPlayersOptions.Length-1];
				playerOptions = persPlayersOptions;
			} else {
				persPlayers = false;				
			}
		}	
		if(!persPlayers){
			if(pairPlayers){
				playerOptions = new int[((int)(maxPlayers-minPlayers)/2) +1];
				int i = 0;
				for(int p = minPlayers; p <= maxPlayers; p+=2){
					playerOptions[i] = p; 
					i++;
				}
			} else {
				playerOptions = new int[maxPlayers-minPlayers+1];
				int i = 0;
				for(int p = minPlayers; p <= maxPlayers; p++){
					playerOptions[i] = p; 
					i++;
				}
			}
		}
	}//DefinePlayersNbr
	
	// Function for add player on the players selector 
	public int[] MorePlayers(int[] playersParam){
		if(playersParam[0] > minPlayers){
		if(playersParam[1] < playerOptions.Length){
				playersParam[1]--;
				playersParam[0] = playerOptions[playersParam[1]];
			}
		}	
		return playersParam;
	}//MorePlayers

	// Function for remove player on the players selector 
	public int[] LessPlayers(int[] playersParam){
		if(playersParam[0] < maxPlayers){
			if(playersParam[1] < playerOptions.Length){
				playersParam[1]++;
				playersParam[0] = playerOptions[playersParam[1]];
			}
			
		}
		return playersParam;
	}//LessPlayers
	
}

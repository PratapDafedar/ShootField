using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;

public class MPlayerData : MonoBehaviour {
	// Global parameters
	public bool useBlacklist;
	public bool useFriendlist;
	public MGameParameter parameters;	
	
	// Profil parameters
	public int id;
	public int gameId;
	public bool isLogin=false;
	public string userName;	
	public string playerName;
	public string nameInGame;
	public string mail;	
	public string privateIP;
	public string publicIP;	
	public bool isInGame;
	public string loginKey;	
	public bool isLan=false;
	public bool isOnline=false;

	// Excusion games list
	public List<MExcusionGame> exclusions; 
	
	// USE IF ONLY IF YOU HAVE MULTIONLINE : 
	// FriendList
	/*public List<MOFriend> friendList;	
		
	// BlackList
	public List<MOBlacklist> blackList; */
	
	// Update settings
	// Game calls
	public DateTime gameCallStartTime;	
	public DateTime gameCallNextTime;
	
	
	// Player calls
	private DateTime playerCallStartTime;	
	private DateTime playerCallNextTime;
	

	public void Awake(){
		DontDestroyOnLoad(this);	
		this.name = "MPlayerData";
		// USE IF ONLY IF YOU HAVE MULTIONLINE :
		/*friendList = new List<MOFriend>();
		blackList = new List<MOBlacklist>();*/
	} 
	
	public void Start(){
        parameters = GameObject.Find("LevelOption").GetComponent<MGameParameter>(); // Get the menu script
		// Instantiate Exclusion list
		exclusions = new List<MExcusionGame>();
		
		// Instantiate calls timers
		// Game timer
		gameCallStartTime = DateTime.Now;
		gameCallNextTime = gameCallNextTime.AddSeconds(parameters.gameCallInterval);
			
		// Player timer
		playerCallStartTime = DateTime.Now;
		playerCallNextTime = playerCallNextTime.AddSeconds(parameters.playerCallInterval);
	}
	
	public void Update(){
		// If we are the host, and the game is online
		if(isOnline){
			DateTime currentTime = DateTime.Now;
			int compare = DateTime.Compare(currentTime, playerCallNextTime);
			if(compare >=0) {
				PlayerCall();
				playerCallStartTime = DateTime.Now;
				playerCallNextTime = playerCallStartTime.AddSeconds(parameters.playerCallInterval);			
			}	
		}
	}

	// OnApplicationQuit : call when player exit the game and logout the player
	void OnApplicationQuit() {
		// Logout the player on the web Server
		// And make he quit the games where he is registred
		/*if(isOnline){
		 	MOServer server = new MOServer();	
			StartCoroutine(server.ExitGame(this.GetComponent<MPlayerData>(), true));
		}*/
	}
	
	// Logout: put the player as logout in the web server and in the current session 
	public void Logout(){
		/*if(isOnline){			
			MOServer server = new MOServer();	
			StartCoroutine(server.LogOut(this.GetComponent<MPlayerData>()));	
			isLogin = false;
			isOnline = false;
			loginKey = "";			
		}*/
	}//Logout
       	
	// ExitGame : call when the client want to exit game
    public void ExitGame(){
		isInGame = false;
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.ExitGame(this.GetComponent<MPlayerData>(), false));	
		}*/
	}
	
	// RefreshGameStatus : call when the game status change
	public void RefreshGameStatus(int gameId, string gameStatus){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.RefreshGameStatus(this.GetComponent<MPlayerData>(), gameId, gameStatus));	
		}*/
	}
	
	// RefreshGameMap : call when the game map change
	public void RefreshGameMap(int gameId, string gameMap){
		/*if(isOnline){			
			MOServer server = new MOServer();	
			StartCoroutine(server.RefreshGameMap(this.GetComponent<MPlayerData>(), gameId, gameMap));	
		}*/
	}
	
	// AddPlayerInGame : call when we have a new player in gane
	public void AddPlayerInGame(int gameId){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.AddPlayerInGame(this.GetComponent<MPlayerData>(), gameId));	
		}*/
	}
	
	// SaveRehostedGame : call after host migration, for save the rehosted game with his new paremeters
	public void SaveRehostedGame(int gameId, string gameName, int gamePort, string gameMap, int gameMaxPlayer, bool isGameUsePassword, bool isGameStarted, string gameRegister){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.SaveRehostedGame(this.GetComponent<MPlayerData>(), gameId, gameName, gamePort, gameMap, gameMaxPlayer, isGameUsePassword, isGameStarted, gameRegister));	
		}*/
	}
	
	// When a player disconnected from the server : remove it from database (call by the host)
	public void RemovePlayerOfGame(int playerId, int gameId){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.RemovePlayerOfGame(playerId, gameId, this.GetComponent<MPlayerData>()));	
		}*/
	}	
	
	
	// Add a new game on the exculsion list
	public void AddExculsion(string hostPublicIp, string hostPrivateIp, int port){
		exclusions.Add(new MExcusionGame(hostPublicIp, hostPrivateIp, port));
	}//AddExculsion	
	
	public void AddInBlacklist(int blacklistId){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.AddPlayerInBlacklist(blacklistId, this.GetComponent<MPlayerData>()));	
			blackList.Add (new MOBlacklist(blacklistId));
		}*/
	}//AddInBlacklist
	
	public void AddInFriendlist(int friendId){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.AddPlayerInFriendlist(friendId, this.GetComponent<MPlayerData>()));	
			friendList.Add (new MOFriend(friendId));
		}*/
	}//AddInFriendlist
	
	// PlayerCall : the player send a message on web server to show that he is active
	// Call in Update() function
	private void PlayerCall(){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.PlayerCall(this.GetComponent<MPlayerData>()));	
		}*/
	}//PlayerCall
	
	// HostCall : the host send a message on webserver to show that he is active
	// Call from MNetwork Update() function
	public void HostCall(int gameId){
		/*if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.HostCall(this.GetComponent<MPlayerData>(), gameId));	
		}*/
	}//HostCall	
}//MPlayerData
 
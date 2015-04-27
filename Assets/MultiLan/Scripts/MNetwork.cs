using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;

public class MNetwork : MonoBehaviour {
	// Global parameters
	public string mainMenuName = "Menu";
	public string waitRoomName = "Room";
	public string networkMessage = "";
	
	// SCRIPTS AND OBJECTS 
	public MMenu menuSrc;
	public MPlayerData playerDataSrc;
	public MGameParameter parameters;	

	// Game parameters
	public MGame gameInfo;	
	public bool isGameServerRebuild = false;
	public bool isGameServerWaitingPlayers = false;
	public bool isGameServerRebuildSuccess = false;
	public bool isGameServerRebuildFailed = false;
	public bool isGameHost;
	
	// Player parameters	
	public bool isSearchGame = false;
	public bool isPlayerExitGame=false;
	public Vector3 playerPrefabPosition;
	public Quaternion playerPrefabRotation;	
	private GameObject playerPrefab;
	
	// Chat (waitroom and gamechat)
	public List<String> chatContent;	
	
	// Playerlist
	public List<MUser> playerList;
	public List<MPlayersPosition> positionList;
	private List<MUser> playerMigrationList;
	
	// Player exclusion
	public bool isExculdePlayer=false;
	public bool isPlayerExculde=false;
	public MUser excludedPlayer;
	public NetworkPlayer excludedNetworkPlayer;
	public bool alwaysExcudePlayer=false;
	
	// FrienList
	public bool isFriendPlayer=false;
	public bool isFriendAsk=false;
	public bool isFriendAskYes=false;
	public bool isFriendAskNo=false;
	public MUser friendPlayer;
	public MUser friendAskPlayer;
	
	//Dedicated
	public bool isDedicatedServerBreakDown=false;
	
	//Rebuild
	public int rebuildServerTimer;
		
	//------------------- BASIC FUNCTIONS  ------------------ //	
	void Awake(){
		DontDestroyOnLoad(this);
		this.name = "MNetwork";		
		menuSrc = GameObject.Find (mainMenuName).GetComponent<MMenu>(); // Get the menu script
        parameters = GameObject.Find("LevelOption").GetComponent<MGameParameter>(); // Get the menu script
		playerDataSrc = GameObject.Find ("MPlayerData").GetComponent<MPlayerData>(); // Get the playerData script
		playerList = new List<MUser>(); // Instantiate the playerList
		positionList = new List<MPlayersPosition>();
		playerMigrationList = new List<MUser>(); // Instantiate the playerMigrationList
		gameInfo = new MGame();
		Network.sendRate = parameters.sendRate;
	}//Awake
	
	void Update(){
		if(playerDataSrc.isInGame && !isGameServerRebuild && parameters.displayPlayerName){
			GetComponent<NetworkView>().RPC("SendPlayerPosition", RPCMode.Others, playerDataSrc.gameId,playerDataSrc.nameInGame, playerPrefabPosition);
		}

		// If we are the host, and the game is online
		if(isGameHost && gameInfo.isOnline){
			DateTime currentTime = DateTime.Now;
			int compare = DateTime.Compare(currentTime, playerDataSrc.gameCallNextTime);
			if(compare >=0) {
				playerDataSrc.HostCall(gameInfo.id);
				playerDataSrc.gameCallStartTime = DateTime.Now;
				playerDataSrc.gameCallNextTime = playerDataSrc.gameCallStartTime.AddSeconds(parameters.gameCallInterval);			
			}	
		}
	}//Update

	//---BASIC FUNCTIONS
	//------------------------------------------------------
	//------------------- EVENTS FUNCTIONS  ------------------ //	
	void OnServerInitialized(){
		// Save host parameters
		isGameHost = true;								
		// Server save his id around the game
		playerDataSrc.gameId = int.Parse(Network.player.ToString()); 
		this.gameInfo.totalPlayer = 1;
		
		// Add server to playerList
		int inGame = 0;
		if(playerDataSrc.isInGame){
			inGame = 1;
		}			
		
		if(!isGameServerRebuild){
			playerList.Clear(); // Clear the playerList 
			// Save the host on the playerList
			playerList.Add(new MUser(playerDataSrc.id, playerDataSrc.gameId,  
			playerDataSrc.privateIP,playerDataSrc.publicIP,
			playerDataSrc.nameInGame,0, inGame, 1)); 			
			StartGame(false); // Try to start the game
		} else {
			playerMigrationList.Clear();
			playerMigrationList.Add(new MUser(playerDataSrc.id, playerDataSrc.gameId,  
			playerDataSrc.privateIP,playerDataSrc.publicIP,
			playerDataSrc.nameInGame,0, inGame, 1)); 
		}	
	}//OnServerInitialized	
	
	// OnFailedToConnect : call on the client
	void OnFailedToConnect(NetworkConnectionError error){		
		if(!isSearchGame) {			
			if(isGameServerRebuild){
				return;					
			} else {
				string errorMsg="";				
				if(error == NetworkConnectionError.TooManyConnectedPlayers){
					 errorMsg = menuSrc.text.nmErrorMaxPlayers;
				} else if(error == NetworkConnectionError.InvalidPassword){
					 errorMsg = menuSrc.text.nmErrorPassword;
				} else if(error == NetworkConnectionError.AlreadyConnectedToServer){
					 errorMsg = menuSrc.text.nmErrorAlreadyConnect;
				} else if(error == NetworkConnectionError.AlreadyConnectedToAnotherServer){
					 errorMsg = menuSrc.text.nmErrorToAnotherServer;
				} 					
				menuSrc.networkJoinMessage[0] = gameInfo.isOnline.ToString();
				menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorConnexion+errorMsg;	
				Destroy(gameObject);
			}
		} else if(isSearchGame && menuSrc.useLan){ // If we are searching games 
			// This IP has no game, so :		
			menuSrc.LanGetNetworkGames(false); // Go to next IP	
		}
		
	}//OnFailedToConnect
	
	// OnPlayerConnected : call on the server
	void OnPlayerConnected(NetworkPlayer player){
		GetComponent<NetworkView>().RPC ("DisabledPlayers", player);	
	}//OnPlayerConnected	
	
	// OnPlayerDisconnected : call on the server
	void OnPlayerDisconnected(NetworkPlayer player) {	
		if(MUser.inList(playerList, int.Parse(player.ToString()))){
			gameInfo.totalPlayer--;
			GetComponent<NetworkView>().RPC("RefreshPlayerCount", RPCMode.All, gameInfo.totalPlayer);		
			if(gameInfo.isOnline){
				playerDataSrc.RemovePlayerOfGame(MUser.SearchPlayer(int.Parse(player.ToString()), playerList).id, gameInfo.id);
			}			
			playerList = MUser.RemoveFromId(playerList, int.Parse(player.ToString()));
			playerList = MUser.PingSort(playerList);	// Sort the list by Ping
			GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
			positionList = MPlayersPosition.RemoveFromId(positionList, int.Parse(player.ToString()));
			GetComponent<NetworkView>().RPC("RemoveInPositionList", RPCMode.Others, int.Parse(player.ToString()));		

		}
		Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }//OnPlayerDisconnected
	
	// OnConnectedToServer : call on the client
	void OnConnectedToServer(){		
		if(!isSearchGame) {	 // If we just searching games 
			// Client save his id around the game
			playerDataSrc.gameId = int.Parse(Network.player.ToString()); 
			if(!isGameServerRebuild){
				GetComponent<NetworkView>().RPC ("AddPlayer", RPCMode.Server, 
					playerDataSrc.id, 
					playerDataSrc.gameId, 
					playerDataSrc.nameInGame, 
					playerDataSrc.privateIP, 
					playerDataSrc.publicIP,
					gameInfo.isOnNetwork,
					playerDataSrc.isInGame,
					playerDataSrc.isOnline, Network.player);
			} else {			
				GetComponent<NetworkView>().RPC ("AddMigrationPlayer", RPCMode.Server, 
					playerDataSrc.id, 
					playerDataSrc.gameId, 
					playerDataSrc.nameInGame, 
					playerDataSrc.privateIP, 
					playerDataSrc.publicIP,
					gameInfo.isOnNetwork,
					playerDataSrc.isInGame,
					playerDataSrc.isOnline, Network.player);
			}
		
		} else {
			// Else, if the client is just searching a game : call SearchGame on the server
			GetComponent<NetworkView>().RPC ("SearchGameInfo", RPCMode.Server, Network.player);
		}
	} //OnConnectedToServer
	
	
	// OnDisconnectedFromServer : call on the client
	void OnDisconnectedFromServer(){
		// If we are not just searching a game or if we have not been exculded 
		if(!isSearchGame && !isPlayerExculde && Network.peerType != NetworkPeerType.Server) {
			// All the game will be rebuilt with a new host (if we find one)			
			gameInfo.totalPlayer = 0;// Put the total player on 0	
			CleanGame(); // Clean the game (remove all players prefab) (they will be re-loaded on the new host)				
			playerList = MUser.RemoveServer(playerList);// Remove server from playerList (because he have leave the game)
			if(gameInfo.isOnDedicatedServer && !isPlayerExitGame){
				isDedicatedServerBreakDown = true;
			} else {
				if(!isPlayerExitGame){ // If the player exit the game
					StartRebuildServer();	// Start the host migration
				}
			}
		}
	}//OnDisconnectedFromServer
	
	// OnDestroy : call when we destroy the current gameObject
	void OnDestroy(){			
		CleanGame(); // Clean the game (remove all players prefab) (they will be re-loaded on the new host)	
		DestroyGameMenu();
	}//OnDestroy
	
	// OnLevelWasLoaded 
	void OnLevelWasLoaded (){
		// save the playerPrefab in local variable
		if(Application.loadedLevelName != waitRoomName) {
			if(!gameInfo.isStarted){
				chatContent.Clear(); // Clear the chat (because it contains the waitroom Thread and timer)
			}
			EnabledPlayer(); // Enabled the render of the other players
            if (GameObject.Find("Spawns") != null)
            {
                MSpawn spawnSrc = GameObject.Find("Spawns").GetComponent<MSpawn>();
                playerPrefab = spawnSrc.playerPrefab;
            }

			gameInfo.isStarted = true;
			playerDataSrc.isInGame = true;						
			
			// Save the server new statut (in game)
			if(isGameHost) {
				playerList = MUser.SavePlayerStatus(playerDataSrc.gameId, 1, playerList);
				// Send my new status to the clients
				GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));						
							
				if(gameInfo.isOnline){
					playerDataSrc.RefreshGameStatus(gameInfo.id, "1");	
				}
			} else {
				// Send to the server that I'm now in game
				GetComponent<NetworkView>().RPC ("RefreshPlayerStatus", RPCMode.Server, playerDataSrc.gameId, 1);
			}	
		}
	}//OnLevelWasLoaded		
	
	//---EVENTS FUNCTIONS
	//------------------------------------------------------
	//------------------- PUBLIC FUNCTIONS  ------------------ //	
	public void StartServer(int id, string name, int maxPlayer, int port, bool usePass, string pass, string register, string registerDate, bool isPrivate, bool isOnline, bool isOnNetwork, bool isOnDedicatedServer){
		if(usePass){
			Network.incomingPassword = pass;
		}
		NetworkConnectionError error = Network.InitializeServer(maxPlayer, port, false);		
		if(error != NetworkConnectionError.NoError){
			menuSrc.networkCreateMessage[0] = isOnline.ToString();
			menuSrc.networkCreateMessage[1] = menuSrc.text.nmErrorGameCreation;
			if(error == NetworkConnectionError.CreateSocketOrThreadFailure){
				menuSrc.networkCreateMessage[1]+= menuSrc.text.nmErrorUsedPort;	
			} else if(error == NetworkConnectionError.InvalidPassword){
				menuSrc.networkCreateMessage[1]+= menuSrc.text.nmErrorPassword;	
			} else if(error == NetworkConnectionError.AlreadyConnectedToAnotherServer){
				menuSrc.networkCreateMessage[1]+= menuSrc.text.nmErrorToAnotherServer;	
			}		
			if(isOnline) { // If it's an online game : destroy the game from database
				playerDataSrc.ExitGame();
			}
		} else {		
			this.gameInfo = new MGame(id, name, port, parameters.maps[0], 0, 1, maxPlayer, usePass,
			false, register, registerDate, isOnline, isPrivate, isOnNetwork, isOnDedicatedServer,
			int.Parse(Network.player.ToString()), playerDataSrc.nameInGame, playerDataSrc.privateIP, playerDataSrc.publicIP, null);
		}
	}//StartServer
	
	
	public void JoinServer(string ip, int port, bool isGameUsePassword, string password, bool isOnNetwork, bool isOnlineGame){
		if(ip != "" && !port.Equals(null)){
			gameInfo.isOnNetwork=isOnNetwork;
			if(isOnlineGame){
				gameInfo.isOnline=true;
			}
			if(isGameUsePassword){
				Network.Connect(ip, port, password);
			} else {
				Network.Connect(ip, port);
			}			
		}
	}//Join Server
	
	public void SearchGame(int port, string ip){
		isSearchGame=true;
		Network.Connect(ip, port);		 	
	}//SearchGame
	
	public void StartGame(bool canLoadGame){
		if(isGameServerRebuildSuccess){ // If server has been rebuilt	
			if(gameInfo.isStarted) {
				LoadPlayer(); // Load the player on map	
			}
			isGameServerRebuildSuccess=false;
		} else if(canLoadGame) { // else if the game is ready to be load	
			if(Application.loadedLevelName != gameInfo.mapName){ // If the map is not already load
				Application.LoadLevel(gameInfo.mapName);					
			}					
		} else if(!playerDataSrc.isInGame){ // Else, load the waitroom
			if(Application.loadedLevelName != waitRoomName && waitRoomName != "" && waitRoomName != null){ // If the map is not already load
				Application.LoadLevel("Room");	
			}
		}
	}//StartGame	
	
	public void PingPlayers(){
		if(playerList.Count > 1){ // If the list is not emtpy :			
			for(int i = 0; i < Network.connections.Length; i++){
				for(int j=0; j < playerList.Count; j++){
					if(Network.connections[i].ToString().Equals(playerList[j].gameId.ToString())){
						playerList[j].playerPing = Network.GetAveragePing(Network.connections[i]);
					} 
				}
			}	
		}
		
	}//PingPlayers
		
	public void ExcludePlayer(MUser player){
		isExculdePlayer = true;
		excludedPlayer = player; // Save the player to exclude
		// Search the corresponding networkPlayer and save it
		excludedNetworkPlayer =	SearchNetworkPlayer(player.gameId);				
	}//ExcludePlayer
	
	public void CancelExcludePlayer(){
		isExculdePlayer = false;
		excludedPlayer = new MUser();
		excludedNetworkPlayer = new NetworkPlayer();
	}//CancelExcludePlayer
	
	public void ConfirmExcludePlayer(){
		GetComponent<NetworkView>().RPC("Exculde", excludedNetworkPlayer);
		if(gameInfo.isOnline && alwaysExcudePlayer){
			playerDataSrc.AddInBlacklist(excludedPlayer.id);
		}
		isExculdePlayer = false;
		excludedPlayer = new MUser();
		excludedNetworkPlayer = new NetworkPlayer();
	}//ConfirmExcludePlayer
	
	
	// USE IF ONLY IF YOU HAVE MULTIONLINE : 
	public void AskFriend(MUser player){
		/*isFriendPlayer = true;
		friendPlayer = player; // Save the player to add in friendlist	
		*/
	}//AskFriend	
	
	public void SendFriendAsk(){
		/*isFriendPlayer = false;
		if(Network.isServer){
			networkView.RPC ("GetFriendAsk", SearchNetworkPlayer(friendPlayer.gameId), MUser.SearchPlayerId(playerDataSrc.id, playerList).UserToString());
		} else {
			networkView.RPC("SendQueryToFriendList", RPCMode.Server, friendPlayer.UserToString(), MUser.SearchPlayerId(playerDataSrc.id, playerList).UserToString());		
		}*/
	}//SendFriendAsk
	

	public void FriendAskAnswer(bool isOk){	
		/*if(isOk){
			playerDataSrc.friendList.Add(new MOFriend(friendAskPlayer.id));
		}
		if(Network.isServer){
			networkView.RPC ("GetFriendAnswer", SearchNetworkPlayer(friendAskPlayer.gameId), isOk);
		} else{
			networkView.RPC("SendAnswerToFriendList", RPCMode.Server, friendAskPlayer.UserToString(), MUser.SearchPlayerId(playerDataSrc.id, playerList).UserToString(), isOk);		

		}
		friendAskPlayer = null;
		isFriendAsk = false;
		*/
	}//FriendAskAnswer
	
	public void SaveFriend(bool isOk){		
		/*if(isOk){
			isFriendAskYes = true;
			isFriendAskNo = false;
			playerDataSrc.AddInFriendlist(friendPlayer.id);
		} else {
			isFriendAskNo = true;
			isFriendAskYes = false;
		}
		*/	
	}//FriendAskAnswer	

	//---
	
	public NetworkPlayer SearchNetworkPlayer(int gameId){
		for(int i = 0; i < Network.connections.Length; i++){
			if(Network.connections[i].ToString().Equals(gameId.ToString())){
				return Network.connections[i];
			} 				
		}
		return new NetworkPlayer();
	}//SearchNetworkPlayer
		
	// Chat function : call from the waitRoom or from the gameChat form sycronise chat
	public void SendChatMessage(string message){
		GetComponent<NetworkView>().RPC ("ChatMessage", RPCMode.All, message);	
	}//SendChatMessage

		
	// Destroy all player GameObject (because they will be reloaded)
	public void CleanGame(){	
		positionList.Clear();
		GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i < gameArray.Length; i++) {
			if(gameArray[i].name == "Player") {
				GameObject.Destroy(gameArray[i]);
			}
		}
	}//CleanGame	
	
	public void DestroyGameMenu(){
		try{
			Destroy(GameObject.Find ("MGameMenu"));
		} catch{}		
	}
			
	// When my player spwaned 
	public void PlayerSpawned(){	
		// Send DisabledPlayers to all other players, so that players in waitroom can disabled my player
		GetComponent<NetworkView>().RPC ("DisabledPlayers", RPCMode.Others);	
		gameInfo.isStarted=true;
		playerDataSrc.isInGame=true;
	}//PlayerSpawned
	
	public void ExitGame(bool onlineLogout){		
		Network.Disconnect();  // Disconnect from the network
		playerDataSrc.isInGame = false; // Put isInGame at false
		Destroy(gameObject); // Desrtoy the NetworkManager
		Application.LoadLevel(mainMenuName); // Load the main menu	
		
		if(onlineLogout){
			// If the game is online 
			if(gameInfo.isOnline){
				// Exit the game on the dataBase
				playerDataSrc.ExitGame();
			}	
		}	
	}//ExitGame	
	
	
	//----REBUILD SERVER FUNCTIONS ----
	public void StartRebuildServer(){
		// Set the rebuild boolean
		isGameServerRebuild = true;	
		isGameServerWaitingPlayers = false;
		isGameServerRebuildFailed = false;
		isGameServerRebuildSuccess=false;
		
		StartCoroutine(RebuildServer()); // Try to rebuild a new server
		StartCoroutine(RebuildServerTimer()); // Start the rebuild timer
	}//StartRebuildServer
	
	private IEnumerator RebuildServer() {
		DateTime startTime = DateTime.Now;
		DateTime maxRebuildTime = startTime.AddSeconds(parameters.rebuildTime);			
		
		// Wait 2 seconds for disconnection be effective
		yield return new WaitForSeconds(2);			
		do {	
			DateTime currentTime = DateTime.Now;
			int compare = DateTime.Compare(currentTime, maxRebuildTime);
			if(compare >=0) {
				FailedRebuildServer();
				break;
			}					
			for(int i = 0; i < playerList.Count; i++) {// Loop the playerList		
				yield return new WaitForSeconds(0);	
				
				if (Network.peerType == NetworkPeerType.Disconnected){ // If I am not yet connect					
					// If the player can host and if the game is not started or if he is on the game
					if(!gameInfo.isStarted || playerList[i].isPlayerInGame == 1){
						if(playerList[i].gameId == playerDataSrc.gameId){ // If the current id is mine	
							
							// I create the server	
							// (with password if the game had a password)
							if(gameInfo.isUsePassword){
								Network.incomingPassword = gameInfo.password;
							}
							Network.InitializeServer(gameInfo.maxPlayer, gameInfo.port, false); 	
							isGameServerWaitingPlayers = true;
							
							// Wait 5 seconds for players connects on me
							yield return new WaitForSeconds(parameters.rebuildWaitPlayers);	
							if(Network.connections.Length > 0 || (Network.connections.Length == 0 && playerList.Count==1)){
								// If some players are connected on me : call SuccessRebuildServer
								SuccessRebuildServer ();						
							} else {
								// Else close my server and continue the loop
								Network.Disconnect();
							}
							
						} else { 
							string connectIp = "";
							if(playerList[i].publicIp != playerDataSrc.publicIP) { // If the server hasn't the same public ip as me :
								connectIp = playerList[i].publicIp; // Use his public IP
							} else { 
								connectIp = playerList[i].privateIp; // Use his private IP
							}
							
							// I connect myself to the server by his IP
							// (with password if the game had a password)
							if(gameInfo.isUsePassword){
								Network.Connect(connectIp, gameInfo.port, gameInfo.password);
							} else {
								Network.Connect(connectIp, gameInfo.port); 
							}			
							// Wait 2 seconds for connection be effective*/
							yield return new WaitForSeconds(2);							
						}						
						
					} 
				}
			}	
		} while(Network.peerType == NetworkPeerType.Disconnected);
	}//RebuildServer	
		
	// Call if we cannot rebuild a server
	private void FailedRebuildServer(){
		isGameServerRebuild = false;
		isGameServerRebuildFailed = true;
	}//FailedRebuildServer	
	
	// Call if we have well rebuild the server
	private void SuccessRebuildServer(){
		isGameServerRebuildSuccess=true;							
		isGameServerWaitingPlayers=false; 
		isGameServerRebuild=false;
		OnServerInitialized();		
		SavePlayerMigrationList();	
		if(gameInfo.isOnline){ // If the game is online : save the new host in dataBase
			playerDataSrc.SaveRehostedGame(gameInfo.id, gameInfo.name, gameInfo.port, gameInfo.mapName, gameInfo.maxPlayer, gameInfo.isUsePassword, gameInfo.isStarted, gameInfo.registerDate);
		}	
	}//SuccessRebuildServer

	private IEnumerator RebuildServerTimer(){
		rebuildServerTimer=parameters.rebuildTime;
		int i = 1;
		while(rebuildServerTimer > i){
			yield return new WaitForSeconds(1);	
			rebuildServerTimer--;
		}
	}//RebuildServerTimer
	
	// Call after the rebuild to update the new data and send the new userlist & game info to connected player
	private void SavePlayerMigrationList(){		
		// Set up the total players
		gameInfo.totalPlayer = playerMigrationList.Count;		
		// Tell to the other players that the rebuild success
		GetComponent<NetworkView>().RPC("RebuildSuccess", RPCMode.Others);			
		playerList.Clear();
		
		// Update the playerList
		for(int i = 0; i < playerMigrationList.Count; i++){			
			playerList.Add(playerMigrationList[i]);
		}
		
		// Refresh the lists of the clients : send on everybody except the server
		GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));
		
		// Send the game informations to the new player
		GetComponent<NetworkView>().RPC("GetGameInfos", RPCMode.Others, gameInfo.GameToString(), true);

		// Send the game state and try to start game
		GetComponent<NetworkView>().RPC("RefreshPlayerCount", RPCMode.Others, gameInfo.totalPlayer);	
						
		if(gameInfo.isOnline){
			// Say to the player to save himself on the online list of game's players
			GetComponent<NetworkView>().RPC("AddPlayerInGame", RPCMode.Others);
		}
	}//SavePlayerMigrationList
	//----REBUILD SERVER FUNCTIONS ----			
	//---PUBLIC FUNCTIONS
	//------------------------------------------------------
	//------------------- PRIVATE FUNCTIONS  ------------------ //
			
	private void LoadPlayer(){
		// Load the player in his previous position and rotation	
		try{
			Network.Instantiate(playerPrefab, playerPrefabPosition, playerPrefabRotation, 0);
		} catch(NullReferenceException){
			return;
		}	}	

	
	//  DISABLED / ENABLED player 
	private void DisabledPlayer(){
		if(!GetComponent<NetworkView>().isMine) {			
			GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int i = 0; i < gameArray.Length; i++) {
				if(gameArray[i].name == "Player") {	
					// Make player invisible : disabled renderer on him and his childrens
					gameArray[i].GetComponent<Renderer>().enabled  = false; 
					gameArray[i].GetComponent<Rigidbody>().isKinematic = false; // Disable the physic on the playes
					Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
					foreach (Transform child in chidrens) {
						if(child.GetComponent<Renderer>() != null) {
							child.GetComponent<Renderer>().enabled = false;
						}
					}									
				}
			}
		}
	}//DisabledPlayer
	
	private void EnabledPlayer(){	
		if(!GetComponent<NetworkView>().isMine) {
			GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int i = 0; i < gameArray.Length; i++) {
				if(gameArray[i].name == "Player") {
					// Make player visible : enabled renderer on him and his childrens
					gameArray[i].GetComponent<Renderer>().enabled  = true; 	
					gameArray[i].GetComponent<Rigidbody>().isKinematic = true; // Disable the physic on the playes, since we don't want that they move alone
					Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
					foreach (Transform child in chidrens) {
						if(child.GetComponent<Renderer>() != null) {
							child.GetComponent<Renderer>().enabled = true;
						}
					}
					// Ask player position to the server
					GetComponent<NetworkView>().RPC ("AskPosition", RPCMode.Server, gameArray[i].GetComponent<NetworkView>().viewID, Network.player);
				}
			}
		}		
	}//EnabledPlayer
	//---PRIVATE FUNCTIONS  	
	//------------------------------------------------------
	//------------------- RPC FUNCTIONS  ------------------ //
	// Call by the client to the server to received game informations
	[RPC]
	void SearchGameInfo(NetworkPlayer player){
		// If it's not an online game
		if(!gameInfo.isOnline){
			// Go search the games informations
			GetComponent<NetworkView>().RPC("GetGameInfos", player, gameInfo.GameToString(), false);		
		} else { // Else			
			// Call GoNextGame to the client
			GetComponent<NetworkView>().RPC("GoNextGame", player);
		}
	}//SearchGameInfo
	
	// Call by the player, to the server, to be add on the playerList	
	[RPC]  
	void AddPlayer(int id, int gameId, string playerName, string privateIp, string publicIp, bool isOnNetwork, bool isPlayerInGame, bool isPlayerOnline, NetworkPlayer player){	
		// If the game is private and that we are not on the network of the host
		// (check by comparing public ip)
		if(gameInfo.isPrivate && publicIp != playerDataSrc.publicIP && !isOnNetwork) {			
			GetComponent<NetworkView>().RPC ("CantJoinGame", player, 2); // We cannont join the game
			return; // Exit the function here
		}
		
		// If the game is Online but the player is not connected
		if(gameInfo.isOnline && !isPlayerOnline){			
			GetComponent<NetworkView>().RPC ("CantJoinGame", player, 3); // We cannont join the game
			return; // Exit the function here
		}
		// If the game is not yet started or if we don't use the wait room
		//or if the player is already on game = if the server has been rebuild
		if(!gameInfo.isStarted || isPlayerInGame || parameters.canJoinStartedGame) {
			gameInfo.totalPlayer++;	// Increments player count				
			int inGame = 0;
			if(isPlayerInGame){
				inGame = 1;
			}		
			// Add the new player on the playerList
			playerList.Add (new MUser(id, gameId, privateIp, publicIp, playerName, Network.GetAveragePing(player), inGame, 0));
			// Sort the list by Ping
			playerList = MUser.PingSort(playerList);	
			
			// Refresh the lists of the clients : send on everybody except the server
			GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));			
			
			// Send the game informations to the new player
			GetComponent<NetworkView>().RPC("GetGameInfos", player, gameInfo.GameToString(), true);

			// Send the game state and try to start game
			GetComponent<NetworkView>().RPC("RefreshPlayerCount", RPCMode.Others, gameInfo.totalPlayer);	
						
			if(gameInfo.isOnline || gameInfo.isOnDedicatedServer){
				// Say to the player to save himself on the online list of game's players
				GetComponent<NetworkView>().RPC("AddPlayerInGame", player);
			}
		} else {
			// Else, we cannot join the game : 
			GetComponent<NetworkView>().RPC ("CantJoinGame", player, 1);
		}
	}//AddPlayer	
	
	// Call by the player, to the server, to be add a player during host migration 
	[RPC]
	void AddMigrationPlayer (int id, int gameId, string playerName, string privateIp, string publicIp, bool isOnNetwork, bool isPlayerInGame, bool isPlayerOnline, NetworkPlayer player){	
		int inGame = 0;
		if(isPlayerInGame){
			inGame = 1;
		}		
		// Add the new player on the playerList
		playerMigrationList.Add (new MUser(id, gameId, privateIp, publicIp, playerName, Network.GetAveragePing(player), inGame, 0));
		
	}//AddMigrationPlayer
	
	// Call by the server on the client who asked it
	[RPC]
	void GetGameInfos(string gameInfo, bool startGame){	
		this.gameInfo = new MGame(gameInfo);			
		// If we are allowed to start the game
		if(startGame){		
			// If the game is not yet started 	
			if(!this.gameInfo.isStarted || parameters.canJoinStartedGame || isGameServerRebuildSuccess) {
				StartGame(false); // Try to start game
			}	
		}		
		// If we are searching parties
		if(isSearchGame && menuSrc.useLan){		
			// We have now every informations we need, so :
			// This IP has no game, so :
			// This IP has no game, so :	
			menuSrc.LanGetNetworkGames(false); // Go to next IP			
		}			
	}//GetGameInfos
	
	// Call by the server to all clients when the list has changed
	[RPC]
	void RefreshUserList(string list){
		playerList = MUser.ListToObject(list);// Save the new list		
	}//RefreshUserList
	
	// Call by the server to all clients when the list has changed
	[RPC]
	void RemoveInPositionList(int id){
		positionList = MPlayersPosition.RemoveFromId(positionList, id);// Save the new list		
	}//RefreshPositionList
	
	// Call by ther server to all clients when a player quit the game
	[RPC]
	void RefreshPlayerCount(int totalPlayer){
		gameInfo.totalPlayer = totalPlayer;
	}//RefreshPlayerCount
	
	
	//By by the server on the client when a fall on an Online game when he searchs network games
	[RPC]
	void GoNextGame(){
		if(menuSrc.useLan){
			menuSrc.LanGetNetworkGames(false); // Go to next IP		
		}
	}//GoNextGame
	
	// Call by the server to a client when he join the game :
	// so that the new client save himself on the game's player list on DataBase
	[RPC]
	void AddPlayerInGame(){
		if(gameInfo.isOnline || gameInfo.isOnDedicatedServer){
			playerDataSrc.AddPlayerInGame(gameInfo.id);
		}
	}//AddPlayerInGame
	
	// Call by the server to a player which have been exclude
	[RPC]
	void Exculde(){
		isPlayerExculde = true;
		Network.Disconnect(); // Disconnect of the network
		CleanGame(); // Clean the game (remove all players objects)		
		playerDataSrc.AddExculsion(gameInfo.hostPublicIp, gameInfo.hostPrivateIp, gameInfo.port);
	}	
	
	// Call by the server to the client, when the connexion is started
	[RPC] 
	void DisabledPlayers(){			
		// If I'm not in game : disabled the rendrer of the other players
		// (Else I can see them before have load the game level)
		if(!playerDataSrc.isInGame){			
			DisabledPlayer();		
		}
	}//DisabledPlayers
	
	// Send to the client, when he cannot join the game
	[RPC]
	void CantJoinGame(int index){
		CleanGame(); // Delete all players on the client scene
	
		// Define a message
		menuSrc.networkJoinMessage[0] = gameInfo.isOnline.ToString();
		if(index == 1) {			
			menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorStartedGame;	
		} else if(index == 2) {
			menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorPrivateGame;
		} else if(index == 3) {
			menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorOnlineGame;
		}
		Destroy(gameObject); // Destroy the client network GameObject
	}//CantJoinGame
	
	// Call on ererybody : send message from chat on waitRoom or gameChat
	[RPC]
	void ChatMessage(string message){
		// Add the message on the messages list
		chatContent.Add (message);			
				
		// Make the chat scroll down
		try {
			// Search if the waiting room is open
			MWaitRoom waitRoomScr = GameObject.Find ("WaitRoom").GetComponent<MWaitRoom>();
			if(waitRoomScr != null) {
				// Update the waiting room chat scroll
				waitRoomScr.chatScroll = new Vector2(0, (20*(chatContent.Count+1)) - waitRoomScr.chatBox.height);
			}
		} catch (NullReferenceException) {
			try {
				// Search if the game chat is open
				MGameChat gameChatSrc = GameObject.Find ("MGameMenu").GetComponent<MGameChat>();
				if(gameChatSrc != null){
					// Update the game chat scroll
					gameChatSrc.chatScroll = new Vector2(0, (20*(chatContent.Count+1)) - gameChatSrc.chatContentSizeY);
				}
			} catch (NullReferenceException) {
				return;
			}
		}
	}//ChatMessage
	
	// Call by the server to the other players when the server is well rebuid
	[RPC]
	void RebuildSuccess(){
		// Put all the rebuild parameter on the good value
		isGameServerRebuildSuccess=true;
		isGameServerRebuild=false;
		isGameServerWaitingPlayers=false;
		isGameServerRebuildFailed=false;
	}//RebuildSuccess
	
	// Call by a client to ther server, when a load the game
	[RPC]
	void RefreshPlayerStatus(int playerId, int isPlayerInGame){
		// Save the new status of the player
		playerList = MUser.SavePlayerStatus(playerId, isPlayerInGame, playerList);
		// Call RefreshUserList on the other players
		GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
	}//RefreshPlayerStatus
	
	// Call by the client (when he load the map) to the server to receive the players positions
	[RPC]
	void AskPosition(NetworkViewID viewId, NetworkPlayer askPlayer){
		NetworkView view = NetworkView.Find(viewId); // Search the view form M
		GameObject gamePlayer = view.observed.gameObject; // Get the gameObject
		// Send the viewid and the position to the askPlayer
		GetComponent<NetworkView>().RPC ("SendPosition", askPlayer, viewId, gamePlayer.transform.position);
	}//AskPosition	
		
	// Call by the server to the client, send the players positions to the player who asked it
	[RPC]
	void SendPosition(NetworkViewID viewId, Vector3 position){
		NetworkView view = NetworkView.Find(viewId); // Search the view form M
		GameObject gamePlayer = view.observed.gameObject; // Get the gameObject
		// Move the gameObject on his good position
		gamePlayer.transform.position = position;
	}//SendPosition	
	
	// Call by each players to all other to keep update the position list
	[RPC]
	void SendPlayerPosition(int id, string name, Vector3 position){
		positionList=MPlayersPosition.UpdatePosition(positionList, id, name, position);
	}//SendPlayerPosition
	
	// FRIENDLIST
	[RPC]
	public void SendQueryToFriendList(string friend, string guest){
		NetworkPlayer networkPlayer = SearchNetworkPlayer(MUser.UserToObject(friend).gameId);
		if(Network.isServer && networkPlayer.ToString().Equals(playerDataSrc.gameId.ToString())){
			isFriendAsk = true;
			friendAskPlayer = MUser.UserToObject(guest);
		} else {
			GetComponent<NetworkView>().RPC ("GetFriendAsk", networkPlayer, guest);
		}
	}//SendQueryToFriendList
	
	[RPC]
	public void SendAnswerToFriendList(string friend, string guest, bool answer){
		NetworkPlayer networkPlayer = SearchNetworkPlayer(MUser.UserToObject(friend).gameId);
		if(Network.isServer && networkPlayer.ToString().Equals(playerDataSrc.gameId.ToString())){
			SaveFriend(answer);
		} else {
			GetComponent<NetworkView>().RPC ("GetFriendAnswer", networkPlayer, answer);
		}
	}//SendAnswerToFriendList
	
	[RPC]
	public void GetFriendAsk(string guest){
		isFriendAsk = true;
		friendAskPlayer = MUser.UserToObject(guest);
	}//GetFriendAsk
	
	[RPC]
	public void GetFriendAnswer(bool isOk){
		SaveFriend(isOk);
	}//GetFriendAnswer
	
	
	//---RPC FUNCTIONS
	
}//MNetwork


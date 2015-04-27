using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;

public class MWaitRoom : MonoBehaviour {
	//------ ATTACHED SCRIPTS
	public MGameParameter parameters;	
	public MText text;	
	public MPlayerData playerDataSrc;	
	public MNetwork networkSrc;	

	
	//------ TIMER PARAMETRES 
	public bool useLoadTimer = true;
	public bool useLoadChatTimer = true;
	public int loadTimerTime = 5;
	public bool isLoading=false;
	private string loadGameCount;
	
	//----POSITIONS SETTINGS
	public int playerListPosX;
	public int playerListPosY;
	public int playerListSizeX;
	public int playerListSizeY;
	public Rect playerListRec;
	public int playerListBoxSizeY = 30;

	public int gamePanelPosX;
	public int gamePanelPosY;
	public int gamePanelSizeX;
	public int gamePanelSizeY;
	public Rect gamePanelRec;
	
	public int chatPosX;
	public int chatPosY;
	public int chatSizeX;
	public int chatSizeY;
	public int chatButtonSizeX = 100;
	public int chatButtonSizeY = 20;
	public Rect chatRec;
	public Rect chatBox;
	
	
	public int buttonsPanelPosX;
	public int buttonsPanelPosY;
	public int buttonsPanelSizeX;
	public int buttonsPanelSizeY;
	public Rect buttonsPanelRec;
	
	public int buttonExculdeSizeX = 25;
	public int buttonExculdeSizeY = 20;
	
	//--- FORMS
	public string formChatTxt;
	
	//---- SCROLLS 
	public Vector2 mapScroll = Vector2.zero;
	public Vector2 playerScroll = Vector2.zero;	
	public Vector2 chatScroll = Vector2.zero;	
	
	void Awake(){; 	
		this.name="WaitRoom";		
	}//Awake
	
	void Start(){
		Instantiate(Resources.Load("MGameMenu"));
		text = GameObject.Find ("MText").GetComponent<MText>(); // Get the menu script
		parameters = GameObject.Find ("MGameParameter").GetComponent<MGameParameter>(); // Get the menu script
		networkSrc = GameObject.Find ("MNetwork").GetComponent<MNetwork>(); // Get the menu script
		playerDataSrc = GameObject.Find ("MPlayerData").GetComponent<MPlayerData>(); // Get the playerData script
		if(!networkSrc.gameInfo.isOnDedicatedServer){
			GetComponent<NetworkView>().RPC("AskStatus", RPCMode.Server, Network.player);	
		}
	}//Start
	
	void OnGUI(){	
		GUI.skin = text.wrCustomSkin;
		GUI.depth =50;		
		DefinePositions();
		DisplayPlayerList();
		DisplayGamePanel();		
		DisplayChat();
		DisplayButtons();
		
		// If GUI changed and if we are the host and if the game is not started
		 if(GUI.changed && networkSrc.isGameHost && !networkSrc.gameInfo.isStarted && !networkSrc.isPlayerExitGame){
			// Send the map modification to the all players
			GetComponent<NetworkView>().RPC("ChangeMap", RPCMode.All, networkSrc.gameInfo.mapKey);		
		}
		
	}//OnGUI
	
	private void DisplayPlayerList(){
		GUI.BeginGroup(playerListRec, "");
		GUI.Box(new Rect(0, 0, playerListSizeX, playerListSizeY), text.wrPlayerListTitle);
		float sizeY = text.margin*3;
		if(networkSrc.isGameHost){
			networkSrc.PingPlayers();
		}	
		playerScroll = GUI.BeginScrollView (new Rect (0,sizeY,playerListSizeX,playerListSizeY-text.margin*4), playerScroll, new Rect(0,0, playerListSizeX-text.margin*2, (networkSrc.playerList.Count*(playerListBoxSizeY+text.margin/2))-text.margin/2));
		sizeY=0;
		// Display the player list
		for(int i = 0; i < networkSrc.playerList.Count; i++){
			string serverMessage="";
			if(networkSrc.playerList[i].isGameHost == 1) {
				serverMessage = text.wrPlayerListHostTxt;
			} else {
				serverMessage = text.wrPlayerListClientTxt;
			}
			string playerStatus ="";
			if(networkSrc.playerList[i].isPlayerInGame == 1){
				playerStatus =text.wrPlayerListStatutGameTxt;
			} else {
				playerStatus =text.wrPlayerListStatWaitTxt;
			}			
			string pingText=""; 				
			// If the current ID is the host's ID : I ping the host (and if I have not been excuded of the game)
			if(networkSrc.playerList[i].isGameHost == 1 && !networkSrc.isGameHost && !networkSrc.isPlayerExculde) {
				pingText = Network.GetAveragePing(Network.connections[networkSrc.gameInfo.hostId]).ToString();
			// ELse (if it's a simple player) : display the ping wich have been done by the host
			} else {
				pingText = networkSrc.playerList[i].playerPing.ToString();
			}						
			GUI.Box(new Rect(text.margin,sizeY,playerListSizeX-text.margin*3, playerListBoxSizeY), "");
			GUI.Label(new Rect(text.margin*2,sizeY+text.margin/2,playerListSizeX, playerListBoxSizeY),networkSrc.playerList[i].playerName+" "+serverMessage+": "+ playerStatus+" "+pingText+" ms ");
			float buttonSizeX = playerListSizeX-buttonExculdeSizeX-text.margin*2.5f;
			
			// USE IF ONLY IF YOU HAVE MULTIONLINE :
			/*if((networkSrc.gameInfo.isOnline || networkSrc.gameInfo.isOnDedicatedServer) && playerDataSrc.useFriendlist){ // If it's an online game
				if(networkSrc.playerList[i].id != playerDataSrc.id){ // And the current player is not myself
					// Display button to add in friendlsit					
					if(!MOFriend.IsInList(playerDataSrc.friendList, networkSrc.playerList[i].id)){
						if(GUI.Button(new Rect(buttonSizeX, sizeY+text.margin/2, buttonExculdeSizeX,buttonExculdeSizeY), "+")){
							networkSrc.AskFriend(networkSrc.playerList[i]);
						}
					} else {
						GUI.Label(new Rect(buttonSizeX, sizeY+text.margin/2, buttonExculdeSizeX,buttonExculdeSizeY), "  +");
					}
				}
				buttonSizeX-= (buttonExculdeSizeX+text.margin/2);
			} */
			//---
			if(networkSrc.isGameHost && networkSrc.playerList[i].gameId != networkSrc.playerDataSrc.gameId){
				if(GUI.Button(new Rect(buttonSizeX, sizeY+text.margin/2, buttonExculdeSizeX,buttonExculdeSizeY), "X")){
					networkSrc.ExcludePlayer(networkSrc.playerList[i]);
				}
			}
			
			sizeY+=playerListBoxSizeY+text.margin/2;		
		}		
		GUI.EndScrollView();		
		GUI.EndGroup();
	}//DisplayPlayerList
	
	private void DisplayGamePanel(){		
		GUI.BeginGroup(gamePanelRec, "");		
		GUI.Box(new Rect(0, 0, gamePanelSizeX, gamePanelSizeY), text.wrGamePanelTitle);
		int sizeY = text.space+text.margin;
		int sizeYMap = sizeY;
		GUI.Label(new Rect(text.internalMargin,sizeY, text.labelSizeX,text.labelSizeY), text.wrGamePanelNameTxt);
		GUI.Label(new Rect(text.labelMargin,sizeY,text.fieldSizeX,text.fieldSizeY), networkSrc.gameInfo.name);
		sizeY+=text.labelSpace;
		GUI.Label(new Rect(text.internalMargin,sizeY, text.labelSizeX,text.labelSizeY), text.wrGamePanelPlayersNbrTxt);
		GUI.Label(new Rect(text.labelMargin,sizeY,text.fieldSizeX,text.fieldSizeY), networkSrc.gameInfo.totalPlayer + " / " + networkSrc.gameInfo.maxPlayer);
		sizeY+=text.labelSpace;
		GUI.Label(new Rect(text.internalMargin,sizeY, text.labelSizeX,text.labelSizeY), text.wrGamePanelMapTxt);
		GUI.Label(new Rect(text.labelMargin,sizeY,text.fieldSizeX,text.fieldSizeY), parameters.maps[networkSrc.gameInfo.mapKey]);
		sizeY+=text.space;
		if(parameters.mapsScreen[networkSrc.gameInfo.mapKey] != null && parameters.displayMapScreen) {
			GUI.DrawTexture(new Rect(gamePanelSizeX-parameters.mapScreenX-text.internalMargin,sizeYMap,parameters.mapScreenX, parameters.mapScreenY),  parameters.mapsScreen[networkSrc.gameInfo.mapKey],  ScaleMode.ScaleAndCrop, true);
		} 
		if(networkSrc.isGameHost){
			DisplayGameMapPanel(sizeY);			
		} 
		GUI.EndGroup();
	}//DisplayGamePanel
	
	// Spectial panel for the host
	private void DisplayGameMapPanel(int sizeY){	
		sizeY+=text.labelSpace;
		int mapSizeX = text.buttonSelectSizeX + text.internalMargin;
		int mapSizeY = gamePanelSizeY - sizeY - text.margin;		
		mapScroll = GUI.BeginScrollView (new Rect(text.internalMargin,sizeY,mapSizeX,mapSizeY), mapScroll, new Rect(0,0,mapSizeX-text.internalMargin, text.buttonSelectSizeY*parameters.maps.Length+text.margin));
		networkSrc.gameInfo.mapKey = GUI.SelectionGrid(new Rect(0, 0,text.buttonSelectSizeX, text.buttonSelectSizeY*parameters.maps.Length), networkSrc.gameInfo.mapKey, parameters.maps, 1);
		GUI.EndScrollView();			
	}//DisplayGameMapPanel
	
	private void DisplayChat(){
		GUI.BeginGroup(chatRec, "");
		GUI.Box(new Rect(0, 0, chatSizeX, chatSizeY), text.wrChatTitle);
		chatBox = new Rect(text.internalMargin, text.internalMargin, chatSizeX-text.internalMargin*2, chatSizeY-text.margin*5-chatButtonSizeY);
		GUI.Box(chatBox, "");
		int sizeY = text.margin;
		
		chatScroll = GUI.BeginScrollView (chatBox, chatScroll, new Rect(0,0, chatSizeX-text.internalMargin*3, 20*(networkSrc.chatContent.Count+1)));
		for(int i = 0; i < networkSrc.chatContent.Count; i++){			
			GUI.Label(new Rect(10,sizeY,chatSizeX, text.labelSizeY), networkSrc.chatContent[i]);
			sizeY+=20;
		}
		GUI.EndScrollView();
		
		formChatTxt = GUI.TextField(new Rect(text.internalMargin, chatSizeY-text.margin-chatButtonSizeY, chatSizeX-chatButtonSizeX-text.internalMargin*2-text.margin,chatButtonSizeY), formChatTxt);
		if(GUI.Button(new Rect(chatSizeX-chatButtonSizeX-text.internalMargin, 
			chatSizeY-text.margin-chatButtonSizeY, chatButtonSizeX, chatButtonSizeY),
			text.wrChatButtonTxt)){
			if(formChatTxt != "") {
				string message = playerDataSrc.nameInGame+" : "+formChatTxt;				
				int maxChar = (int)Math.Round((chatSizeX) / 6.9f);
				if(message.Length > maxChar){
					message = message.Substring(0, maxChar);
				}
			
				ChatMessage(message);
				formChatTxt = "";
			}
		}
		GUI.EndGroup();
	}//DisplayChat
	
	private void DisplayButtons(){
		GUI.BeginGroup(buttonsPanelRec, "");
		GUI.Box(new Rect(0, 0, buttonsPanelSizeX, buttonsPanelSizeY), "");
		if(GUI.Button(new Rect(text.margin, text.margin, text.buttonSubMenuSizeX, text.buttonSubMenuSizeY), text.wrExitButtonTxt)){
			networkSrc.isPlayerExitGame=true;
		}
		string textButton=text.wrGamePanelLoadButton;
		if(isLoading){
			GUI.enabled = false;
			textButton=loadGameCount;
		} else{
			if(!networkSrc.isGameHost){			
				if(!networkSrc.gameInfo.isStarted && !networkSrc.gameInfo.isOnDedicatedServer){
					GUI.enabled = false;
					textButton=text.wrGamePanelWaitButton;
				} else {
					textButton=text.wrGamePanelJoinButton;
				}
			}
		}
		if(GUI.Button(new Rect(buttonsPanelSizeX-text.buttonSubMenuSizeX-text.margin, text.margin, text.buttonSubMenuSizeX, text.buttonSubMenuSizeY), textButton)){
			// If I click on button, and I'm the host, and the game is not started :
			if(networkSrc.isGameHost && !networkSrc.gameInfo.isStarted){
				// Display load panel on the others players
				GetComponent<NetworkView>().RPC("LoadGame", RPCMode.All, networkSrc.gameInfo.mapKey, loadTimerTime);	
				if(useLoadTimer) { // If we use a load timer :
					// Display the load timer on the others players
					GetComponent<NetworkView>().RPC("PrivateChatMessage", RPCMode.All, text.wrGamePanelLoadMessage);	
				}
			} else {
				// Else : if I'm not the host, but I have the load button enabled :
				// = if I join a started game
				networkSrc.gameInfo.mapName = parameters.maps[networkSrc.gameInfo.mapKey]; // Save the map
				networkSrc.StartGame(true); // Load the game for me
			}
			isLoading = true;
		}
		if((!networkSrc.isGameHost && !networkSrc.gameInfo.isStarted) || isLoading){
			GUI.enabled = true;
		}

		GUI.EndGroup();
	}//DisplayButtons
	
	private void DefinePositions(){
		buttonsPanelSizeY= text.buttonSubMenuSizeY+text.margin*2;
		playerListSizeX =  Screen.width/3 - text.margin*2;
		playerListSizeY = Screen.height - text.margin*2;
		playerListPosX = text.margin;
		playerListPosY = text.margin;
				
		buttonsPanelSizeX = Screen.width- playerListSizeX - text.margin*3;
		buttonsPanelPosY = Screen.height - buttonsPanelSizeY - text.margin;
		buttonsPanelPosX = playerListSizeX + text.margin*2;
		
		gamePanelPosX = playerListSizeX + text.margin*2;
		gamePanelPosY = text.margin;
		gamePanelSizeX = Screen.width - playerListSizeX - text.margin*3;
		gamePanelSizeY = ((Screen.height - buttonsPanelSizeY - text.margin*3) / 2) - (text.margin/2);
		
		chatSizeX = gamePanelSizeX;
		chatSizeY = gamePanelSizeY;
		chatPosX = gamePanelPosX;
		chatPosY = gamePanelPosY + gamePanelSizeY + text.margin;

		buttonsPanelRec = new Rect(buttonsPanelPosX, buttonsPanelPosY, buttonsPanelSizeX, buttonsPanelSizeY);
		playerListRec = new Rect(playerListPosX, playerListPosY, playerListSizeX, playerListSizeY);
		gamePanelRec = new Rect(gamePanelPosX, gamePanelPosY, gamePanelSizeX, gamePanelSizeY);
		chatRec = new Rect(chatPosX, chatPosY, chatSizeX, chatSizeY);
	}//DefinePositions
	
		
	// Send message from
	private void ChatMessage(string message){
		networkSrc.SendChatMessage(message);
	}//ChatMessage
	
	
	// Display the loadPanel
	private IEnumerator LoadPanel(int timerStart){
		for(int i = timerStart; i > 0; i--){	
			yield return new WaitForSeconds(1);		
			loadGameCount = i.ToString();
			if(useLoadChatTimer){		
				PrivateChatMessage(i.ToString());	
			}
			
		}		
		yield return new WaitForSeconds(1);	
		// When the timer is finish : load the game
		networkSrc.StartGame(true);
	}//LoadPanel

	
	// Change the map : call from server to clients when a choose a new map
	[RPC]
	void ChangeMap(int newMapKey){
		networkSrc.gameInfo.mapKey = newMapKey;
		networkSrc.gameInfo.mapName = parameters.maps[newMapKey];
		if(networkSrc.gameInfo.isOnline){			
			networkSrc.playerDataSrc.RefreshGameMap(networkSrc.gameInfo.id, parameters.maps[newMapKey]);	
		}
	}//ChangeMap
	
	// Load the game : call from server to the clients when he click on "Start game"
	[RPC]
	void LoadGame(int newMapKey, int timerStart){
		networkSrc.gameInfo.mapKey = newMapKey;
		networkSrc.gameInfo.mapName = parameters.maps[newMapKey];
		isLoading = true;
		if(useLoadTimer) { // If we use timer : start it
			StartCoroutine(LoadPanel(timerStart));
		} else { // Else : start the game directly
			networkSrc.StartGame(true);
		}
	}//LoadGame
	
	[RPC]
	void AskStatus(NetworkPlayer player){
		// If the game is currently loading
		if(isLoading){ // Make the new player start the timer with the current value
			GetComponent<NetworkView>().RPC("LoadGame", player, networkSrc.gameInfo.mapKey, int.Parse(loadGameCount));
		}
	}//AskStatus
	
	[RPC]
	void PrivateChatMessage(string message){
		networkSrc.chatContent.Add (message);
		chatScroll = new Vector2(0, (20*(networkSrc.chatContent.Count+1)) - chatBox.height);
	}//PrivateChatMessage
	

}



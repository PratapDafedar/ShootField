using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;
using uLink;

public class MWaitRoom : uLink.MonoBehaviour {

    public static MWaitRoom Instance;

	//------ ATTACHED SCRIPTS
	public MGameParameter parameters;	
	public MText text;
	
	//------ TIMER PARAMETRES 
	public bool useLoadTimer = true;
	public bool useLoadChatTimer = true;
	public int loadTimerTime = 5;
	public bool isLoading=false;
	public string loadGameCount;
	
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
	
	void Awake()
    {
        Instance = this;
		this.name="WaitRoom";		
	}//Awake
	
	void Start()
    {
		Instantiate(Resources.Load("MGameMenu"));
		text = GameObject.Find ("MText").GetComponent<MText>(); // Get the menu script
        parameters = GameObject.Find("LevelOption").GetComponent<MGameParameter>(); // Get the menu script
        if (!NetworkManager.Instance.gameInfo.isOnDedicatedServer)
        {
            NetworkManager.Instance.AskStatus_(uLink.Network.player);	
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
        if (GUI.changed && GameManager.Instance.cPlayer.isGameHost && !NetworkManager.Instance.gameInfo.isStarted && !NetworkManager.Instance.isPlayerExitGame)
        {
			// Send the map modification to the all players
             NetworkManager.Instance.ChangeMap_();	
		}
		
	}//OnGUI
	
	private void DisplayPlayerList(){
		GUI.BeginGroup(playerListRec, "");
		GUI.Box(new Rect(0, 0, playerListSizeX, playerListSizeY), text.wrPlayerListTitle);
		float sizeY = text.margin*3;
        if (GameManager.Instance.cPlayer.isGameHost)
        {
            NetworkManager.Instance.PingPlayers();
		}
        playerScroll = GUI.BeginScrollView(new Rect(0, sizeY, playerListSizeX, playerListSizeY - text.margin * 4), playerScroll, new Rect(0, 0, playerListSizeX - text.margin * 2, (NetworkManager.Instance.playerList.Count * (playerListBoxSizeY + text.margin / 2)) - text.margin / 2));
		sizeY=0;

		// Display the player list
        GUI.Box(new Rect(text.margin, sizeY, playerListSizeX - text.margin * 3, playerListBoxSizeY), "TEAM - BLUE");
        sizeY += playerListBoxSizeY + text.margin / 2;

        for (int i = 0; i < NetworkManager.Instance.playerList.Count; i++)
        {
            if (NetworkManager.Instance.playerList[i].cTeam == User.Team.BLUE)
            {
                string serverMessage = "";
                if (NetworkManager.Instance.playerList[i].isGameHost == true)
                {
                    serverMessage = text.wrPlayerListHostTxt;
                }
                else
                {
                    serverMessage = text.wrPlayerListClientTxt;
                }
                string playerStatus = "";
                if (NetworkManager.Instance.playerList[i].isPlayerInGame == true)
                {
                    playerStatus = text.wrPlayerListStatutGameTxt;
                }
                else
                {
                    playerStatus = text.wrPlayerListStatWaitTxt;
                }
                string pingText = "";
                // If the current ID is the host's ID : I ping the host (and if I have not been excuded of the game)
                if (NetworkManager.Instance.playerList[i].isGameHost == true && !GameManager.Instance.cPlayer.isGameHost && !NetworkManager.Instance.isPlayerExculde)
                {
                    pingText = uLink.Network.GetAveragePing(uLink.Network.connections[i]).ToString();
                    // ELse (if it's a simple player) : display the ping wich have been done by the host
                }
                else
                {
                    pingText = NetworkManager.Instance.playerList[i].playerPing.ToString();
                }
                GUI.Box(new Rect(text.margin, sizeY, playerListSizeX - text.margin * 3, playerListBoxSizeY), "");
                GUI.Label(new Rect(text.margin * 2, sizeY + text.margin / 2, playerListSizeX, playerListBoxSizeY - 4), NetworkManager.Instance.playerList[i].name + " " + serverMessage + ": " + playerStatus + " " + pingText + " ms ");
                float buttonSizeX = playerListSizeX - buttonExculdeSizeX - text.margin * 2.5f;
            }
						
#if false
			if(networkSrc.isGameHost && networkSrc.playerList[i].gameId != networkSrc.playerDataSrc.gameId){
				if(GUI.Button(new Rect(buttonSizeX, sizeY+text.margin/2, buttonExculdeSizeX,buttonExculdeSizeY), "X")){
					networkSrc.ExcludePlayer(networkSrc.playerList[i]);
				}
			}
#endif
			sizeY+=playerListBoxSizeY+text.margin/2;		
		}

        GUI.Box(new Rect(text.margin, sizeY, playerListSizeX - text.margin * 3, playerListBoxSizeY), "TEAM - RED");
        sizeY += playerListBoxSizeY + text.margin / 2;
        for (int i = 0; i < NetworkManager.Instance.playerList.Count; i++)
        {
            if (NetworkManager.Instance.playerList[i].cTeam == User.Team.RED)
            {
                string serverMessage = "";
                if (NetworkManager.Instance.playerList[i].isGameHost == true)
                {
                    serverMessage = text.wrPlayerListHostTxt;
                }
                else
                {
                    serverMessage = text.wrPlayerListClientTxt;
                }
                string playerStatus = "";
                if (NetworkManager.Instance.playerList[i].isPlayerInGame == true)
                {
                    playerStatus = text.wrPlayerListStatutGameTxt;
                }
                else
                {
                    playerStatus = text.wrPlayerListStatWaitTxt;
                }
                string pingText = "";
                // If the current ID is the host's ID : I ping the host (and if I have not been excuded of the game)
                if (NetworkManager.Instance.playerList[i].isGameHost == true && !GameManager.Instance.cPlayer.isGameHost && !NetworkManager.Instance.isPlayerExculde)
                {
                    pingText = uLink.Network.GetAveragePing(uLink.Network.connections[i]).ToString();
                    // ELse (if it's a simple player) : display the ping wich have been done by the host
                }
                else
                {
                    pingText = NetworkManager.Instance.playerList[i].playerPing.ToString();
                }
                GUI.Box(new Rect(text.margin, sizeY, playerListSizeX - text.margin * 3, playerListBoxSizeY), "");
                GUI.Label(new Rect(text.margin * 2, sizeY + text.margin / 2, playerListSizeX, playerListBoxSizeY - 4), NetworkManager.Instance.playerList[i].name + " " + serverMessage + ": " + playerStatus + " " + pingText + " ms ");
                float buttonSizeX = playerListSizeX - buttonExculdeSizeX - text.margin * 2.5f;

                sizeY += playerListBoxSizeY + text.margin / 2;
            }
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
        GUI.Label(new Rect(text.labelMargin, sizeY, text.fieldSizeX, text.fieldSizeY), NetworkManager.Instance.gameInfo.name);
		sizeY+=text.labelSpace;
		GUI.Label(new Rect(text.internalMargin,sizeY, text.labelSizeX,text.labelSizeY), text.wrGamePanelPlayersNbrTxt);
        GUI.Label(new Rect(text.labelMargin, sizeY, text.fieldSizeX, text.fieldSizeY), NetworkManager.Instance.gameInfo.totalPlayer + " / " + NetworkManager.Instance.gameInfo.maxPlayer);
		sizeY+=text.labelSpace;
		GUI.Label(new Rect(text.internalMargin,sizeY, text.labelSizeX,text.labelSizeY), text.wrGamePanelMapTxt);
        GUI.Label(new Rect(text.labelMargin, sizeY, text.fieldSizeX, text.fieldSizeY), parameters.maps[NetworkManager.Instance.gameInfo.mapKey]);
		sizeY+=text.space;
        if (parameters.mapsScreen[NetworkManager.Instance.gameInfo.mapKey] != null && parameters.displayMapScreen)
        {
            GUI.DrawTexture(new Rect(gamePanelSizeX - parameters.mapScreenX - text.internalMargin, sizeYMap, parameters.mapScreenX, parameters.mapScreenY), parameters.mapsScreen[NetworkManager.Instance.gameInfo.mapKey], ScaleMode.ScaleAndCrop, true);
		}
        if (GameManager.Instance.cPlayer.isGameHost)
        {
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
        NetworkManager.Instance.gameInfo.mapKey = GUI.SelectionGrid(new Rect(0, 0, text.buttonSelectSizeX, text.buttonSelectSizeY * parameters.maps.Length), NetworkManager.Instance.gameInfo.mapKey, parameters.maps, 1);
		GUI.EndScrollView();			
	}//DisplayGameMapPanel
	
	private void DisplayChat(){
		GUI.BeginGroup(chatRec, "");
		GUI.Box(new Rect(0, 0, chatSizeX, chatSizeY), text.wrChatTitle);
		chatBox = new Rect(text.internalMargin, text.internalMargin, chatSizeX-text.internalMargin*2, chatSizeY-text.margin*5-chatButtonSizeY);
		GUI.Box(chatBox, "");
		int sizeY = text.margin;

        chatScroll = GUI.BeginScrollView(chatBox, chatScroll, new Rect(0, 0, chatSizeX - text.internalMargin * 3, 20 * (NetworkManager.Instance.chatContent.Count + 1)));
        for (int i = 0; i < NetworkManager.Instance.chatContent.Count; i++)
        {
            GUI.Label(new Rect(10, sizeY, chatSizeX, text.labelSizeY), NetworkManager.Instance.chatContent[i]);
			sizeY+=20;
		}
		GUI.EndScrollView();
		
		formChatTxt = GUI.TextField(new Rect(text.internalMargin, chatSizeY-text.margin-chatButtonSizeY, chatSizeX-chatButtonSizeX-text.internalMargin*2-text.margin,chatButtonSizeY), formChatTxt);
		if(GUI.Button(new Rect(chatSizeX-chatButtonSizeX-text.internalMargin, 
			chatSizeY-text.margin-chatButtonSizeY, chatButtonSizeX, chatButtonSizeY),
			text.wrChatButtonTxt)){
			if(formChatTxt != "") {
				string message = GameManager.Instance.cPlayer.name + " : " + formChatTxt;				
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
            NetworkManager.Instance.isPlayerExitGame = true;
		}
		string textButton=text.wrGamePanelLoadButton;
		if(isLoading){
			GUI.enabled = false;
			textButton=loadGameCount;
		} else{
            if (!GameManager.Instance.cPlayer.isGameHost)
            {
                if (!NetworkManager.Instance.gameInfo.isStarted && !NetworkManager.Instance.gameInfo.isOnDedicatedServer)
                {
					GUI.enabled = false;
					textButton=text.wrGamePanelWaitButton;
				} else {
					textButton=text.wrGamePanelJoinButton;
				}
			}
		}
		if(GUI.Button(new Rect(buttonsPanelSizeX-text.buttonSubMenuSizeX-text.margin, text.margin, text.buttonSubMenuSizeX, text.buttonSubMenuSizeY), textButton)){
			// If I click on button, and I'm the host, and the game is not started :
            if (GameManager.Instance.cPlayer.isGameHost && !NetworkManager.Instance.gameInfo.isStarted)
            {
				// Display load panel on the others players
                NetworkManager.Instance.LoadGame_(NetworkManager.Instance.gameInfo.mapKey, loadTimerTime);	
				if(useLoadTimer) { // If we use a load timer :
					// Display the load timer on the others players
                    NetworkManager.Instance.PrivateChatMessage_(text.wrGamePanelLoadMessage);	
				}
			} else {
				// Else : if I'm not the host, but I have the load button enabled :
				// = if I join a started game
                NetworkManager.Instance.gameInfo.mapName = parameters.maps[NetworkManager.Instance.gameInfo.mapKey]; // Save the map
                NetworkManager.Instance.StartGame(true); // Load the game for me
			}
			isLoading = true;
		}
        if ((!GameManager.Instance.cPlayer.isGameHost && !NetworkManager.Instance.gameInfo.isStarted) || isLoading)
        {
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
        NetworkManager.Instance.SendChatMessage(message);
	}//ChatMessage
	
	
	// Display the loadPanel
	public IEnumerator LoadPanel(int timerStart){
		for(int i = timerStart; i > 0; i--){	
			yield return new WaitForSeconds(1);		
			loadGameCount = i.ToString();
			if(useLoadChatTimer){
                NetworkManager.Instance.PrivateChatMessage_(i.ToString());	
			}
			
		}		
		yield return new WaitForSeconds(1);	
		// When the timer is finish : load the game
        NetworkManager.Instance.StartGame(true);
	}//LoadPanel

	
	

}



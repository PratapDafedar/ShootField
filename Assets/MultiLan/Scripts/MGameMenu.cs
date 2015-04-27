using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MGameMenu : MonoBehaviour {
	// Parameters
	public bool useExit = true;
	public bool useChat = true;
	public bool usePlayerList = true;
	
	public MNetwork networkSrc;
	public MText text;
	public bool viewChat;
	public bool viewPlayerList;

	private Rect buttonPosRect;
	private Rect buttonPos;
	
	// Game pannel positions and sizes settings
	private int menuSizeX = 450;
	public int menuSizeY = 200;	
	private int menuPosX;
	private int menuPosY;
	private Rect menu;
	private int buttonSizeX = 150;
	private int buttonSizeY = 30;
	private int sizeY = 30;	
	
	void Awake(){
		DontDestroyOnLoad(this);	
		this.name = "MGameMenu";		
	}//Awake
	
	void Start(){
		networkSrc = GameObject.Find ("MNetwork").GetComponent<MNetwork>();
		text = GameObject.Find ("MText").GetComponent<MText>();
	}//Start
	
	void Update(){
		// Search if the player want to exit the game (down the Escape key)
		if(Input.GetKeyDown(KeyCode.Escape)) {
			networkSrc.isPlayerExitGame = true;
		}	
		
	}//Upadte
	
	public void OnGUI(){		
		GUI.depth =0;
		GUI.skin = text.gmCustomSkin;
		// Display game Pannels
		DisplayGamePannel();	
		// Display game menus
		buttonPosRect = new Rect(5,Screen.height-25,60,20);
		if(!networkSrc.isGameServerRebuild && !networkSrc.isGameServerRebuildFailed && !networkSrc.isPlayerExitGame && networkSrc.gameInfo.isStarted && networkSrc.playerDataSrc.isInGame) {
			buttonPos = buttonPosRect;
			if(useExit){				
				if(GUI.Button(buttonPos, text.gmMenuExit)){
					networkSrc.isPlayerExitGame=true;				
				}
				buttonPos.x += 65;
			}	
			if(useChat){
				if(GUI.Button(buttonPos, text.gmMenuChat)){
					if(viewChat) {
						viewChat=false;
					} else {
						viewChat=true;
					}				
				}
				buttonPos.x += 65;
			}
			if(usePlayerList){
				if(GUI.Button(buttonPos, text.gmMenuPL)){
					if(viewPlayerList) {
						viewPlayerList=false;
					} else {
						viewPlayerList=true;
					}
				}	
			}
		}
	}//OnGUI
	
	public void DisplayGamePannel(){		
		// Positions settings
		menuPosX =  Screen.width/2-(menuSizeX/2);
		menuPosY = Screen.height/2-(menuSizeY/2);
		menu = new Rect(menuPosX, menuPosY, menuSizeX, menuSizeY);

		GUI.BeginGroup(menu);	
		//  IF THE HOST TRY TO EXCULDE A PLAYER
		if (networkSrc.isExculdePlayer && networkSrc.isGameHost){	
			DisplayExcludePlayer();
			
		// IF A PLAYER HAVE BEEN EXCULDED
		} else if(networkSrc.isPlayerExculde){	
			DisplayExculdedPlayer();	
			
		// IF THE SERVER IS REBUILD	
		} else if(networkSrc.isGameServerRebuild){
			DisplayRebuild();
		
		// IF A PLAYER WANT ADD A FRIEND
		} else if(networkSrc.isFriendPlayer){	
			DisplayFriendPlayer();	
		
		// IF A PLAYER ASK US TO BE FRIEND
		} else if(networkSrc.isFriendAsk){	
			DisplayFriendAsk();	
		
		// IF AD ANSWER TO FRIENDLIST ADDING
		} else if(networkSrc.isFriendAskYes || networkSrc.isFriendAskNo){	
			DisplayFriendAnswer();	
		
		// ELSE, IF THE SERVER REBUILD FAILED :
		} else if(networkSrc.isGameServerRebuildFailed){
			DisplayRebuildFailed();
		
		 // ELSE, IF THE DEDICATED SERVER IS BREALDOWN :
		} else if(networkSrc.isDedicatedServerBreakDown){
			DisplayBreakDownServer();
		
		// ELSE IF THE PLAYER WANT TO EXIT THE GAME
		// and if we are not on the main menu scene (because this scene have her own exit system)
		} else if(networkSrc.isPlayerExitGame && Application.loadedLevelName != networkSrc.mainMenuName){
			DisplayExit();
		} 	
		
		GUI.EndGroup();	
	}//DisplayGamePannel
		
	public void DisplayExit(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmExitTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmExitMessage);
		sizeY+= 100;
		
		// If we click on "OK" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmExitButton)){
			// Exit the game
			networkSrc.ExitGame(true);							
		}
		// If we click on "cancel" :
		if(GUI.Button(new Rect(buttonSizeX+40, sizeY, buttonSizeX, buttonSizeY), text.gmExitCancelButton)){
			networkSrc.isPlayerExitGame = false;
		}		
	}//DisplayExit
	
	public void DisplayRebuild(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmRebuildTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildSubTitle);
		sizeY+= 30;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildInfoTxt);
		sizeY+= 20;
		if(networkSrc.isGameServerWaitingPlayers){				
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildSearchHostTxt);
			sizeY+= 30;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildSearchHostWaitTxt);
		} else {				
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildSeachNewHostTxt);
			sizeY+= 30;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmRebuildTimerTxt+networkSrc.rebuildServerTimer.ToString()+text.gmRebuildTimerSecTxt);
		}
	}//DisplayRebuild
	
	public void DisplayRebuildFailed(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmRebuildTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.nmRebuildSearchTxt);				
		sizeY+= 100;
		// If we want to try the rebuild one more time :
		if(GUI.Button(new Rect(20,sizeY, buttonSizeX, buttonSizeY), text.gmRebuildFailedTryButton)) {
			// Call StartRebuildServer to restart the rebuild
			networkSrc.StartRebuildServer();	
		}
		if(GUI.Button(new Rect(buttonSizeX+50,sizeY, buttonSizeX, buttonSizeY), text.gmRebuildFailedExitButton)) {
			// Exit the game
			networkSrc.ExitGame(true);		
		}
	}//DisplayRebuildFailed
	
	public void DisplayBreakDownServer(){
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmBreakDownTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmBreakDownTxt);				
		sizeY+= 100;		
		if(GUI.Button(new Rect(20,sizeY, buttonSizeX, buttonSizeY), text.gmRebuildFailedExitButton)) {
			// Exit the game
			networkSrc.ExitGame(true);		
		}
	}//DisplayBreakDownServer
	
	public void DisplayExcludePlayer(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmExcludeTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmExcludeText);
		sizeY+= 20;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), networkSrc.excludedPlayer.playerName);
		sizeY+= 40;
		if(networkSrc.gameInfo.isOnline && networkSrc.playerDataSrc.useBlacklist){
			networkSrc.alwaysExcudePlayer=GUI.Toggle(new Rect(20,sizeY,menuSizeX,50), networkSrc.alwaysExcudePlayer, text.gmExcludeBlacklist);
		}
		sizeY+= 50;
		// If we click on "YES" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmYes)){
			networkSrc.ConfirmExcludePlayer();
		}
		// If we click on "NO" :
		if(GUI.Button(new Rect(buttonSizeX+40, sizeY, buttonSizeX, buttonSizeY), text.gmNo)){
			networkSrc.CancelExcludePlayer();
		}
	}//DisplayExcludePlayer
	
	public void DisplayExculdedPlayer(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmExcludePlayerTitle );
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmExcludePlayerText );
		sizeY+= 100;
			
		// If the game is online 
		if(networkSrc.gameInfo.isOnline){
			// Exit the game on the dataBase
			networkSrc.playerDataSrc.ExitGame();
		}			
		// If we click on "OK" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmExcludePlayerButton)){
			// Exit the game
			networkSrc.ExitGame(false);			
		}		
	}//DisplayExculdedPlayer
	
	public void DisplayFriendPlayer(){	
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmFriendTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmFriendText);
		sizeY+= 20;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), networkSrc.friendPlayer.playerName);
		sizeY+= 50;
		// If we click on "YES" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmYes)){
			networkSrc.SendFriendAsk();
		}
		// If we click on "NO" :
		if(GUI.Button(new Rect(buttonSizeX+40, sizeY, buttonSizeX, buttonSizeY), text.gmNo)){
			networkSrc.isFriendPlayer = false;
		}
	}//DisplayFriendPlayer
	
	public void DisplayFriendAsk(){
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmFriendAskTitle);
		sizeY+= 10;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), networkSrc.friendAskPlayer.playerName+text.gmFriendAskText);
		sizeY+= 20;
		GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), text.gmFriendAskQuestion);	
		sizeY+= 50;
		// If we click on "YES" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmYes)){
			networkSrc.FriendAskAnswer(true);
		}
		// If we click on "NO" :
		if(GUI.Button(new Rect(buttonSizeX+40, sizeY, buttonSizeX, buttonSizeY), text.gmNo)){
			networkSrc.FriendAskAnswer(false);
		}
	}//DisplayFriendAsk
	
	public void DisplayFriendAnswer(){
		int sizeY=this.sizeY;
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), text.gmFriendAnswerTitle);
		sizeY+= 10;
		if(networkSrc.isFriendAskYes){
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), networkSrc.friendPlayer.playerName+text.gmFriendAnswerTextYes);	
		}else{
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), networkSrc.friendPlayer.playerName+text.gmFriendAnswerTextNo);	
		}
		sizeY+= 50;
		// If we click on "YES" :
		if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), text.gmOk)){
			networkSrc.friendPlayer=null;
			networkSrc.isFriendAskYes = false;
			networkSrc.isFriendAskNo = false;
		}
	}//DisplayFriendAnswer	
}//MGameMenu

using UnityEngine;
using System.Collections;
using MultiPlayer;

public class MGamePlayerList : MonoBehaviour {
	
	/**************** SCRIPTS AND OBJECTS ****************************/
	private MGameMenu menuSrc;	
	public Vector2 listScroll = Vector2.zero;	
	
	
	// Use this for initialization
	void Start () {
		// Find menuSrc
		menuSrc = GameObject.Find ("MGameMenu").GetComponent<MGameMenu>();
		listScroll = new Vector2(0, 0);
	}//Start
	
	void OnGUI(){		
		if(!menuSrc.networkSrc.isGameServerRebuild && !menuSrc.networkSrc.isGameServerRebuildFailed && !menuSrc.networkSrc.isPlayerExitGame && menuSrc.networkSrc.gameInfo.isStarted) {		
			// Positions settings
			int margin = 5;
			int listSizeX = 0;
			int listSizeY = 0;
			if(35*(menuSrc.networkSrc.playerList.Count)+15 > 225){
				listSizeY=225;
				listSizeX=215;
			}else {
				listSizeY=35*(menuSrc.networkSrc.playerList.Count)+15;
				listSizeX=200;
			}
			
			int listPosX = margin;
			int listPosY = margin;
			Rect listMenu = new Rect(listPosX, listPosY, listSizeX, listSizeY);		
					
			int sizeY = 10;			
			
			if(menuSrc.viewPlayerList){
				GUI.BeginGroup(listMenu, "");			
				GUI.Box(new Rect(0,0,listSizeX, listSizeY), "");				
				sizeY=10;				
				
				listScroll = GUI.BeginScrollView (new Rect (0,sizeY,listSizeX,listSizeY-20), listScroll, new Rect(0,sizeY, listSizeX-20, 35*(menuSrc.networkSrc.playerList.Count)-5));						
				
				
				if(menuSrc.networkSrc.isGameHost && menuSrc.networkSrc.playerList.Count > 1){ // If I'm the host :			
					MUser.PingPlayers(menuSrc.networkSrc);
				}				
				
				for(int i = 0; i < menuSrc.networkSrc.playerList.Count; i++){									
					GUI.Box(new Rect(10,sizeY,180, 30), "");	
					GUI.Label(new Rect(15,sizeY+6,170, 20), menuSrc.networkSrc.playerList[i].playerName+" "+MUser.GetHostMessage(i, menuSrc.networkSrc)+" : "+MUser.GetPlayerPing(i, menuSrc.networkSrc)+" ms");
					
					// USE IF ONLY IF YOU HAVE MULTIONLINE : 
					/*if(menuSrc.networkSrc.playerDataSrc.useFriendlist){
						if(menuSrc.networkSrc.playerList[i].gameId != menuSrc.networkSrc.playerDataSrc.gameId && menuSrc.networkSrc.playerDataSrc.isOnline) {
							
							if(!MOFriend.IsInList(menuSrc.networkSrc.playerDataSrc.friendList, menuSrc.networkSrc.playerList[i].id)) {
								if(GUI.Button(new Rect(165,sizeY+6,20, 20), "+")){
									menuSrc.networkSrc.AskFriend(menuSrc.networkSrc.playerList[i]);
								}
							} else {
								GUI.Label(new Rect(165,sizeY+6,20, 20), "  +");
							}
						}
					}*/
					//---
					if(menuSrc.networkSrc.isGameHost && menuSrc.networkSrc.playerList[i].gameId != menuSrc.networkSrc.playerDataSrc.gameId){
						int sizeZ = 165;
						if(menuSrc.networkSrc.playerDataSrc.isOnline && menuSrc.networkSrc.playerDataSrc.useFriendlist){
							sizeZ = 140;
						}
						if(GUI.Button(new Rect(sizeZ,sizeY+6,20, 20), "x")){
							menuSrc.networkSrc.ExcludePlayer(menuSrc.networkSrc.playerList[i]);				
						}
					}
					sizeY+=35;
				}								
				GUI.EndScrollView();
				GUI.EndGroup();					
			}
		}
	}//OnGUI
}//MGamePlayerList

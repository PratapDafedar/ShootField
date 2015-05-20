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
			if(35*(menuSrc.networkSrc.playerTable.Count)+15 > 225){
				listSizeY=225 + 70;
				listSizeX=400;
			}else {
				listSizeY=35*(menuSrc.networkSrc.playerTable.Count)+15 + 70;
				listSizeX=400;
			}
			
			int listPosX = margin;
			int listPosY = margin;
			Rect listMenu = new Rect(listPosX, listPosY, listSizeX, listSizeY);		
					
			int sizeY = 10;			
			
			if(menuSrc.viewPlayerList || Input.GetKey (KeyCode.Tab)){
				GUI.BeginGroup(listMenu, "");			
				GUI.Box(new Rect(0,0,listSizeX, listSizeY), "");				
				sizeY=10;				
				
				listScroll = GUI.BeginScrollView (new Rect (0,sizeY,listSizeX,listSizeY-20), listScroll, new Rect(0,sizeY, listSizeX-20, 35*(menuSrc.networkSrc.playerTable.Count)-5));						
				
				
				if(GameManager.Instance.cPlayer.isGameHost && menuSrc.networkSrc.playerTable.Count > 1)
                { // If I'm the host :			
					User.PingPlayers(menuSrc.networkSrc);
				}
				
                GUI.Box(new Rect(10, sizeY, 180, 30), "TEAM - BLUE       : " + GameManager.Instance.teamScoreBlue);
                sizeY += 35;
				for(int i = 0; i < menuSrc.networkSrc.playerTable.Count; i++)
                {
                    if (menuSrc.networkSrc.playerTable[i].cTeam == User.Team.BLUE)
                    {
                        //GUI.Box(new Rect(10, sizeY, 180, 30), "");
                        GUI.Label(new Rect(15, sizeY + 6, 350, 20), menuSrc.networkSrc.playerTable[i].name + " " 
                                                                    + User.GetHostMessage(i, menuSrc.networkSrc) + " : " 
                                                                    + User.GetPlayerPing(i, menuSrc.networkSrc) + " ms"
                                                                    + " <--> Kills :   " + menuSrc.networkSrc.playerTable[i].score);
                        sizeY += 35;
                    }
				}

                GUI.Box(new Rect(10, sizeY, 180, 30), "TEAM - RED       : " + GameManager.Instance.teamScoreRed);
                sizeY += 35;
                for (int i = 0; i < menuSrc.networkSrc.playerTable.Count; i++)
                {
                    if (menuSrc.networkSrc.playerTable[i].cTeam == User.Team.RED)
                    {
                        //GUI.Box(new Rect(10, sizeY, 180, 30), "");
                        GUI.Label(new Rect(15, sizeY + 6, 350, 20), menuSrc.networkSrc.playerTable[i].name + " " 
                                                                    + User.GetHostMessage(i, menuSrc.networkSrc) + " : "
                                                                    + User.GetPlayerPing(i, menuSrc.networkSrc) + " ms"
                                                                    + " <--> Kills :   " + menuSrc.networkSrc.playerTable[i].score);
                        sizeY += 35;
                    }
                }		
				GUI.EndScrollView();
				GUI.EndGroup();					
			}
		}
	}//OnGUI
}//MGamePlayerList

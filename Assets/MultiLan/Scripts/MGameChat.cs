using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MGameChat : MonoBehaviour {
	private MGameMenu menuSrc;	
	
	public Vector2 chatScroll = Vector2.zero;	
	public int chatContentSizeY;
	
	public string textChat;	
	public int chatPanelSizeX=300;
	public int chatPanelSizeY=160;
	public int chatButtonSizeX=40;
	public int chatButtonsSizeY=20;
	public int chatPanelPosX= 5;
	public int 	margin = 5;
	private int chatBoxSizeY;
	private int chatBoxSizeX;
			
	// Use this for initialization
	void Start () {
		menuSrc = GameObject.Find ("MGameMenu").GetComponent<MGameMenu>();
		chatScroll = new Vector2(0, (20*(menuSrc.networkSrc.chatContent.Count+1)) - chatBoxSizeY);		
	}
	
	void OnGUI(){	
		if(!menuSrc.networkSrc.isGameServerRebuild && !menuSrc.networkSrc.isGameServerRebuildFailed && !menuSrc.networkSrc.isPlayerExitGame && menuSrc.networkSrc.gameInfo.isStarted) {		
			// Positions settings
			int chatFieldSizeX = chatPanelSizeX-chatButtonSizeX-margin*4;
			int chatFieldSieY= chatButtonsSizeY;
			chatBoxSizeX = chatPanelSizeX-margin*2;
			chatBoxSizeY = chatPanelSizeY-chatButtonsSizeY-margin*3;			
			int chatPanelPoxY=Screen.height-25-chatPanelSizeY-margin;
 
			Rect chatRect = new Rect(chatPanelPosX, chatPanelPoxY, chatPanelSizeX, chatPanelSizeY);
			int sizeY=0;
			if(menuSrc.viewChat){
				GUI.BeginGroup(chatRect, "");			
				GUI.Box(new Rect(0,0,chatPanelSizeX, chatPanelSizeY), "");		
				sizeY=margin;
				GUI.Box(new Rect(margin,sizeY,chatBoxSizeX, chatBoxSizeY), "");				
				chatScroll = GUI.BeginScrollView (new Rect (margin,sizeY,chatBoxSizeX,chatBoxSizeY), chatScroll, new Rect(0,0, chatBoxSizeX-margin*4, 20*menuSrc.networkSrc.chatContent.Count+margin*2));						
				for(int i = 0; i < menuSrc.networkSrc.chatContent.Count; i++){
					GUI.Label(new Rect(margin*2,sizeY,chatBoxSizeX-margin*2, 30), menuSrc.networkSrc.chatContent[i]);
					sizeY+=20;
				}
									
				GUI.EndScrollView();
					
				textChat=GUI.TextField(new Rect(margin, chatPanelSizeY-margin-chatFieldSieY, chatFieldSizeX, chatFieldSieY), textChat);
				if(GUI.Button(new Rect(chatFieldSizeX+margin*2, chatPanelSizeY-margin-chatFieldSieY, chatButtonSizeX, chatButtonsSizeY), menuSrc.text.gmChatButtonTxt) || Input.GetKeyDown(KeyCode.KeypadEnter)){
					if(textChat != "") {
						string message = menuSrc.networkSrc.playerDataSrc.nameInGame+" : "+textChat;
						int maxChar = (int)Math.Round((chatBoxSizeX-20) / 6.8f);
						if(message.Length > maxChar){
							message = message.Substring(0, maxChar);
						}
						ChatMessage(message);
						textChat = "";
					}
				}
				GUI.EndGroup();	
			}				
		}
	}		
	private void ChatMessage(string message){
		menuSrc.networkSrc.SendChatMessage(message);		
	}
}

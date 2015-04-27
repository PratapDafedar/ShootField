using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;

using MultiPlayer;
using Filter;

public class MLMenu : MonoBehaviour {	
	/*********************** IP SERVICE **************************/	
	/* IF YOU USE ONLY MULTILAN :
	 * URL of the ip service, use in GetPublicAddress(), you can use an other web site, 
	 * but in this case, you probably have to change some things on the GetPublicAddress function
	 * 
	 * The best thing to do (if you have a web hosting) is to create your own ip service
	 * using only this simple PHP code : <?php echo $_SERVER['REMOTE_ADDR']; ?> 
	 * if you to that, go on the GetPublicAddress() function at line 581 to read the instructions
	 * */	
	public string ipService ="http://www.mon-ip.com/";
	
	/* IF YOU USE MULTILAN WITH MULTIONLINE
	 * The page askIp.php provided with MultiOnline make you have your own ip service
	 * on your web hosting (in this way you needn't connect on an external website)
	 * You juste have to put the address of the page askIp.php here :
	 * */
	public string ipOnline ="";
	
	/******** OBJECTS, SCRIPTS ANS LISTS *******************/
	private MMenu m;
	private List<MGame> gameList;
	private List<MGame> searchList;
	private List<MGame> filterList;
	private List<string> networkIPList;
	
	/************** SCRIPT PARAMETERS *************************/
	public bool useJoinIP=true;
	public bool useNetworkGames=true;
	private bool searchingGame;
	private int searchNextIpKey;
	private int sortColumn;
	private int sortType;
	public string defaultPlayerName = "New Player"; 
	
	/******************** PROFIL ****************************/
	public string profilMessage;
	public bool viewProfilMessage;
	// --- Form fields
	public string formProfilName;
	
	/***************** MULTIPLAYER VIEW GAMES ****************************/	
	public string[] subMenuMultiButtons;
	public int subMenuMultiButton;		
	public string viewMessage;
	public string viewErrorMessage;
	
	// --- Form fields	
	public Vector2 viewScroll = Vector2.zero;
	public Vector2 viewFilterScroll = Vector2.zero;
	public string formViewPort;
	public string formViewSearch;
	public bool[] formViewFilterMap;
	public bool[] formViewFilterPlayers;
	public string formViewFilterPing;
	public bool formViewFilterStarted = true;
	public bool formViewFilterNotStarted = true;
	public bool formGameFilterFullGamesYes = true;
	
	// --- Messages	
	public string viewErrorWinMessage;
	public bool errorWinMessage;
	public string viewSearchMessage;
	
	/************ MULTIPLAYER JOIN BY IP *********************/
	// --- Text
	
	public string joinMessage;
	public bool viewJoinMessage;
	// --- Form fields	 
	public string formJoinIp;
	public string formJoinPort;

	/***************** MULTIPLAYER  CREATE GAMES *************************/
	// ---Text	
	public string createMessage;
	public bool viewCreateMessage;	
	public int createPlayersKey=0;
	
	// --- Form fields	
	public string formCreateName;
	public string formCreatePort;
	public int[] formCreatePlayers;	
	public int formCreateTypeButton;	

	private float sizeY;
	
	void Awake(){
		m = this.GetComponent<MMenu>();		
	}//Awake
	
	void Start(){

        if (GameManager.Instance.cPlayer != null)
            defaultPlayerName = GameManager.Instance.cPlayer.name;

		DefineFields();	// Define form fields	
		DefinePlayerData();// Fill player Data
		StartCoroutine(GetPublicAddress());	// Search the public IP 		
		if(useNetworkGames){	
			try {
				networkIPList = MLIpList.GetComputers();		
			} catch (Win32Exception ex) {
				// If it failed : show the Windows error message
				errorWinMessage = true;
				viewErrorWinMessage = m.text.mmViewFailed+ex.Message;
			}				
			gameList = new List<MGame>();
			searchList = null;
			filterList = null;
			// Search if network computers have open games
			GetNetworkGames(false);				
		}
	}//Start
	
		
	/****************** GUI DISPLAY FUNCTION **********************/
	// Call from the ONGui function of MMenu.cs
	
	
	// DisplayMenu : manage the display of all the MLMenu	
	public void DisplayMenu(){
		if(!m.displayMainMenu){			
			if(m.displayProfilLan){ // If we clicked on "Profil" button
				subMenuMultiButton = 0;
				viewErrorMessage=null;
				viewCreateMessage=false;
				EmtpyCreateForm();
				DisplayProfil(); // Display profil				
			} else if(m.displayGameLan){ // If we clicked on "Multiplayer" button
				viewProfilMessage=false;			
				DisplayMultiPlayer();// Display multiplayer			
			} 
		}
	}//DisplayMenu
	
		
	// Dispaly the player profil
	private void DisplayProfil(){	
			GUI.BeginGroup(m.menuPage, "");
			GUI.Box(new Rect(0,0,m.menuPageSizeX, m.menuPageSizeY), m.text.mmProfilLanTitle);
			sizeY = m.text.space;	
			sizeY+= m.text.space;	
			GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmProfilNameTxt);
			formProfilName = GUI.TextField(new Rect(m.text.formMargin,sizeY,m.text.fieldSizeX,m.text.fieldSizeY), formProfilName);
			sizeY+= m.text.space;
							
			if(GUI.Button(new Rect(m.text.formMargin, sizeY, m.text.buttonSizeX, m.text.buttonSizeY), m.text.mlmProfilSave)) {
				if(CheckProfil(formProfilName)){	
					SaveProfile(formProfilName);;				
				}
			} 		
			
			if(viewProfilMessage){
				sizeY+= m.text.space;
				GUI.Label(new Rect(m.text.margin,sizeY, m.menuPageSizeX,200), profilMessage);
			}	
		GUI.EndGroup();			
	}//DisplayProfil
	
	private void DisplayMultiPlayer(){
		GUI.BeginGroup(m.menuPage, "");
		GUI.Box(new Rect(0,0,m.menuPageSizeX, m.menuPageSizeY), m.text.mmMutliLanTitle);
		sizeY = m.text.space;	
		subMenuMultiButton = GUI.SelectionGrid(new Rect(m.text.margin,sizeY,m.text.buttonSubMenuSizeX*subMenuMultiButtons.Length,m.text.buttonSubMenuSizeY), subMenuMultiButton, subMenuMultiButtons, subMenuMultiButtons.Length);
			sizeY+= m.text.space;
		if(ViewGameList()){
			EmtpyCreateForm();
			EmtpyJoinIpForm();
			sizeY+= m.text.margin;	
			DisplayGameList();
		} else if(ViewJoinIp()){
			viewErrorMessage =null;
			EmtpyCreateForm();
			sizeY+= m.text.space;
			DisplayJoinIp();
		} else if(ViewCreateGame()){
			viewErrorMessage =null;
			EmtpyJoinIpForm();
			sizeY+= m.text.space;
			DisplayCreateGame();
		}
		GUI.EndGroup();	
	}//DisplayMultiPlayer
	
	private void DisplayGameList(){
		List<MGame> viewList = DisplayGameListSettings(); // Define settings and list
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.menuPageSizeX,200), viewMessage);
		sizeY+= m.text.space;
				
		// If the game list is not null and not empty	
		if(gameList != null && gameList.Count > 0){		
			int pannelSizeY = (m.menuPageSizeY-(int)sizeY-m.text.listRefreshSizeY-(m.text.margin*2));
			int sizeX = m.text.margin;		
			int contentX = m.menuPageSizeX-m.text.margin*2;
			if(m.parameters.useFilters){
				sizeX+= m.text.margin+m.text.gameListFiltersSizeX;
				contentX-= m.text.margin+ m.text.gameListFiltersSizeX;
			}
			GUI.Box(new Rect(sizeX,sizeY, contentX, pannelSizeY), "");								
			sizeX = m.text.margin*2;
			if(m.parameters.useFilters){
				DisplayGameListFilters(sizeY, pannelSizeY);
				sizeX = m.text.margin*3+m.text.gameListFiltersSizeX;		
			} 
			if((viewList != null && viewList.Count > 0)){// If the list to display is not null	
				sizeY+=m.text.margin;								
				/********** Table titles ********/
				if(GUI.Button(new Rect(sizeX,sizeY,m.text.gameListGameNameSizeX,20), m.text.mmViewGameNameTxt)){
					viewList = SortGames(viewList, 0);
				}
				sizeX+=m.text.gameListGameNameSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY,m.text.gameListHostSizeX,20), m.text.mmViewHostTxt)){
					viewList = SortGames(viewList, 1);
				}
				sizeX+=m.text.gameListHostSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY,m.text.gameListMapSizeX,20), m.text.mmViewMapTxt)){
					viewList = SortGames(viewList, 2);
				}
				sizeX+=m.text.gameListMapSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY,m.text.gameListPlayerSizeX,20), m.text.mmViewPlayerTxt)){
					viewList = SortGames(viewList, 3);
				}
				sizeX+=m.text.gameListPlayerSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY, m.text.gameListTypeSizeX,20), m.text.mmViewTypeTxt)){
					viewList = SortGames(viewList, 4);
				}			
				sizeX+=m.text.gameListTypeSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY, m.text.gameListStatusSizeX,20), m.text.mmViewStatusTxt)){
					viewList = SortGames(viewList, 5);
				}
				sizeX+=m.text.gameListStatusSizeX+5;
				if(GUI.Button(new Rect(sizeX,sizeY, m.text.gameListPingSizeX,20), m.text.mmViewPingTxt)){
					viewList = SortGames(viewList, 6);
				}		
				/**********************************/
				viewScroll = GUI.BeginScrollView (new Rect (0,sizeY+m.text.listFieldSizeY, m.menuPageSizeX-10, pannelSizeY-m.text.listFieldSizeY-20), viewScroll, new Rect(0,0, m.menuPageSizeX-40, (32*viewList.Count)));
				sizeY=0;
				// Loop the gameList to display all the games
				for(int i = 0; i < viewList.Count; i++){
					string gameStatus=m.text.mmViewStatusWait;
					if(viewList[i].isStarted){
						gameStatus = m.text.mmViewStatusGame;
					} 
					string gameType=m.text.mmViewTypePublic;
					if(viewList[i].isPrivate){
						gameType = m.text.mmViewTypePrivate;
					}
					sizeX = m.text.margin*2;
					contentX = m.menuPageSizeX-(m.text.margin+sizeX);
					int buttonX = m.menuPageSizeX-m.text.gameListJoinButtonX-m.text.margin*3;
					if(m.parameters.useFilters){
						sizeX+= m.text.margin + m.text.gameListFiltersSizeX;
						contentX = m.menuPageSizeX-(m.text.margin*2+sizeX);
					}
					GUI.Box(new Rect(sizeX,sizeY,contentX, m.text.listPanelSizeY), "");			
					sizeX+= m.text.margin;
					sizeY+=m.text.listPanelSizeY/1.5f/10;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListGameNameSizeX,m.text.listFieldSizeY), viewList[i].name);
					sizeX+=m.text.gameListGameNameSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListHostSizeX,m.text.listFieldSizeY), viewList[i].hostName);
					sizeX+=m.text.gameListHostSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListMapSizeX,m.text.listFieldSizeY), viewList[i].mapName);
					sizeX+=m.text.gameListMapSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListPlayerSizeX,m.text.listFieldSizeY),viewList[i].totalPlayer+"/"+gameList[i].maxPlayer);
					sizeX+=m.text.gameListPlayerSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListTypeSizeX,m.text.listFieldSizeY), gameType);				
					sizeX+=m.text.gameListTypeSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListStatusSizeX,m.text.listFieldSizeY), gameStatus);
					sizeX+=m.text.gameListStatusSizeX+5;
					GUI.Label(new Rect(sizeX,sizeY, m.text.gameListPingSizeX,m.text.listFieldSizeY), viewList[i].ping.ToString()+" ms");
					
					if(viewList[i].totalPlayer < viewList[i].maxPlayer){
						if(GUI.Button(new Rect(buttonX, sizeY+(m.text.listPanelSizeY/1.5f/10), m.text.gameListJoinButtonX, m.text.buttonSizeY), m.text.mmViewJoinButton)){
							JoinNetwork(viewList[i].hostPrivateIp, viewList[i].port);			
						}
					}
					sizeY+= m.text.listPanelSizeY;	
				}
				GUI.EndScrollView();
				sizeY = m.menuPageSizeY-m.text.listRefreshSizeY-m.text.margin;
			}
		}
		sizeY= m.menuPageSizeY-m.text.listRefreshSizeY-m.text.margin;
		GUI.Box(new Rect(m.text.margin,sizeY,m.menuPageSizeX-(m.text.margin*2), m.text.listRefreshSizeY), "");	
		sizeY+= m.text.margin;
		if(gameList != null && gameList.Count > 0){
			GUI.Label(new Rect(m.text.margin*2, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmViewSearchTxt);
			formViewSearch = GUI.TextField(new Rect(70+m.text.margin, sizeY,m.text.fieldSizeX, m.text.fieldSizeY), formViewSearch);
			if(GUI.Button(new Rect(70+m.text.fieldSizeX+m.text.margin*2, sizeY, 50, m.text.buttonSizeY),m.text.mmViewSearchButton)){
				SearchGame();
			}
		}		
		int formViewPortSizeX = 50;
		GUI.Label(new Rect(m.menuPageSizeX-(m.text.margin*2)-m.text.listRefreshSizeX-formViewPortSizeX-m.text.labelSizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmViewPortTxt);
		formViewPort = GUI.TextField(new Rect(m.menuPageSizeX-(m.text.margin*3)-m.text.listRefreshSizeX-formViewPortSizeX,sizeY,formViewPortSizeX,m.text.labelSizeY), formViewPort);		
		
		if(GUI.Button(new Rect(m.menuPageSizeX-(m.text.margin*2)-m.text.listRefreshSizeX, sizeY, m.text.listRefreshSizeX, m.text.buttonSizeY), m.text.mmViewRefreshButton)){
			GetNetworkGames(true);		
			viewMessage = m.text.mmViewSearch;			
		}
	}//DisplayGameList
	
	private void DisplayGameListFilters(float sizeY, int pannelSizeY){
		GUI.Box(new Rect(m.text.margin,sizeY, m.text.gameListFiltersSizeX, pannelSizeY), m.text.mmFilterTilte);			
		sizeY+= m.text.margin*2;
		int sizeX = m.text.margin*2;
		int scrollContentY = m.text.margin + m.text.margin*13 + 4*12 + formViewFilterMap.Length*(m.text.margin*2) + formViewFilterPlayers.Length*(m.text.margin*2);
		if(m.parameters.viewFullGames){
			scrollContentY+= m.text.margin*5;
		}
		viewFilterScroll = GUI.BeginScrollView (new Rect (0,sizeY, m.text.gameListFiltersSizeX+m.text.margin, pannelSizeY-20), viewFilterScroll, new Rect(0,0, m.text.gameListFiltersSizeX-m.text.margin, scrollContentY));
		sizeY= m.text.margin;
		GUI.Label(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmFilterMap);
		for(int i = 0; i < m.parameters.maps.Length; i++){
			sizeY+= m.text.margin*2;
			formViewFilterMap[i] = GUI.Toggle(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), formViewFilterMap[i], m.parameters.maps[i]);
		}		
		sizeY+= m.text.margin*3;
		GUI.Label(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmFilterPing);
		sizeY+= m.text.margin*2;
		formViewFilterPing = GUI.TextField(new Rect(sizeX, sizeY, 50, m.text.fieldSizeY), formViewFilterPing);				
		sizeY+= m.text.margin*3;
		GUI.Label(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmFilterStarted);
		sizeY+= m.text.margin*2;
		formViewFilterNotStarted = GUI.Toggle(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), formViewFilterNotStarted, m.text.mmFilterNo);
		sizeY+= m.text.margin*2;
		formViewFilterStarted = GUI.Toggle(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), formViewFilterStarted, m.text.mmFilterYes);
		sizeY+= m.text.margin*3;
		if(m.parameters.viewFullGames){
			GUI.Label(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmFilterFullGames);
			sizeY+= m.text.margin*2;			
			formGameFilterFullGamesYes = GUI.Toggle(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), formGameFilterFullGamesYes, m.text.mmFilterYes);
			sizeY+= m.text.margin*3;
		}
		GUI.Label(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), m.text.mmFilterPlayers);
		for(int i = 0; i < m.parameters.playerOptions.Length; i++){
			sizeY+= m.text.margin*2;
			formViewFilterPlayers[i] = GUI.Toggle(new Rect(sizeX, sizeY, m.text.labelSizeX, m.text.labelSizeY), formViewFilterPlayers[i], m.parameters.playerOptions[i].ToString());
		}	
		GUI.EndScrollView();
		if(GUI.changed){
			FilterGames(formViewFilterMap, formViewFilterPing, formViewFilterNotStarted, formViewFilterStarted, formViewFilterPlayers, formGameFilterFullGamesYes);
		}
	}//DisplayGameListFilters
	
	// Define the message and the gameList to display
	private List<MGame> DisplayGameListSettings(){
		// Define the message display on the panel
		if(errorWinMessage){
			viewMessage = viewErrorWinMessage;
		} else if(searchingGame){
			viewMessage = m.text.mmViewSearch;
		} else if(searchList != null){
			if(searchList.Count > 1){
				viewMessage = searchList.Count.ToString()+m.text.mmViewSearchMany;
			} else if(searchList.Count == 1){
				viewMessage = m.text.mmViewSearchOne;
			} else if(viewSearchMessage != null){
				viewMessage = viewSearchMessage;			
			}			
		} else if(filterList != null){
			if(filterList.Count > 1){
				viewMessage = filterList.Count.ToString()+m.text.mmViewManyGame;
			
			} else if(filterList.Count == 1){
				viewMessage = m.text.mmViewOneGame;
			} else {
				viewMessage =  m.text.mmViewSearchNoMatch;			
			} 			
		} else if(gameList.Count > 0){
			if(gameList.Count > 1){
				viewMessage = gameList.Count.ToString()+m.text.mmViewManyGame;
			}else{
				viewMessage = m.text.mmViewOneGame;
			}
		} else {
			viewMessage = m.text.mmViewNoGame;
		}		
		
		// If searchList is not null = if we have done a search : display searchList
		if(searchList != null){
			return searchList;
			// Else if filterList is not null = if we have done a search : display filterList
		} else if(filterList != null){
			return filterList;
			// Else display general list
		}else {
			return gameList;				
		}
	}//DisplayGameListSettings

	
	private void DisplayJoinIp(){
		if(m.networkJoinMessage[1] != null && m.networkJoinMessage[0].Equals("False")){
			viewJoinMessage = true;
			joinMessage = m.networkJoinMessage[1];
			m.networkJoinMessage[0] = null;
			m.networkJoinMessage[1] = null;			
		}
						
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmJoinIpTxt);
		formJoinIp = GUI.TextField(new Rect(m.text.formMargin,sizeY,m.text.fieldSizeX,m.text.fieldSizeY), formJoinIp);
		sizeY+= m.text.space;		
				
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmJoinPortTxt);
		formJoinPort = GUI.TextField(new Rect(m.text.formMargin,sizeY,m.text.fieldSizeX,m.text.fieldSizeY), formJoinPort);
		sizeY+= m.text.space;								
				
		if(GUI.Button (new Rect(m.text.formMargin,sizeY, m.text.buttonSizeX,m.text.buttonSizeY), m.text.mlmJoinButton)) {
			if(CheckJoinIp(formJoinIp, formJoinPort)){	
				JoinNetwork(formJoinIp, int.Parse(formJoinPort));
			}
		} 
		if(viewJoinMessage) {
			sizeY+= m.text.space;		
			sizeY+= m.text.space;		
			GUI.Label(new Rect(m.text.internalMargin,sizeY,m.menuPageSizeX,200), joinMessage);	
		}

	}//DisplayJoinIp
	
	private void DisplayCreateGame(){		
		string createTypeMessage = "";
		if(formCreateTypeButton == 0){
			createTypeMessage = m.text.mlmCreatePrivateGameMessage;
		} else {
			createTypeMessage = m.text.mlmCreatePublicGameMessage;
		}
		
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmCreateNameTxt);
		formCreateName = GUI.TextField(new Rect(m.text.formMargin,sizeY,m.text.fieldSizeX,m.text.fieldSizeY), formCreateName);
		sizeY+= m.text.space;		
			
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmCreateTypeTxt);
		formCreateTypeButton = GUI.SelectionGrid(new Rect(m.text.formMargin,sizeY,m.text.buttonSelectSizeX*m.text.mlmFormCreateTypeButtons.Length,m.text.buttonSelectSizeY), formCreateTypeButton, m.text.mlmFormCreateTypeButtons, 2);
		sizeY+= m.text.space;
		
		GUI.Label(new Rect(m.text.formMargin,sizeY,400,m.text.labelSizeY),  createTypeMessage);
		sizeY+= m.text.space;			
		
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmCreatePrivateIpTxt);
		GUI.Label(new Rect(m.text.formMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY),  m.playerDataSrc.privateIP);
		sizeY+= m.text.space;		
				
		if(m.playerDataSrc.publicIP != "" && m.playerDataSrc.publicIP != null) {
			GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY),  m.text.mlmCreatePublicIpTxt);
			GUI.Label(new Rect(m.text.formMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY),  m.playerDataSrc.publicIP);
			sizeY+= m.text.space;
		}
		
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmCreatePortTxt);			
		formCreatePort = GUI.TextField(new Rect(m.text.formMargin,sizeY,m.text.fieldSizeX,m.text.fieldSizeY), formCreatePort);
		sizeY+= m.text.space;		
		
		GUI.Label(new Rect(m.text.internalMargin,sizeY, m.text.labelSizeX,m.text.labelSizeY), m.text.mlmCreatePlayersTxt);	
		if(GUI.RepeatButton(new Rect(m.text.formMargin,sizeY, 25,20), "<<")){			
			formCreatePlayers = m.parameters.MorePlayers(formCreatePlayers);
		}
		if(GUI.Button(new Rect(m.text.formMargin+27,sizeY,20,20), "<")){			
			formCreatePlayers = m.parameters.MorePlayers(formCreatePlayers);		
		}		
		
		GUI.Label(new Rect(m.text.formMargin+25+20+10,sizeY,m.text.labelSizeX,m.text.labelSizeY), formCreatePlayers[0].ToString());		
		
		if(GUI.Button(new Rect(m.text.formMargin+25+20+10+30,sizeY, 20,20), ">")){			
			formCreatePlayers = m.parameters.LessPlayers(formCreatePlayers);	
		}
		if(GUI.RepeatButton(new Rect(m.text.formMargin+25+20+10+30+22,sizeY, 25,20), ">>")){			
			formCreatePlayers = m.parameters.LessPlayers(formCreatePlayers);	
		}
		
		sizeY+= m.text.space;
		sizeY+= m.text.space;
		if(GUI.Button(new Rect(m.text.formMargin,sizeY, m.text.buttonSizeX,m.text.buttonSizeY), m.text.mlmCreateButton)) {					
			if(CheckCreate(formCreateName, formCreatePort)){		
				StartNetwork(formCreateName, formCreatePlayers[0],int.Parse(formCreatePort),formCreateTypeButton==1);
				EmtpyCreateForm();
			}
		}
			
		if(m.networkCreateMessage[1] != null && m.networkCreateMessage[1] != "" && m.networkCreateMessage[0].Equals("False")){
			viewCreateMessage = true;
			createMessage = m.networkCreateMessage[1];
			m.networkCreateMessage[0] = null;
			m.networkCreateMessage[1] = null;			
		}
		if(viewCreateMessage){
			sizeY+= m.text.space;
			GUI.Label(new Rect(m.text.internalMargin,sizeY, m.menuPageSizeX, m.menuPageSizeY), createMessage);
		}
	}//DisplayCreateGame
	
	private void StartNetwork(string name, int playerNbr, int port,  bool isPrivate){
		InstantiateNetwork();
		m.networkSrc.StartServer(0, name, playerNbr, port, false, null, null, null, isPrivate, false, true, false);
		
	}//StartNetwork	
	
	private void JoinNetwork(string ip, int port){
		InstantiateNetwork();	
		m.networkSrc.JoinServer(ip, port, false, null, true, false);
	}//JoinNetwork
	
	// Instantiate MNetwork gameobject and define some parameters
	private void InstantiateNetwork(){
		if(m.networkObj != null) { // If we have already a network gameObject
			Destroy(m.networkObj); // Destroy it
		}		
		m.networkObj =  Instantiate(Resources.Load("MNetworkManager")) as GameObject; 
		m.networkSrc = m.networkObj.GetComponent<MNetwork>();	
		m.playerDataSrc.isLan = true;	
		m.playerDataSrc.isOnline = false;	
		m.playerDataSrc.nameInGame = m.playerDataSrc.playerName;
	}//InstantiateNetwork
	
	private bool ViewGameList(){
		if((subMenuMultiButton == 0 && subMenuMultiButtons[0].Equals(m.text.mlmMultiButtonsOptions[0]))
			|| (subMenuMultiButton == 1 && subMenuMultiButtons[1].Equals(m.text.mlmMultiButtonsOptions[0]))){	
			return true;
		}
		return false;
	}//ViewGameList
	
	private bool ViewJoinIp(){
		 if((subMenuMultiButton == 0 && subMenuMultiButtons[0].Equals(m.text.mlmMultiButtonsOptions[1]))
			|| (subMenuMultiButton == 1 && subMenuMultiButtons[1].Equals(m.text.mlmMultiButtonsOptions[1]))){		
			return true;
		}
		return false;
	}//ViewJoinIp
	
	private bool ViewCreateGame(){
		if((subMenuMultiButton == 1 && subMenuMultiButtons[1].Equals(m.text.mlmMultiButtonsOptions[2]))
			|| (subMenuMultiButton == 2 && subMenuMultiButtons[2].Equals(m.text.mlmMultiButtonsOptions[2]))){
			return true;
		}
		return false;
	}// ViewCreateGame
	
	
	/******************** SAVE FUNCTIONS *****************/
	//Check the forms before send it anywhere (call from the different Display functions)	
	private void SaveProfile(string name){
		m.playerDataSrc.playerName = name;
		if(!m.useOnline){
			m.playerDataSrc.nameInGame = name;
		}
		PlayerPrefs.SetString("playerProfil", name);		
	}//SaveProfile
	
	private void EmtpyCreateForm(){
		formCreateName="";
		formCreatePlayers[0] = m.parameters.minPlayers;
		formCreatePlayers[1] = 0;
		formCreatePort = m.parameters.defaultPort.ToString();
		createMessage=null;
		viewCreateMessage = false;
	}//EmtpyCreateForm
	
	private void EmtpyJoinIpForm(){
		m.networkJoinMessage[0] = null;
		m.networkJoinMessage[1] = null;		
		formJoinIp="";
		formJoinPort=m.parameters.defaultPort.ToString();
		viewJoinMessage = false;
		joinMessage=null;
	}//EmtpyJoinIpForm
	
	/******************** CHECK FUNCTIONS *****************/
	//Check the forms before send it anywhere (call from Display functions)	
	
	// Check profil form 
	private bool CheckProfil(string name){
		profilMessage=null;
		viewProfilMessage=false;
		if(!MFilter.CheckName(name)){		
			viewProfilMessage = true;
			profilMessage = m.text.mlmProfilFailed;
			if(MFilter.error.Equals("empty")){
				profilMessage+= m.text.mmErrorIncompleteFields;
			} else {
				profilMessage+= m.text.mlmErrorName;
			}
			return false;
		}	
		return true;
	}//CheckProfil
	
	// Check create game form
	private bool CheckCreate(string name, string port){		
		createMessage=null;
		viewCreateMessage=false;
		if(!MFilter.CheckName(name)){
			viewCreateMessage = true;
			createMessage = m.text.mlmErrorCreateFailed;
			if(MFilter.error.Equals("empty")){
				createMessage+= m.text.mmErrorIncompleteFields;
			} else {
				createMessage+= m.text.mlmErrorCreateName;
			}
			return false;
		}
		if(!MFilter.CheckNumber(port)){
			viewCreateMessage = true;
			createMessage = m.text.mlmErrorCreateFailed;
			if(MFilter.error.Equals("empty")){
				createMessage+= m.text.mmErrorIncompleteFields;
			} else {
				createMessage+= m.text.mlmErrorPort;
			}
			return false;
		}		
		return true;
	}//CheckCreate
	
	// Check join from IP form
	private bool CheckJoinIp(string ip, string port){
		viewJoinMessage = false;	
		joinMessage=null;
		bool noError=true;		
		if(!MFilter.CheckNumber(port)){
			if(MFilter.error.Equals("empty")){
				joinMessage+= m.text.mmErrorIncompleteFields;
			} else {
				joinMessage+= m.text.mlmErrorPort;
			}
			noError= false;
		}
		
		try{
			IPAddress.Parse(ip);
		} catch(ArgumentNullException){
			joinMessage+= m.text.mmErrorIncompleteFields;
			noError= false;
		} catch(FormatException){
			joinMessage+= m.text.mlmErrorJoinIp;
			noError= false;
		}
		if(noError){			
			return true;
		}else{
			viewJoinMessage=true;
			joinMessage = m.text.mlmErrorJoinFailed+joinMessage;
			return false;
		}
		
	}//CheckJoinIp
	
	/******************** PUBLIC FUNCTIONS **************************/
	// Browse the differents IP of the network to see if they have an open game
	public void GetNetworkGames(bool refresh){
		if(networkIPList == null){ // If the network ip list is null : stop the function
			return;
		}else {	// Else : 
			if(refresh) { // If we have refreshed
				searchNextIpKey = 0; // Put the IP key on zero
				gameList.Clear(); // Clear the game list
			}
			
			searchingGame = true; // Put searchParties on true
			string previousIp = null; // Set the variable previousIp 
			string currentIp = null; // Set the variable currentIp 			
			
			if(searchNextIpKey > 0 ) { 
				previousIp = networkIPList[searchNextIpKey-1]; // Define the previous IP
			} 
			if(searchNextIpKey < networkIPList.Count) { // If the current IP key is valid (shorter as networkIp length)
				currentIp = networkIPList[searchNextIpKey]; // Define the current IP
			} else { // Else : put searchParties on false 
				searchingGame = false;
			}
			
			if(m.networkObj != null) { // If we have already a network gameObject
				Destroy(m.networkObj); // Destroy it
			}
			
			//If the network PeerType is not disconnected 
			// = we have succeeded to connect us on a game on the previous IP
			if(Network.peerType != NetworkPeerType.Disconnected && previousIp != null){				
				// If the game is joinable :
				if(m.parameters.canJoinStartedGame || (!m.parameters.canJoinStartedGame && !m.networkSrc.gameInfo.isStarted)){
					
					// If we have obtained the game informations
					if(m.networkSrc.gameInfo.name != null && m.networkSrc.gameInfo.name != ""){
						
						// ------ Check that I have not been exculded of this game -------
						bool isExcluded = false;
						string hostIp = m.networkSrc.gameInfo.hostPublicIp;
						if(m.networkSrc.gameInfo.hostPublicIp == m.playerDataSrc.publicIP){
							hostIp = m.networkSrc.gameInfo.hostPrivateIp;
						}
						for(int i=0; i < m.playerDataSrc.exclusions.Count; i++){
							if(hostIp == m.playerDataSrc.exclusions[i].hostPrivateIp && int.Parse(formViewPort) == m.playerDataSrc.exclusions[i].port){
								isExcluded=true;
							}
						}
						//-----------------------------------------------------------------						
						// If I have not been excluded of this game :
						if(!isExcluded && (m.parameters.viewFullGames || m.networkSrc.gameInfo.totalPlayer < m.networkSrc.gameInfo.maxPlayer)){
							// Save the game on the GameList
							m.networkSrc.gameInfo.SetPing(Network.GetAveragePing(Network.connections[m.networkSrc.gameInfo.hostId]));
							gameList.Add(m.networkSrc.gameInfo);				
						}			
					}
				}				
				Network.Disconnect(); // Disconnect us to this game
			}				
			
			if(currentIp != null) {	// If current IP is not null 
				SearchGameOnIp(currentIp); // Search game on this IP
				searchNextIpKey++; // Increment the IP key
			}
		}				
	}//GetNetworkGames 
	
	// Instantiate the server by the networkPrefab object to search a party
	private void SearchGameOnIp(string serverIp){	
		// Define the port where we will try to connect
		if(formViewPort == ""){
			formViewPort = m.parameters.defaultPort.ToString();
		}
		
		try{
			int serverPort = int.Parse(formViewPort);			
			// Instantiate the network prefab which will make us connect on the IP
			m.networkObj =  Instantiate(Resources.Load("MNetworkManager")) as GameObject; 
			m.networkSrc = m.networkObj.GetComponent<MNetwork>();	
			m.networkSrc.SearchGame(serverPort, serverIp);		
		} catch(FormatException){
			viewErrorMessage = m.text.mlmErrorPort;
			return;
		}
	}//SearchGameOnIp
	
	/******************** PRIVATE FUNCTIONS **************************/
	// Private functions, called only by the current class	
		
	private void DefineFields(){
		// Fill the port fields with the default value
		formCreatePort = m.parameters.defaultPort.ToString();
		formJoinPort = m.parameters.defaultPort.ToString();
		formViewPort = m.parameters.defaultPort.ToString();	
		formCreatePlayers = new int[]{m.parameters.minPlayers, 0};
		formViewFilterStarted = true;
		formViewFilterNotStarted = true;
		formViewFilterMap = new bool[m.parameters.maps.Length];
		for(int i = 0; i < formViewFilterMap.Length; i++){
			formViewFilterMap[i] = true;			
		}
		formViewFilterPlayers = new bool[m.parameters.playerOptions.Length];
		for(int i = 0; i < formViewFilterPlayers.Length; i++){
			formViewFilterPlayers[i] = true;			
		}
		formViewFilterPing = "100";
		formProfilName = PlayerPrefs.GetString("playerProfil");	// Search if we have saved playerName
		if(formProfilName == "" || formProfilName == null){
			// Else : fill the name field with the default playerName
			formProfilName = defaultPlayerName;
		}
			
		// Define the multiplayer buttons
		if(!useJoinIP && useNetworkGames){
			subMenuMultiButtons = new string[2]{m.text.mlmMultiButtonsOptions[0], m.text.mlmMultiButtonsOptions[2]};		
		// If we don't use the menu "View network games"
		} else if(useJoinIP && !useNetworkGames){		
			subMenuMultiButtons = new string[2]{m.text.mlmMultiButtonsOptions[1], m.text.mlmMultiButtonsOptions[2]};		
		// If we don't use the menus "Join game from IP" and  "View network games"
		}else if(!useJoinIP && !useNetworkGames){		
			subMenuMultiButtons = new string[1]{m.text.mlmMultiButtonsOptions[2]};	
		} else {
			subMenuMultiButtons = new string[3]{m.text.mlmMultiButtonsOptions[0], m.text.mlmMultiButtonsOptions[1], m.text.mlmMultiButtonsOptions[2]};	
		}	
	}//DefineFields
	
	private void DefinePlayerData(){	
		// Fill m.playerDataSrc with the player's data
		if(m.playerDataSrc.playerName == "" || m.playerDataSrc.playerName == null){
			m.playerDataSrc.playerName = formProfilName;
		} else {
			formProfilName = m.playerDataSrc.playerName;
		}
		if(!m.useOnline){
			m.playerDataSrc.nameInGame = m.playerDataSrc.playerName;
		}
		m.playerDataSrc.isInGame = false;
		m.playerDataSrc.privateIP = Network.player.ipAddress;				
	}//DefinePlayerData
	
	private void FilterGames(bool[] maps, string ping, bool notStarted, bool started, bool[] playerNbr, bool fullGameYes){				
		List<string> mapList = new List<string>();
		List<int> playersList = new List<int>();
		for(int i = 0; i < maps.Length; i++){
			if(maps[i]){
				mapList.Add (m.parameters.maps[i]);
			}
		}
		for(int i = 0; i < playerNbr.Length; i++){
			if(playerNbr[i]){
				playersList.Add (m.parameters.playerOptions[i]);
			}
		}
		int pingInt = 0;
		if(MFilter.CheckNumber(ping)){
			try{
				pingInt = int.Parse(ping);
			} catch{
				return;
			}
		} else{
			return;
		}
		filterList = MGame.FilterGames(gameList, playersList,mapList, pingInt, notStarted, started, fullGameYes);
		if(searchList != null){
			 SearchGame();
		} 
	}//FilterGames
	
	
	// Sort the gameList
	private List<MGame> SortGames(List<MGame> list, int sortColumn){		
		// Search if we have to do a asc or desc sort and save the current sort in variables
		if(sortColumn == this.sortColumn){
			if(this.sortType == 0){
				this.sortType = 1;
			} else {
				this.sortType = 0;
			}
		} else {
			this.sortType = 0;
		}
		this.sortColumn = sortColumn;
		return MGame.SortGames(list,this.sortColumn,this.sortType);
	}//SortGames
	
	// Search Game
	private void SearchGame(){
		List<MGame> list = new List<MGame>();
		if(filterList != null){
			list = filterList;
		} else {
			list = gameList;
		}
		
		if(formViewSearch != null && formViewSearch != ""){
			if(MFilter.CheckName(formViewSearch)){
				searchList = MGame.SearchGame(list,formViewSearch);
				if(searchList.Count <= 0){
					viewSearchMessage = m.text.mmViewSearchNoMatch;
				}
			} else {
				viewSearchMessage = m.text.mmViewSearchInvalid;
			}
		} else {
			searchList = null;
		}		
	}
	
	// Search the public ip by asking to an ip service website
	private IEnumerator GetPublicAddress(){
		// IF YOU USE MULTIONLINE
		// Search public IP from our own web hosting
		if(m.useOnline && ipOnline != null && ipOnline != ""){
			WWW www = new WWW(ipOnline);
        	yield return www;
			if(www.isDone){			
				if(www.text != "") {					
					m.playerDataSrc.publicIP = www.text;
				}
			}
		} else {
			// Search public IP from a web IP service
			WWW www = new WWW(ipService);
        	yield return www;
			if(www.isDone){			
				if(www.text != "") {					
					/*** IF YOU USE THE SAME IP SERVICE AS IN THE SAMPLE : ***/
					
					// Search the ip from the website source code
					Regex searchIP = new Regex("<span class=\"clip\">[0-9]+.[0-9]+.[0-9]+.[0-9]+</span>");    
					Match match = searchIP.Match(www.text);
					if(match.Success){
						searchIP = new Regex("[0-9]+.[0-9]+.[0-9]+.[0-9]+");    
						Match match2 = searchIP.Match(match.Value);					
						if(match2.Success){
							m.playerDataSrc.publicIP = match2.Value;
						}
					}	
				}			
				/*** IF YOU HAVE CREATE YOUR OWN IP SERVICE 
				 * with the simpe PHP script <?php echo $_SERVER['REMOTE_ADDR'] ?>, use just the follow line  
				 and delete or put on comments the lines  571 to 579 * ***/
				// m.playerDataSrc.publicIP = www.text;
				
				/*** IF YOU USE AN OTHER IP SERVICE : 
				 * you'll have to do your own function depending the source code of the service you use***/
			}  
		} 	
	}//GetPublicAddress
}//MLMenu

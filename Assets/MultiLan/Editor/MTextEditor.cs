using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MText))]
public class MTextEditor : Editor {
	private MText edit;	
	private static string openTxt="--- OPEN ---";
	private static string closeTxt="--- CLOSE ---";
	

	private string showMMenuTxt=openTxt;
	public bool showMMenu;
	public bool showMMenuB;
	public bool showMMenuG;
	public bool showMMenuM;
	public bool showMMenuT;
	
	/*private string showMOMenuTxt=openTxt;
	public bool showMOMenu;
	public bool showMOMenuL;
	public bool showMOMenuP;
	public bool showMOMenuF;
	public bool showMOMenuB;
	public bool showMOMenuG;
	public bool showMOMenuGM;
	
	private string showDSMenuTxt=openTxt;
	public bool showDSMenu;
	public bool showDSMenuL;
	public bool showDSMenuP;
	public bool showDSMenuC;
	public bool showDSMenuG;*/
	
	private string showMLMenuTxt=openTxt;
	public bool showMLMenu;
	public bool showMLMenuP;
	public bool showMLMenuGM;
	public bool showMLMenuGIP;
	public bool showMLMenuGC;
		
	private string showWaitroomTxt=openTxt;
	public bool showWaitroom;
	public bool showWP;
	public bool showWGM;
	public bool showWC;
	public bool showWE;
		
	private string showGameMenuTxt=openTxt;
	public bool showGameMenu;
	public bool showGMP;
	public bool showGMB;
	public bool showGME;
	public bool showGMED;
	public bool showGMAF;
	public bool showGMIF;
	public bool showGMAIF;
	public bool showGMC;
	public bool showGMM;
	public bool showGMEX;
	public bool showGMGB;
	
	private string showSizeTxt=openTxt;
	public bool showSize;
	public bool showSSGM;
	public bool showSSGL;
	public bool showSSFL;
	public bool showSSBL;
	
	private string showGameNetworkTxt=openTxt;
	public bool showGameNetwork;
	
	public void OnEnable(){
		edit = (MText) target;	
	}
	
	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("TEXT MANAGER", EditorStyles.boldLabel);
		
		
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("Show main menu text", EditorStyles.boldLabel);
		

		showMMenu = EditorGUILayout.Foldout(showMMenu, showMMenuTxt);
		if(showMMenu){
			showMMenuTxt=closeTxt;
			EditorGUILayout.Space();	
			edit.mmCustomSkin = EditorGUILayout.ObjectField(edit.mmCustomSkin, typeof(GUISkin), true) as GUISkin;

			showMMenuT = EditorGUILayout.Foldout(showMMenuT, "Titles");
			if(showMMenuT){
				edit.mmProfilOnlineTitle = EditorGUILayout.TextField("Profil", edit.mmProfilOnlineTitle);
				edit.mmMutliOnlineTitle = EditorGUILayout.TextField("Online games", edit.mmMenuMultiOnlineButton);	
				edit.mmMutliLanTitle = EditorGUILayout.TextField("LAN games", edit.mmMutliLanTitle);	
			}
			
			EditorGUILayout.Space();
			showMMenuB = EditorGUILayout.Foldout(showMMenuB, "Buttons");
			if(showMMenuB){
				edit.mmMenuProfilOnlineButton = EditorGUILayout.TextField("Online profil", edit.mmMenuProfilOnlineButton);
				edit.mmMenuMultiOnlineButton = EditorGUILayout.TextField("Online games", edit.mmMenuMultiOnlineButton);
				edit.mmMenuProfilLanButton = EditorGUILayout.TextField("LAN profil", edit.mmMutliOnlineTitle);	
				edit.mmMenuMultiLanButton = EditorGUILayout.TextField("LAN games", edit.mmMenuMultiLanButton);	
				edit.mmMenuExitButton = EditorGUILayout.TextField("Exit", edit.mmMenuExitButton);	
				edit.mmMenuBackButton = EditorGUILayout.TextField("Back to menu", edit.mmMenuBackButton);
			}
			
			EditorGUILayout.Space();
			showMMenuG = EditorGUILayout.Foldout(showMMenuG, "Game list panel");
			if(showMMenuG){
				EditorGUILayout.LabelField("GAME LIST");
				EditorGUILayout.LabelField("Titles, buttons and texts", EditorStyles.miniLabel);
				edit.mmViewTilte = EditorGUILayout.TextField("Titl. view list", edit.mmViewTilte);				
				edit.mmViewSearchTxt = EditorGUILayout.TextField("Txt. Search game", edit.mmViewSearchTxt);						
				edit.mmViewPortTxt = EditorGUILayout.TextField("Txt. Port (ML)", edit.mmViewPortTxt);
				edit.mmViewRefreshButton = EditorGUILayout.TextField("Butt. refresh", edit.mmViewRefreshButton);	
				edit.mmViewSearchButton = EditorGUILayout.TextField("Butt. search", edit.mmViewSearchButton);
				
				EditorGUILayout.LabelField("Columns", EditorStyles.miniLabel);
				edit.mmViewGameNameTxt = EditorGUILayout.TextField("Col. game name", edit.mmViewGameNameTxt);	
				edit.mmViewHostTxt = EditorGUILayout.TextField("Col. host", edit.mmViewHostTxt);
				edit.mmViewHostTypeTxt = EditorGUILayout.TextField("Col. host type", edit.mmViewHostTypeTxt);	
				edit.mmViewPingTxt = EditorGUILayout.TextField("Col. ping", edit.mmViewPingTxt);	
				edit.mmViewPlayerTxt = EditorGUILayout.TextField("Col. players nbr.", edit.mmViewPlayerTxt);	
				edit.mmViewMapTxt = EditorGUILayout.TextField("Col. map", edit.mmViewMapTxt);	
				edit.mmViewTypeTxt = EditorGUILayout.TextField("Col. type", edit.mmViewTypeTxt);	
				edit.mmViewStatusTxt = EditorGUILayout.TextField("Col. Status", edit.mmViewStatusTxt);
				edit.mmViewPasswordTxt = EditorGUILayout.TextField("Col. password", edit.mmViewPasswordTxt);
					
				EditorGUILayout.LabelField("List values", EditorStyles.miniLabel);				
				edit.mmViewStatusWait = EditorGUILayout.TextField("Val. status waitroom", edit.mmViewStatusWait);	
				edit.mmViewStatusGame = EditorGUILayout.TextField("Val. status ingame", edit.mmViewStatusGame);
				edit.mmViewTypePublic = EditorGUILayout.TextField("Val. public game", edit.mmViewTypePublic);
				edit.mmViewTypePrivate = EditorGUILayout.TextField("Val. private game", edit.mmViewTypePrivate);
				edit.mmViewNoPassTxt = EditorGUILayout.TextField("Val. no password", edit.mmViewNoPassTxt);
				edit.mmViewHostPlayer = EditorGUILayout.TextField("Val. host player", edit.mmViewHostPlayer);
				edit.mmViewHostServer = EditorGUILayout.TextField("Val. host server", edit.mmViewHostServer);
				
				EditorGUILayout.LabelField("GAME FILTERS");
				edit.mmFilterTilte = EditorGUILayout.TextField("Titl. filters", edit.mmFilterTilte);
				edit.mmFilterMap = EditorGUILayout.TextField("Txt. map", edit.mmFilterMap);
				edit.mmFilterPassword = EditorGUILayout.TextField("Txt. password", edit.mmFilterPassword);
				edit.mmFilterFullGames = EditorGUILayout.TextField("Txt. full game", edit.mmFilterFullGames);
				edit.mmFilterPing = EditorGUILayout.TextField("Txt. ping", edit.mmFilterPing);
				edit.mmFilterPlayers = EditorGUILayout.TextField("Txt. player nbr.", edit.mmFilterPlayers);
				edit.mmFilterYes = EditorGUILayout.TextField("Val. YES", edit.mmFilterYes);
				edit.mmFilterNo = EditorGUILayout.TextField("Val. NO", edit.mmFilterNo);

				EditorGUILayout.LabelField("MESSAGE & ERRORS");
				edit.mmViewSearch = EditorGUILayout.TextField("Msg. searching", edit.mmViewSearch);
				edit.mmViewNoGame = EditorGUILayout.TextField("Msg. no game", edit.mmViewNoGame);
				edit.mmViewOneGame = EditorGUILayout.TextField("Msg. one game", edit.mmViewOneGame);
				edit.mmViewManyGame = EditorGUILayout.TextField("Msg. many games", edit.mmViewManyGame);
				edit.mmViewSearchNoMatch = EditorGUILayout.TextField("Msg. search no match", edit.mmViewSearchNoMatch);
				edit.mmViewSearchOne = EditorGUILayout.TextField("Msg. search one entry", edit.mmViewSearchOne);
				edit.mmViewSearchMany = EditorGUILayout.TextField("Msg. search many entries", edit.mmViewSearchMany);
				edit.mmErrorView = EditorGUILayout.TextField("Err. general msg", edit.mmErrorView);
				edit.mmViewFailed = EditorGUILayout.TextField("Err. search failed", edit.mmViewFailed);
				edit.mmViewSearchInvalid = EditorGUILayout.TextField("Err. search invalid ", edit.mmViewSearchInvalid);
				edit.mmErrorViewPassword = EditorGUILayout.TextField("Err. password error", edit.mmErrorViewPassword);
				edit.mmErrorViewEmptyPassword = EditorGUILayout.TextField("Err. empty password", edit.mmErrorViewEmptyPassword);
			}

		} else {
		 	showMMenuTxt= openTxt;
		}
		
		/*EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show MO menu text", EditorStyles.boldLabel);
		showMOMenu = EditorGUILayout.Foldout(showMOMenu, showMOMenuTxt);
		if(showMOMenu){
			showMOMenuTxt=closeTxt;
			
			EditorGUILayout.Space();				
			showMOMenuL = EditorGUILayout.Foldout(showMOMenuL, "Login/Register panel");
			if(showMOMenuL){
				EditorGUILayout.LabelField("CONTENT");				
				edit.momLoginButtons[0] = EditorGUILayout.TextField("Butt. menu login", edit.momLoginButtons[0]);				
				edit.momLoginButtons[1] = EditorGUILayout.TextField("Butt. menu register", edit.momLoginButtons[1]);
				edit.momLoginButtons[2] = EditorGUILayout.TextField("Butt. menu forgot login", edit.momLoginButtons[2]);
				edit.momLoginTxt = EditorGUILayout.TextField("Txt. login", edit.momLoginTxt);	
				edit.momLoginNameTxt = EditorGUILayout.TextField("Txt. username", edit.momLoginNameTxt);	
				edit.momLoginMailTxt = EditorGUILayout.TextField("Txt. mail", edit.momLoginMailTxt);	
				edit.momLoginPassTxt = EditorGUILayout.TextField("Txt. password", edit.momLoginPassTxt);	
				edit.momLoginPassConfTxt = EditorGUILayout.TextField("Txt. password conf.", edit.momLoginPassConfTxt);	
				edit.momLoginSaveDataTxt = EditorGUILayout.TextField("Txt. save login data", edit.momLoginSaveDataTxt);	
				edit.momLoginForgotMailTxt = EditorGUILayout.TextField("Txt. forgot login mail", edit.momLoginForgotMailTxt);
				edit.momForgotLoginNameTxt = EditorGUILayout.TextField("Txt. forgot username box", edit.momForgotLoginNameTxt);	
				edit.momForgotLoginPassTxt = EditorGUILayout.TextField("Txt. forgot password box", edit.momForgotLoginPassTxt);	
				edit.momLoginLogButton = EditorGUILayout.TextField("Butt. login", edit.momLoginLogButton);	
				edit.momRegisterButton = EditorGUILayout.TextField("Butt. register", edit.momRegisterButton);	
				edit.momForgotLoginButton = EditorGUILayout.TextField("Butt. forgot login", edit.momForgotLoginButton);					
			
				EditorGUILayout.LabelField("MESSAGE & ERRORS");		
				edit.momErrorPass = EditorGUILayout.TextField("Err. empty password", edit.momErrorPass);	
				edit.momErrorMail = EditorGUILayout.TextField("Err. empty mail", edit.momErrorMail);	
				edit.momErrorLoginPass = EditorGUILayout.TextField("Err. password", edit.momErrorLoginPass);	
				edit.momErrorLoginUserName = EditorGUILayout.TextField("Err. username", edit.momErrorLoginUserName);	
				edit.momErrorLoginAlreadyLog = EditorGUILayout.TextField("Err. user login", edit.momErrorLoginAlreadyLog);	
				edit.momErrorNameExist = EditorGUILayout.TextField("Err. username exists", edit.momErrorNameExist);	
				edit.momErrorMailExist = EditorGUILayout.TextField("Err. mail exists", edit.momErrorMailExist);	
				edit.momErrorPassConf = EditorGUILayout.TextField("Err. password conf.", edit.momErrorPassConf);	
				edit.momLoginFailed = EditorGUILayout.TextField("Err. login failed", edit.momLoginFailed);
				edit.momRegisterFailed = EditorGUILayout.TextField("Err. register failed", edit.momRegisterFailed);	
				edit.momForgotLoginFailed = EditorGUILayout.TextField("Err. send info failed", edit.momForgotLoginFailed);
				edit.momErrorForgotLoginMail = EditorGUILayout.TextField("Err. mail", edit.momErrorForgotLoginMail);		
				edit.momRegisterSuccess = EditorGUILayout.TextField("Mgs. register success", edit.momRegisterSuccess);	
				edit.momForgotLoginSuccessUserName = EditorGUILayout.TextField("Msg. login  success", edit.momForgotLoginSuccessUserName);	
				edit.momForgotLoginSuccessPass = EditorGUILayout.TextField("Msg. send info success", edit.momForgotLoginSuccessPass);							
			}			
			
			EditorGUILayout.Space();			
			showMOMenuP = EditorGUILayout.Foldout(showMOMenuP, "Profil panel");
			if(showMOMenuP){
				EditorGUILayout.LabelField("CONTENT");	
				edit.momSubMenuProfilButtons[0] = EditorGUILayout.TextField("Titl. profil page", edit.momSubMenuProfilButtons[0]);	
				edit.momProfilTitle = EditorGUILayout.TextField("Titl. profil settings", edit.momProfilTitle);	
				edit.momPasswordTilte = EditorGUILayout.TextField("Titl. change password", edit.momPasswordTilte);				
				edit.momLogoutTitle = EditorGUILayout.TextField("Titl. logout", edit.momLogoutTitle);
				edit.momProfilMailTxt = EditorGUILayout.TextField("Txt. mail", edit.momProfilMailTxt);
				edit.momProfilNameTxt = EditorGUILayout.TextField("Txt. username", edit.momProfilNameTxt);
				edit.momPasswordTxt = EditorGUILayout.TextField("Txt. pass", edit.momPasswordTxt);
				edit.momPasswordNewTxt = EditorGUILayout.TextField("Txt. new pass", edit.momPasswordNewTxt);
				edit.momPasswordNewConfTxt = EditorGUILayout.TextField("Txt.new pass conf.", edit.momPasswordNewConfTxt);
				edit.momProfilSave = EditorGUILayout.TextField("Butt. save profil", edit.momProfilSave);
				edit.momPasswordButton = EditorGUILayout.TextField("Butt. save password", edit.momPasswordButton);
				edit.momLogoutButton = EditorGUILayout.TextField("Butt. logout", edit.momLogoutButton);

				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.momErrorLoginData = EditorGUILayout.TextField("Err. login data", edit.momErrorLoginData);
				edit.momProfilFailed = EditorGUILayout.TextField("Err. change profil", edit.momProfilFailed);
				edit.momPasswordFailed = EditorGUILayout.TextField("Err. change password", edit.momPasswordFailed);
				edit.momErrorCurrentPass = EditorGUILayout.TextField("Err. current pass", edit.momErrorCurrentPass);
				edit.momErrorNewPass = EditorGUILayout.TextField("Err. new pass", edit.momErrorNewPass);
				edit.momErrorNewPassConf = EditorGUILayout.TextField("Err. new pass conf.", edit.momErrorNewPassConf);
				edit.momProfilSuccess = EditorGUILayout.TextField("Msg. change profil success", edit.momProfilSuccess);
				edit.momPasswordSuccess = EditorGUILayout.TextField("Msg. change pass success", edit.momPasswordSuccess);
			}	
			
			EditorGUILayout.Space();				
			showMOMenuF = EditorGUILayout.Foldout(showMOMenuF, "Friendlist panel");
			if(showMOMenuF){
			EditorGUILayout.LabelField("CONTENT");	
				edit.momSubMenuProfilButtons[1] = EditorGUILayout.TextField("Titl. friendlist page", edit.momSubMenuProfilButtons[1]);	
				edit.momFriendlistNameTxt = EditorGUILayout.TextField("Col. username", edit.momFriendlistNameTxt);
				edit.momFriendlistDateTxt = EditorGUILayout.TextField("Col. add date", edit.momFriendlistDateTxt);	
				edit.momFriendlistSatutTxt = EditorGUILayout.TextField("Col. status", edit.momFriendlistSatutTxt);	
				edit.momFriendlistGameTxt = EditorGUILayout.TextField("Col. game name", edit.momFriendlistGameTxt);	
				edit.momFriendlistGamePassTxt = EditorGUILayout.TextField("Col. game pass", edit.momFriendlistGamePassTxt);	
				edit.momFriendlistSatutInGame = EditorGUILayout.TextField("Val. status in game", edit.momFriendlistSatutInGame);	
				edit.momFriendlistSatutLogin = EditorGUILayout.TextField("Val. status login", edit.momFriendlistSatutLogin);	
				edit.momFriendlistSatutLogout = EditorGUILayout.TextField("Val. satus offline", edit.momFriendlistSatutLogout);	
				edit.momFriendlistEmptyField = EditorGUILayout.TextField("Val. empty field", edit.momFriendlistEmptyField);
				edit.momFriendlistJoinGameTxt = EditorGUILayout.TextField("Butt.join game", edit.momFriendlistJoinGameTxt);	
				edit.momFriendlistRemoveTxt = EditorGUILayout.TextField("Butt. remove", edit.momFriendlistRemoveTxt);
				edit.momRefreshFriendlistButton = EditorGUILayout.TextField("Butt. refresh", edit.momRefreshFriendlistButton);	
				edit.momSearchFriendlistButton = EditorGUILayout.TextField("Butt. search", edit.momSearchFriendlistButton);	
				edit.momFriendlistSearchTxt = EditorGUILayout.TextField("Txt.search", edit.momFriendlistSearchTxt);	
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.momFriendlistSearch = EditorGUILayout.TextField("Msg. searching list", edit.momFriendlistSearch);
				edit.momFriendlistMany = EditorGUILayout.TextField("Msg. many friends in list", edit.momFriendlistMany);	
				edit.momFriendlistOne = EditorGUILayout.TextField("Msg one friend in list", edit.momFriendlistOne);	
				edit.momFriendlistEmpty = EditorGUILayout.TextField("Msg. empty friendlist", edit.momFriendlistEmpty);	
				edit.momFriendlistSearchNoMatch = EditorGUILayout.TextField("Msg. no entry found", edit.momFriendlistSearchNoMatch);	
				edit.momFriendlistSearchOne = EditorGUILayout.TextField("Msg. one entry found", edit.momFriendlistSearchOne);	
				edit.momFriendlistSearchMany = EditorGUILayout.TextField("Msg. many entries found", edit.momFriendlistSearchMany);	
				edit.momFriendlistFailed = EditorGUILayout.TextField("Err. get friendlist failed", edit.momFriendlistFailed);	
				edit.momFriendlistSearchInvalid = EditorGUILayout.TextField("Err. invalid search", edit.momFriendlistSearchInvalid);	
			}	
			
			EditorGUILayout.Space();				
			showMOMenuB = EditorGUILayout.Foldout(showMOMenuB, "Blacklist panel");
			if(showMOMenuB){
				EditorGUILayout.LabelField("CONTENT");	
				edit.momSubMenuProfilButtons[2] = EditorGUILayout.TextField("Titl. blacklist page", edit.momSubMenuProfilButtons[2]);	
				edit.momBlacklistDateTxt = EditorGUILayout.TextField("Col. username", edit.momBlacklistNameTxt);
				edit.momBlacklistRemoveTxt = EditorGUILayout.TextField("Col. blacklist date", edit.momBlacklistDateTxt);
				edit.momBlacklistRemoveTxt = EditorGUILayout.TextField("Butt. Remove", edit.momBlacklistRemoveTxt);
				edit.momBlacklistSearchTxt = EditorGUILayout.TextField("Txt. seach", edit.momBlacklistSearchTxt);
				edit.momBacklistAddTxt = EditorGUILayout.TextField("Txt. add", edit.momBlacklistSearchTxt);
				edit.momBlacklistSearchButton = EditorGUILayout.TextField("Butt. search", edit.momBlacklistSearchButton);
				edit.momBacklistAddButton = EditorGUILayout.TextField("Butt. add", edit.momBacklistAddButton);
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.momBlacklistSearch = EditorGUILayout.TextField("Msg. searching blacklist", edit.momBlacklistSearch);
				edit.momBlacklistEmpty = EditorGUILayout.TextField("Msg. empty blacklist", edit.momBlacklistEmpty);
				edit.momBlacklistFailed = EditorGUILayout.TextField("Err. get blackist failed", edit.momBlacklistFailed);
				edit.momBlacklistMany = EditorGUILayout.TextField("Msg. many entries in list", edit.momBlacklistMany);
				edit.momBlacklistOne = EditorGUILayout.TextField("Msg. one entry in list", edit.momBlacklistOne);	
				edit.momBlacklistSearchOne = EditorGUILayout.TextField("Msg. one entry found", edit.momBlacklistSearchOne);
				edit.momBlacklistSearchMany = EditorGUILayout.TextField("Msg. many entries found", edit.momBlacklistSearchMany);
				edit.momBlacklistSearchNoMatch = EditorGUILayout.TextField("Msg no entry found", edit.momBlacklistSearchNoMatch);
				edit.momBlacklistAddFailed = EditorGUILayout.TextField("Err. add failed", edit.momBlacklistAddFailed);
				edit.momErrorBlacklistName = EditorGUILayout.TextField("Err. added player doesn's exist", edit.momErrorBlacklistName);
				edit.momBlacklistRemoveFailed = EditorGUILayout.TextField("Err. remove failed", edit.momBlacklistRemoveFailed);
				edit.momErrorBlacklistId = EditorGUILayout.TextField("Err. add palyer", edit.momErrorBlacklistId);				
				edit.momBlacklistSearchInvalid = EditorGUILayout.TextField("Err. invalid search", edit.momBlacklistSearchInvalid);
			}
			EditorGUILayout.Space();				
			showMOMenuGM = EditorGUILayout.Foldout(showMOMenuGM, "Multiplayer menu");
			if(showMOMenuGM){
				edit.momSubMenuMultiButtons[0] = EditorGUILayout.TextField("Titl. view games", edit.momSubMenuMultiButtons[0]);	
				edit.momSubMenuMultiButtons[1] = EditorGUILayout.TextField("Titl. create game", edit.momSubMenuMultiButtons[1]);
			}
			
			EditorGUILayout.Space();				
			showMOMenuG = EditorGUILayout.Foldout(showMOMenuG, "Create game panel");
			if(showMOMenuG){
				EditorGUILayout.LabelField("CONTENT");	
				edit.momCreateTilte= EditorGUILayout.TextField("Titl. create game", edit.momCreateTilte);
				edit.momCreateNameTxt= EditorGUILayout.TextField("Txt. game name", edit.momCreateNameTxt);
				edit.momCreatePortTxt= EditorGUILayout.TextField("Txt. port", edit.momCreatePortTxt);
				edit.momCreateInfoPortTxt= EditorGUILayout.TextField("Txt. info port", edit.momCreateInfoPortTxt);
				edit.momCreatePlayersTxt= EditorGUILayout.TextField("Txt. players nbr.", edit.momCreatePlayersTxt);
				edit.momCreateUsePassTxt= EditorGUILayout.TextField("Txt. use pass", edit.momCreateUsePassTxt);
				edit.momCreatePassTxt= EditorGUILayout.TextField("Txt. pass", edit.momCreatePassTxt);
				edit.momCreatePassConfTxt= EditorGUILayout.TextField("Txt. pass conf.", edit.momCreatePassConfTxt);
				edit.momCreateButton= EditorGUILayout.TextField("Butt. create", edit.momCreateButton);
								
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.momErrorCreateFailed= EditorGUILayout.TextField("Err. create game failed", edit.momErrorCreateFailed);
				edit.momErrorCreateName= EditorGUILayout.TextField("Err. invalid game name", edit.momErrorCreateName);
				edit.momErrorCreatePort= EditorGUILayout.TextField("Err. invalid port", edit.momErrorCreatePort);
				edit.momErrorCreatePlayers= EditorGUILayout.TextField("Err. invalid player nbr.", edit.momErrorCreatePlayers);
				edit.momErrorCreatePass= EditorGUILayout.TextField("Err. invalid pass", edit.momErrorCreatePass);
				edit.momErrorCreatePassConf= EditorGUILayout.TextField("Err. bad pass conf.", edit.momErrorCreatePassConf);
			}
			
		}else{
			showMOMenuTxt= openTxt;
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show Dedicated Server menu text", EditorStyles.boldLabel);
		showDSMenu = EditorGUILayout.Foldout(showDSMenu, showDSMenuTxt);
		if(showDSMenu){
			showDSMenuTxt=closeTxt;
			EditorGUILayout.Space();
			edit.dsTitle= EditorGUILayout.TextField("Titl. pannel ", edit.dsTitle);
			edit.dsCustomSkin = EditorGUILayout.ObjectField(edit.dsCustomSkin, typeof(GUISkin), true) as GUISkin;
			EditorGUILayout.Space();				
			showDSMenuL = EditorGUILayout.Foldout(showDSMenuL, "Login/Register panel");
			if(showDSMenuL){
				EditorGUILayout.LabelField("CONTENT");	
				edit.dsLoginButtons[0]= EditorGUILayout.TextField("Titl. login", edit.dsLoginButtons[0]);
				edit.dsLogNameTxt= EditorGUILayout.TextField("Txt. name", edit.dsLogNameTxt);
				edit.dsLogPassTxt= EditorGUILayout.TextField("Txt. pass", edit.dsLogPassTxt);
				edit.dsLogRememberTxt= EditorGUILayout.TextField("Txt. remember", edit.dsLogRememberTxt);
				edit.dsLogButton= EditorGUILayout.TextField("Butt. login", edit.dsLogButton);
				edit.dsLoginButtons[1]= EditorGUILayout.TextField("Titl. register", edit.dsLoginButtons[1]);
				edit.dsRegNameTxt= EditorGUILayout.TextField("Txt. server name", edit.dsRegNameTxt);
				edit.dsRegLocationTxt= EditorGUILayout.TextField("Txt. location", edit.dsRegLocationTxt);
				edit.dsRegMailTxt= EditorGUILayout.TextField("Txt. mail", edit.dsRegMailTxt);
				edit.dsRegPassTxt= EditorGUILayout.TextField("Txt. pass", edit.dsRegPassTxt);
				edit.dsRegPassConfTxt= EditorGUILayout.TextField("Txt. pass confirmation", edit.dsRegPassConfTxt);
				edit.dsRegButton= EditorGUILayout.TextField("Butt. register", edit.dsRegButton);
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.dsRegErr= EditorGUILayout.TextField("Err. ", edit.dsRegErr);
				edit.dsRegErrEmptyField= EditorGUILayout.TextField("Err. empty field", edit.dsRegErrEmptyField);
				edit.dsRegErrName= EditorGUILayout.TextField("Err. name", edit.dsRegErrName);
				edit.dsRegErrLoc= EditorGUILayout.TextField("Err. location", edit.dsRegErrLoc);
				edit.dsRegErrMail= EditorGUILayout.TextField("Err. mail", edit.dsRegErrMail);
				edit.dsRegErrPass= EditorGUILayout.TextField("Err. pass", edit.dsRegErrPass);
				edit.dsRegErrPassConf= EditorGUILayout.TextField("Err. pass conf", edit.dsRegErrPassConf);
				edit.dsRegErrNameExist= EditorGUILayout.TextField("Err. name exist", edit.dsRegErrNameExist);
				edit.dsLogErrPass= EditorGUILayout.TextField("Err. login pass", edit.dsLogErrPass);
				edit.dsLogErrName= EditorGUILayout.TextField("Err. login name", edit.dsLogErrName);
				edit.dsRegSuccess= EditorGUILayout.TextField("Msg. register success", edit.dsRegSuccess);	
			}			
			
			EditorGUILayout.Space();			
			showDSMenuP = EditorGUILayout.Foldout(showDSMenuP, "Server data panel");
			if(showDSMenuP){
				EditorGUILayout.LabelField("CONTENT");	
				edit.dsMenuButtons[1]= EditorGUILayout.TextField("Titl. server data", edit.dsMenuButtons[1]);
				edit.dsMenuButtons[2]= EditorGUILayout.TextField("Titl. logout", edit.dsMenuButtons[2]);
				edit.dsProTitle= EditorGUILayout.TextField("Titl. account data", edit.dsProTitle);
				edit.dsProNameTxt= EditorGUILayout.TextField("Txt. server name", edit.dsProNameTxt);
				edit.dsProLocationTxt= EditorGUILayout.TextField("Txt. location", edit.dsProLocationTxt);
				edit.dsProMailTxt= EditorGUILayout.TextField("Txt. mail", edit.dsProMailTxt);
				edit.dsProChangeButton= EditorGUILayout.TextField("Butt. save", edit.dsProChangeButton);
				edit.dsProPassTitle= EditorGUILayout.TextField("Titl. change pass", edit.dsProPassTitle);
				edit.dsProPassCurrentTxt= EditorGUILayout.TextField("Txt. current pass", edit.dsProPassCurrentTxt);
				edit.dsProPassNewTxt= EditorGUILayout.TextField("Txt. new pass", edit.dsProPassNewTxt);
				edit.dsProPassNewConfTxt= EditorGUILayout.TextField("Txt. new pass conf.", edit.dsProPassNewConfTxt);
				edit.dsProPassChangeTxt= EditorGUILayout.TextField("Butt. save pass", edit.dsProPassChangeTxt);
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.dsProErr= EditorGUILayout.TextField("Err. ", edit.dsProErr);
				edit.dsProErrEmptyField= EditorGUILayout.TextField("Err. empty field", edit.dsProErrEmptyField);
				edit.dsProErrName= EditorGUILayout.TextField("Err. name", edit.dsProErrName);
				edit.dsProErrLoc= EditorGUILayout.TextField("Err. location", edit.dsProErrLoc);
				edit.dsProErrMail= EditorGUILayout.TextField("Err. mail", edit.dsProErrMail);
				edit.dsProErrNameExist= EditorGUILayout.TextField("Err. name exist", edit.dsProErrNameExist);
				edit.dsProErrId= EditorGUILayout.TextField("Err. ID", edit.dsProErrId);				
				edit.dsProErrPass= EditorGUILayout.TextField("Err. pass format", edit.dsProErrPass);
				edit.dsProErrBadPass= EditorGUILayout.TextField("Err. current pass", edit.dsProErrBadPass);
				edit.dsProErrPassConf= EditorGUILayout.TextField("Err. new pass conf", edit.dsProErrPassConf);
				edit.dsProSuccess= EditorGUILayout.TextField("Msg. profil updated", edit.dsProSuccess);
				edit.dsProPassSuccess= EditorGUILayout.TextField("Msg. pass updated", edit.dsProPassSuccess);
			}				
			
			EditorGUILayout.Space();			
			showDSMenuC = EditorGUILayout.Foldout(showDSMenuC, "Create game panel");
			if(showDSMenuC){
				EditorGUILayout.LabelField("CONTENT");	
				edit.dsMenuButtons[0]= EditorGUILayout.TextField("Titl. game", edit.dsMenuButtons[0]);
				edit.dsCreateNameTxt= EditorGUILayout.TextField("Txt. game name", edit.dsCreateNameTxt);
				edit.dsCreatePortTxt= EditorGUILayout.TextField("Txt. port", edit.dsCreatePortTxt);
				edit.dsCreatePlayersTxt= EditorGUILayout.TextField("Txt. players", edit.dsCreatePlayersTxt);
				edit.dsCreateMapTxt= EditorGUILayout.TextField("Txt. map", edit.dsCreateMapTxt);
				edit.dsCreateUsePassTxt= EditorGUILayout.TextField("Txt. use pass", edit.dsCreateUsePassTxt);
				edit.dsCreatePassTxt= EditorGUILayout.TextField("Txt. pass", edit.dsCreatePassTxt);
				edit.dsCreatePassConfTxt= EditorGUILayout.TextField("Txt. conf", edit.dsCreatePassConfTxt);
				edit.dsCreateButton= EditorGUILayout.TextField("Butt. create", edit.dsCreateButton);
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.dsCreateErr= EditorGUILayout.TextField("Err. ", edit.dsCreateErr);
				edit.dsCreateErrEmptyField= EditorGUILayout.TextField("Err. empty field", edit.dsCreateErrEmptyField);
				edit.dsCreateErrName= EditorGUILayout.TextField("Err. game name", edit.dsCreateErrName);
				edit.dsCreateErrPort= EditorGUILayout.TextField("Err. port", edit.dsCreateErrPort);
				edit.dsCreateErrPlayers= EditorGUILayout.TextField("Err. players", edit.dsCreateErrPlayers);
				edit.dsCreateErrPass= EditorGUILayout.TextField("Err. pass", edit.dsCreateErrPass);
				edit.dsCreateErrPassConf= EditorGUILayout.TextField("Err. pass conf", edit.dsCreateErrPassConf);
				edit.dsCreateErrHost= EditorGUILayout.TextField("Err. invalid host", edit.dsCreateErrHost);
				edit.dsCreateErrUsedPort= EditorGUILayout.TextField("Err. port already used", edit.dsCreateErrUsedPort);
				edit.dsCreateErrImpossible= EditorGUILayout.TextField("Err. unknow", edit.dsCreateErrImpossible);
				edit.dsCreateSuccess= EditorGUILayout.TextField("Msg. success", edit.dsCreateSuccess);
			}
			EditorGUILayout.Space();			
			showDSMenuG = EditorGUILayout.Foldout(showDSMenuG, "Game data panel");
			if(showDSMenuG){
				EditorGUILayout.LabelField("CONTENT");	
				edit.dsGameTitle= EditorGUILayout.TextField("Titl. game status", edit.dsGameTitle);
				edit.dsGameNameTxt= EditorGUILayout.TextField("Txt. name", edit.dsGameNameTxt);
				edit.dsGamePlayerTxt= EditorGUILayout.TextField("Txt. players", edit.dsGamePlayerTxt);
				edit.dsGameMapTxt= EditorGUILayout.TextField("Txt. map", edit.dsGameMapTxt);
				edit.dsLogTitle= EditorGUILayout.TextField("Titl. log", edit.dsLogTitle);
				edit.dsLogInstantiatedTxt= EditorGUILayout.TextField("Txt. log instantiate", edit.dsLogInstantiatedTxt);
				edit.dsLogPlayerConnectedTxt= EditorGUILayout.TextField("Txt. log connected", edit.dsLogPlayerConnectedTxt);
				edit.dsLogPlayerDisconnectedTxt= EditorGUILayout.TextField("Txt. log disconnected", edit.dsLogPlayerDisconnectedTxt);
				edit.dsPlayersTitle= EditorGUILayout.TextField("Titl. playerlist", edit.dsPlayersTitle);
				edit.dsCloseButton= EditorGUILayout.TextField("Butt. close server", edit.dsCloseButton);
				edit.dsStopTitle= EditorGUILayout.TextField("Titl. close server", edit.dsStopTitle);
				edit.dsStopTxt= EditorGUILayout.TextField("Txt. confirm", edit.dsStopTxt);
				edit.dsStop2Txt= EditorGUILayout.TextField("Txt. players nbr", edit.dsStop2Txt);
				edit.dsStopPlayerTxt= EditorGUILayout.TextField("Txt. players", edit.dsStopPlayerTxt);
				edit.dsStopConfirmButton= EditorGUILayout.TextField("Butt. confirm", edit.dsStopConfirmButton);
				edit.dsStopCancelButton= EditorGUILayout.TextField("Butt. cancel", edit.dsStopCancelButton);				
			}			
		}else{
			showDSMenuTxt= openTxt;
		}*/
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show ML menu text", EditorStyles.boldLabel);
		showMLMenu = EditorGUILayout.Foldout(showMLMenu, showMLMenuTxt);
		if(showMLMenu){
			showMLMenuTxt=closeTxt;
			
			EditorGUILayout.Space();				
			showMLMenuP = EditorGUILayout.Foldout(showMLMenuP, "Profil");
			if(showMLMenuP){
				EditorGUILayout.LabelField("CONTENT");	
				edit.mlmProfilTitle= EditorGUILayout.TextField("Titl. Profil settings", edit.mlmProfilTitle);
				edit.mlmProfilNameTxt= EditorGUILayout.TextField("Txt. player name", edit.mlmProfilNameTxt);
				edit.mlmProfilSave= EditorGUILayout.TextField("Butt. save", edit.mlmProfilSave);
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.mlmProfilSuccess= EditorGUILayout.TextField("Msg. save success", edit.mlmProfilSuccess);
				edit.mlmProfilFailed= EditorGUILayout.TextField("Err. save failed", edit.mlmProfilFailed);
				edit.mlmErrorName= EditorGUILayout.TextField("Err. invalid playername", edit.mlmErrorName);
			}
			
			EditorGUILayout.Space();				
			showMLMenuGM = EditorGUILayout.Foldout(showMLMenuGM, "Multiplayer menu");
			if(showMLMenuGM){
				edit.mlmMultiButtonsOptions[0]= EditorGUILayout.TextField("Titl. view games", edit.mlmMultiButtonsOptions[0]);
				edit.mlmMultiButtonsOptions[1]= EditorGUILayout.TextField("Titl. join from IP", edit.mlmMultiButtonsOptions[1]);
				edit.mlmMultiButtonsOptions[2]= EditorGUILayout.TextField("Titl. create game", edit.mlmMultiButtonsOptions[2]);	
			}
			
			EditorGUILayout.Space();				
			showMLMenuGIP = EditorGUILayout.Foldout(showMLMenuGIP, "Join game from IP");
			if(showMLMenuGIP){
				EditorGUILayout.LabelField("CONTENT");	
				edit.mlmJoinTitle= EditorGUILayout.TextField("Titl. join from IP", edit.mlmJoinTitle);
				edit.mlmJoinIpTxt= EditorGUILayout.TextField("Txt. IP", edit.mlmJoinIpTxt);
				edit.mlmJoinPortTxt= EditorGUILayout.TextField("Txt. port", edit.mlmJoinPortTxt);
				edit.mlmJoinButton= EditorGUILayout.TextField("Butt. connect", edit.mlmJoinButton);
				
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.mlmErrorJoinFailed= EditorGUILayout.TextField("Err. join failed", edit.mlmErrorJoinFailed);
				edit.mlmErrorJoinIp= EditorGUILayout.TextField("Err. invalid IP", edit.mlmErrorJoinIp);				
			}
			
			EditorGUILayout.Space();				
			showMLMenuGC = EditorGUILayout.Foldout(showMLMenuGC, "Create game");
			if(showMLMenuGC){
				EditorGUILayout.LabelField("CONTENT");	
				edit.mlmCreateTilte= EditorGUILayout.TextField("Titl. create game", edit.mlmCreateTilte);
				edit.mlmCreateNameTxt= EditorGUILayout.TextField("Txt. game name", edit.mlmCreateNameTxt);
				edit.mlmCreatePortTxt= EditorGUILayout.TextField("Txt. port", edit.mlmCreatePortTxt);
				edit.mlmCreatePlayersTxt= EditorGUILayout.TextField("Txt players nbr.", edit.mlmCreatePlayersTxt);
				edit.mlmCreatePrivateIpTxt= EditorGUILayout.TextField("Txt. private IP", edit.mlmCreatePrivateIpTxt);
				edit.mlmCreatePublicIpTxt= EditorGUILayout.TextField("Txt. public IP", edit.mlmCreatePublicIpTxt);
				edit.mlmCreateTypeTxt= EditorGUILayout.TextField("Txt. type", edit.mlmCreateTypeTxt);
				edit.mlmCreatePrivateGameMessage= EditorGUILayout.TextField("Txt. type private", edit.mlmCreatePrivateGameMessage);
				edit.mlmCreatePublicGameMessage= EditorGUILayout.TextField("Txt. type public", edit.mlmCreatePublicGameMessage);
				edit.mlmFormCreateTypeButtons[0]= EditorGUILayout.TextField("Val. type private", edit.mlmFormCreateTypeButtons[0]);
				edit.mlmFormCreateTypeButtons[1]= EditorGUILayout.TextField("Val. type public", edit.mlmFormCreateTypeButtons[1]);
				edit.mlmCreateButton= EditorGUILayout.TextField("Butt. create", edit.mlmCreateButton);
			
				EditorGUILayout.LabelField("MESSAGE & ERRORS");	
				edit.mlmErrorCreateFailed= EditorGUILayout.TextField("Err. create failed", edit.mlmErrorCreateFailed);
				edit.mlmErrorCreateName= EditorGUILayout.TextField("Err. invalid game name", edit.mlmErrorCreateName);
				edit.mlmErrorPort= EditorGUILayout.TextField("Err. invalid game port", edit.mlmErrorPort);
			}
		} else {
			showMLMenuTxt=openTxt;
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show waitroom text", EditorStyles.boldLabel);
		showWaitroom = EditorGUILayout.Foldout(showWaitroom, showWaitroomTxt);
		if(showWaitroom){
			showWaitroomTxt=closeTxt;					
			
			EditorGUILayout.Space();		
			edit.wrCustomSkin = EditorGUILayout.ObjectField(edit.wrCustomSkin, typeof(GUISkin), true) as GUISkin;
			showWP = EditorGUILayout.Foldout(showWP, "Playerlist");
			if(showWP){
				edit.wrPlayerListTitle= EditorGUILayout.TextField("Titl. playerlist", edit.wrPlayerListTitle);
				edit.wrPlayerListHostTxt= EditorGUILayout.TextField("Val. hsot", edit.wrPlayerListHostTxt);
				edit.wrPlayerListClientTxt= EditorGUILayout.TextField("Val. client", edit.wrPlayerListClientTxt);
				edit.wrPlayerListStatutGameTxt= EditorGUILayout.TextField("Val. in game", edit.wrPlayerListStatutGameTxt);
				edit.wrPlayerListStatWaitTxt= EditorGUILayout.TextField("Val. waiting", edit.wrPlayerListStatWaitTxt);

			}
			
			EditorGUILayout.Space();				
			showWGM = EditorGUILayout.Foldout(showWGM, "Game settings");
			if(showWGM){
				edit.wrGamePanelTitle= EditorGUILayout.TextField("Titl. game", edit.wrGamePanelTitle);
				edit.wrGamePanelNameTxt= EditorGUILayout.TextField("Txt. game name", edit.wrGamePanelNameTxt);
				edit.wrGamePanelPlayersRequireTxt= EditorGUILayout.TextField("Txt. min players", edit.wrGamePanelPlayersRequireTxt);
				edit.wrGamePanelPlayersMaxTxt= EditorGUILayout.TextField("Txt. max players", edit.wrGamePanelPlayersMaxTxt);
				edit.wrGamePanelPlayersNbrTxt= EditorGUILayout.TextField("Txt. total players", edit.wrGamePanelPlayersNbrTxt);
				edit.wrGamePanelMapTxt= EditorGUILayout.TextField("Txt. map", edit.wrGamePanelMapTxt);
				edit.wrGamePanelMapTitle= EditorGUILayout.TextField("Txt. select map", edit.wrGamePanelMapTitle);
				edit.wrGamePanelLoadMessage= EditorGUILayout.TextField("Txt. game start", edit.wrGamePanelLoadMessage);
				edit.wrGamePanelLoadButton= EditorGUILayout.TextField("Butt. start same", edit.wrGamePanelLoadButton);
				edit.wrGamePanelJoinButton= EditorGUILayout.TextField("Butt. join game", edit.wrGamePanelJoinButton);
				edit.wrGamePanelWaitButton= EditorGUILayout.TextField("Butt. wait", edit.wrGamePanelWaitButton);
			}
			
			EditorGUILayout.Space();				
			showWC = EditorGUILayout.Foldout(showWC, "Chat");
			if(showWC){
				edit.wrChatTitle= EditorGUILayout.TextField("Titl. chat", edit.wrChatTitle);
				edit.wrChatButtonTxt= EditorGUILayout.TextField("Butt. send msg", edit.wrChatButtonTxt);
			}
			
			EditorGUILayout.Space();				
			showWE = EditorGUILayout.Foldout(showWE, "Exit panel");
			if(showWE){
				edit.wrExitTitle= EditorGUILayout.TextField("Titl. exit", edit.wrExitTitle);
				edit.wrExitButtonTxt= EditorGUILayout.TextField("Butt. chat", edit.wrExitButtonTxt);
				edit.wrExitTxt= EditorGUILayout.TextField("Txt. confirm exit", edit.wrExitTxt);
				edit.wrExitConfirmButtonTxt= EditorGUILayout.TextField("Val. confirm", edit.wrExitConfirmButtonTxt);
				edit.wrExitCancelButtonTxt= EditorGUILayout.TextField("Val. cancel", edit.wrExitCancelButtonTxt);
			}

		} else {
			showWaitroomTxt=openTxt;	
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show game menu text", EditorStyles.boldLabel);
		showGameMenu = EditorGUILayout.Foldout(showGameMenu, showGameMenuTxt);
		if(showGameMenu){
			showGameMenuTxt=closeTxt;	
			
			EditorGUILayout.Space();	
			edit.gmCustomSkin = EditorGUILayout.ObjectField(edit.gmCustomSkin, typeof(GUISkin), true) as GUISkin;
			showGMB = EditorGUILayout.Foldout(showGMB, "Game menu buttons");
			if(showGMB){
				edit.gmMenuExit= EditorGUILayout.TextField("Butt. exit", edit.gmMenuExit);
				edit.gmMenuChat= EditorGUILayout.TextField("Butt. chat", edit.gmMenuChat);
				edit.gmMenuPL= EditorGUILayout.TextField("Butt. player list", edit.gmMenuPL);
			}
			
			EditorGUILayout.Space();				
			showGMEX = EditorGUILayout.Foldout(showGMEX, "Exclude player panel");
			if(showGMEX){
				edit.gmExcludeTitle= EditorGUILayout.TextField("Titl. exclude", edit.gmExcludeTitle);
				edit.gmExcludeText= EditorGUILayout.TextField("Txt. exclude", edit.gmExcludeText);
				edit.gmExcludeBlacklist= EditorGUILayout.TextField("Txt. blacklist", edit.gmExcludeBlacklist);
			}
			
			EditorGUILayout.Space();				
			showGMED = EditorGUILayout.Foldout(showGMED, "Excluded player panel");
			if(showGMED){
				edit.gmExcludePlayerTitle= EditorGUILayout.TextField("Titl. excluded", edit.gmExcludePlayerTitle);
				edit.gmExcludePlayerText= EditorGUILayout.TextField("Txt. excluded", edit.gmExcludePlayerText);
				edit.gmExcludePlayerButton= EditorGUILayout.TextField("Butt. back menu", edit.gmExcludePlayerButton);
			}
			
			EditorGUILayout.Space();				
			showGMAF = EditorGUILayout.Foldout(showGMAF, "Add in friendlist panel");
			if(showGMAF){
				edit.gmFriendTitle= EditorGUILayout.TextField("Titl. add in friendlist", edit.gmFriendTitle);
				edit.gmFriendText= EditorGUILayout.TextField("Txt. add in friendlist", edit.gmFriendText);
			}
			
			EditorGUILayout.Space();				
			showGMIF = EditorGUILayout.Foldout(showGMIF, "Invitation in friendlist pannel");
			if(showGMIF){
				edit.gmFriendAskTitle= EditorGUILayout.TextField("Titl. invitation", edit.gmFriendAskTitle);
				edit.gmFriendAskText= EditorGUILayout.TextField("Txt. friend invitation", edit.gmFriendAskText);
				edit.gmFriendAskQuestion= EditorGUILayout.TextField("Txt. accept invitation", edit.gmFriendAskQuestion);
				
			}
			EditorGUILayout.Space();				
			showGMAIF = EditorGUILayout.Foldout(showGMAIF, "Answer to invitation pannel");
			if(showGMAIF){
				edit.gmFriendAnswerTitle= EditorGUILayout.TextField("Titl. answer", edit.gmFriendAnswerTitle);
				edit.gmFriendAnswerTextYes= EditorGUILayout.TextField("Txt. answer:accepted", edit.gmFriendAnswerTextYes);
				edit.gmFriendAnswerTextNo= EditorGUILayout.TextField("Txt. anwer:refused", edit.gmFriendAnswerTextNo);
				edit.gmOk= EditorGUILayout.TextField("Butt. ok", edit.gmOk);
			}
			
			EditorGUILayout.Space();				
			showGMC = EditorGUILayout.Foldout(showGMC, "Chat pannel");
			if(showGMC){
				edit.gmChatButtonTxt= EditorGUILayout.TextField("Butt. send", edit.gmChatButtonTxt);
			}	
			
			EditorGUILayout.Space();				
			showGMM = EditorGUILayout.Foldout(showGMM, "Host migration panel");
			if(showGMM){
				edit.gmRebuildTitle= EditorGUILayout.TextField("Titl. host migration", edit.gmRebuildTitle);
				edit.gmRebuildSubTitle= EditorGUILayout.TextField("Titl. lost connection", edit.gmRebuildSubTitle);
				edit.gmRebuildInfoTxt= EditorGUILayout.TextField("Txt. host migration", edit.gmRebuildInfoTxt);
				edit.gmRebuildTimerTxt= EditorGUILayout.TextField("Txt. wait", edit.gmRebuildTimerTxt);
				edit.gmRebuildTimerSecTxt= EditorGUILayout.TextField("Txt. timer", edit.gmRebuildTimerSecTxt);
				edit.nmRebuildFailedInfoText= EditorGUILayout.TextField("Txt. no host found", edit.nmRebuildFailedInfoText);
				edit.gmRebuildSearchHostTxt= EditorGUILayout.TextField("Txt. new host", edit.gmRebuildSearchHostTxt);
				edit.gmRebuildSearchHostWaitTxt= EditorGUILayout.TextField("Txt. waiting players", edit.gmRebuildSearchHostWaitTxt);
				edit.nmRebuildSearchWaitSecTxt= EditorGUILayout.TextField("Txt. seconds", edit.nmRebuildSearchWaitSecTxt);
				edit.nmRebuildFailedInfoText= EditorGUILayout.TextField("Txt. no host found", edit.nmRebuildFailedInfoText);
				edit.gmRebuildFailedExitButton= EditorGUILayout.TextField("Butt. exit", edit.gmRebuildFailedExitButton);
				edit.gmRebuildFailedTryButton= EditorGUILayout.TextField("Butt. try again", edit.gmRebuildFailedTryButton);
				edit.nmRebuildSearchTxt= EditorGUILayout.TextField("Msg. searching", edit.nmRebuildSearchTxt);
				edit.nmRebuildSearchWaitTxt= EditorGUILayout.TextField("Msg. wait", edit.nmRebuildSearchWaitTxt);
			}	
			
			EditorGUILayout.Space();				
			showGME = EditorGUILayout.Foldout(showGME, "Exit game panel");
			if(showGME){				
				edit.gmExitTitle= EditorGUILayout.TextField("Titl. exit", edit.gmExitTitle);
				edit.gmExitMessage= EditorGUILayout.TextField("Txt. exit", edit.gmExitMessage);
				edit.gmExitButton= EditorGUILayout.TextField("Butt. confirm", edit.gmExitButton);
				edit.gmExitCancelButton= EditorGUILayout.TextField("Butt. cancel", edit.gmExitCancelButton);
			}
			
			EditorGUILayout.Space();				
			showGMGB = EditorGUILayout.Foldout(showGMGB, "General buttons");
			if(showGMGB){	
				edit.gmYes= EditorGUILayout.TextField("Butt. yes", edit.gmYes);
				edit.gmNo= EditorGUILayout.TextField("Butt. no", edit.gmNo);

			}		
			
					
		} else {
			showGameMenuTxt=openTxt;
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show network error messages", EditorStyles.boldLabel);
		showGameNetwork = EditorGUILayout.Foldout(showGameNetwork, showGameNetworkTxt);
		if(showGameNetwork){
			showGameNetworkTxt=closeTxt;
			
			EditorGUILayout.Space();
			edit.nmErrorGameCreation= EditorGUILayout.TextField("Err. create game", edit.nmErrorGameCreation);
			edit.nmErrorConnexion= EditorGUILayout.TextField("Err. connection", edit.nmErrorConnexion);
			edit.nmErrorMaxPlayers= EditorGUILayout.TextField("Err. max player", edit.nmErrorMaxPlayers);
			edit.nmErrorPassword= EditorGUILayout.TextField("Err. invalid pass", edit.nmErrorPassword);
			edit.nmErrorAlreadyConnect= EditorGUILayout.TextField("Err. already connect", edit.nmErrorAlreadyConnect);
			edit.nmErrorToAnotherServer= EditorGUILayout.TextField("Err. connect smwhere else", edit.nmErrorToAnotherServer);
			edit.nmErrorStartedGame= EditorGUILayout.TextField("Err. stated game", edit.nmErrorStartedGame);
			edit.nmErrorPrivateGame= EditorGUILayout.TextField("Err. private game", edit.nmErrorPrivateGame);
			edit.nmErrorUsedPort= EditorGUILayout.TextField("Err. used port", edit.nmErrorUsedPort);
			edit.nmErrorOnlineGame= EditorGUILayout.TextField("Err. online game", edit.nmErrorOnlineGame);
		} else {
			showGameNetworkTxt=openTxt;			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show size parameters", EditorStyles.boldLabel);
		showSize = EditorGUILayout.Foldout(showSize, showSizeTxt);
		if(showSize){
			showSizeTxt=closeTxt;			
			EditorGUILayout.Space();
			showSSGM = EditorGUILayout.Foldout(showSSGM, "Global");
			if(showSSGM){			
				edit.labelSizeX = EditorGUILayout.IntField("Label X", edit.labelSizeX);
				edit.labelSizeY = EditorGUILayout.IntField("Label Y", edit.labelSizeY);
				edit.fieldSizeX = EditorGUILayout.IntField("Field X", edit.fieldSizeX);
				edit.fieldSizeY = EditorGUILayout.IntField("Field Y", edit.fieldSizeY);
				edit.buttonMenuSizeX = EditorGUILayout.IntField("Menu button X", edit.buttonMenuSizeX);
				edit.buttonMenuSizeY = EditorGUILayout.IntField("Menu button Y", edit.buttonMenuSizeY);
				edit.buttonSubMenuSizeX = EditorGUILayout.IntField("Submenu button X", edit.buttonSubMenuSizeX);
				edit.buttonSubMenuSizeY = EditorGUILayout.IntField("Submenu button Y", edit.buttonSubMenuSizeY);
				edit.buttonSizeX = EditorGUILayout.IntField("Button X", edit.buttonSizeX);
				edit.buttonSizeY = EditorGUILayout.IntField("Button Y", edit.buttonSizeY);
				edit.buttonSelectSizeX = EditorGUILayout.IntField("Select button X", edit.buttonSelectSizeX);
				edit.buttonSelectSizeY = EditorGUILayout.IntField("Select button Y", edit.buttonSelectSizeY);
				edit.margin = EditorGUILayout.IntField("Margin", edit.margin);
				edit.internalMargin = EditorGUILayout.IntField("Internal margin", edit.internalMargin);
				edit.space = EditorGUILayout.IntField("Space", edit.space);
				edit.labelSpace = EditorGUILayout.IntField("Label space", edit.labelSpace);
				edit.labelMargin = EditorGUILayout.IntField("Label margin", edit.labelMargin);
				edit.formMargin = EditorGUILayout.IntField("Form margin", edit.formMargin);
			}
			showSSGL = EditorGUILayout.Foldout(showSSGL, "Game list");
			if(showSSGL){
				edit.listRefreshSizeY = EditorGUILayout.IntField("Refresh panel Y", edit.listRefreshSizeY);
				edit.listRefreshSizeX = EditorGUILayout.IntField("Refresh button X", edit.listRefreshSizeX);
				edit.listPanelSizeY = EditorGUILayout.IntField("Entry box Y", edit.listPanelSizeY);
				edit.listFieldSizeY = EditorGUILayout.IntField("Entry field Y", edit.listFieldSizeY);
				edit.gameListJoinButtonX = EditorGUILayout.IntField("Join button X", edit.gameListJoinButtonX);
				edit.gameListGameNameSizeX = EditorGUILayout.IntField("Col. game name X", edit.gameListGameNameSizeX);
				edit.gameListHostSizeX = EditorGUILayout.IntField("Col. host name X", edit.gameListHostSizeX);
				edit.gameListHostTypeSizeX = EditorGUILayout.IntField("Col. host type X", edit.gameListHostTypeSizeX);
				edit.gameListPingSizeX = EditorGUILayout.IntField("Col. ping X", edit.gameListPingSizeX);
				edit.gameListPlayerSizeX = EditorGUILayout.IntField("Col. players X", edit.gameListPlayerSizeX);
				edit.gameListMapSizeX = EditorGUILayout.IntField("Col. map X", edit.gameListMapSizeX);
				edit.gameListTypeSizeX = EditorGUILayout.IntField("Col. type X", edit.gameListTypeSizeX);
				edit.gameListStatusSizeX = EditorGUILayout.IntField("Col. status X", edit.gameListStatusSizeX);
				edit.gameListPasswordSizeX = EditorGUILayout.IntField("Col. pass X", edit.gameListPasswordSizeX);
				edit.gameListTypeSizeX = EditorGUILayout.IntField("Col. type X", edit.gameListTypeSizeX);
				edit.gameListFiltersSizeX = EditorGUILayout.IntField("Filters X", edit.gameListFiltersSizeX);
			}
			showSSFL = EditorGUILayout.Foldout(showSSFL, "Friend list [MO]");
			if(showSSFL){
				edit.momFriendlistNameSizeX = EditorGUILayout.IntField("Col. name X", edit.momFriendlistNameSizeX);
				edit.momFriendlistDateSizeX = EditorGUILayout.IntField("Col. date X", edit.momFriendlistDateSizeX);
				edit.momFriendlistSatutSizeX = EditorGUILayout.IntField("Col. status X", edit.momFriendlistSatutSizeX);
				edit.momFriendlistGameSizeX = EditorGUILayout.IntField("Col. game X", edit.momFriendlistGameSizeX);
				edit.momFriendlistGamePassSizeX = EditorGUILayout.IntField("Col. pass X", edit.momFriendlistGamePassSizeX);
				edit.momFriendlistJoinGameSizeX = EditorGUILayout.IntField("Col. join X", edit.momFriendlistJoinGameSizeX);
				edit.momFriendlistRemoveSizeX = EditorGUILayout.IntField("Col. remove X", edit.momFriendlistRemoveSizeX);
			}
			showSSBL = EditorGUILayout.Foldout(showSSBL, "Blacklist [MO]");
			if(showSSBL){
				edit.momBlacklistNameSizeX = EditorGUILayout.IntField("Col. name X", edit.momBlacklistNameSizeX);
				edit.momBlacklistDateSizeX = EditorGUILayout.IntField("Col. date X", edit.momBlacklistDateSizeX);
				edit.momBlacklistRemoveSizeX = EditorGUILayout.IntField("Col. remove X", edit.momBlacklistRemoveSizeX);
				edit.momBacklistAddSizeX = EditorGUILayout.IntField("Button remove X", edit.momBacklistAddSizeX);
			}
		} else {
			showSizeTxt=openTxt;			
		}
		
	}
}

using UnityEngine;
using System.Collections;

public class MText : MonoBehaviour {
	
	/******************  GUI SKIN ******************/	
	public GUISkin gmCustomSkin;	
	public GUISkin mmCustomSkin;
	public GUISkin wrCustomSkin;
	public GUISkin dsCustomSkin;
	/**************** BUTTONS SIZE *********************/
	
	// General
	public int labelSizeX=300;
	public int labelSizeY=20;
	public int fieldSizeX=160;
	public int fieldSizeY=20;
	public int buttonMenuSizeX=180;
	public int buttonMenuSizeY=50;
	public int buttonSubMenuSizeX=140;
	public int buttonSubMenuSizeY=30;
	public int buttonSizeX=160;
	public int buttonSizeY=20;
	public int buttonSelectSizeX = 120;
	public int buttonSelectSizeY = 25;
	public int margin = 10;
	public int internalMargin = 30;
	public int space=30;
	public int labelSpace=20;
	public int labelMargin=150;
	public int formMargin=200;
	
	// GameList
	public int listRefreshSizeY = 40;	
	public int listRefreshSizeX = 120;	
	public int listPanelSizeY=30;
	public int listFieldSizeY=30;
	public int gameListJoinButtonX=45;
	public int gameListGameNameSizeX = 90; // columns Sizes
	public int gameListHostSizeX= 80; // columns Sizes
	public int gameListHostTypeSizeX= 80; // columns Sizes
	public int gameListPingSizeX= 60; // columns Sizes
	public int gameListPlayerSizeX= 60; // columns Sizes
	public int gameListMapSizeX= 60; // columns Sizes
	public int gameListTypeSizeX= 60; // columns Sizes
	public int gameListStatusSizeX = 60; // columns Sizes
	public int gameListPasswordSizeX = 80;	// columns Sizes
	public int gameListFiltersSizeX = 150; // Filters columns size
	
	/*****************************************************/	
	
	/******************  MMenu MESSAGES ******************/	
	/*****************************************************/	
	public string mmWaitMessage = "... Wait...";	
	public string mmErrorIncompleteFields = "please fill in all fields. ";	
	public string mmCreateChooseTxt ="Choose the game type :";
	
	// MAIN MENU  
	// MultiOnline buttons
	public string mmMenuProfilOnlineButton = "ONLINE ACCOUNT";
	public string mmProfilOnlineTitle = "ONLINE ACCOUNT";
	
	public string mmMenuMultiOnlineButton = "ONLINE GAMES";
	public string mmMutliOnlineTitle = "ONLINE GAMES";
	
	// MultiLan buttons
	public string mmMenuProfilLanButton = "PROFIL";
	public string mmProfilLanTitle = "PROFIL";
	
	public string mmMenuMultiLanButton = "NETWORK GAMES";
	public string mmMutliLanTitle = "NETWORK GAMES";
	
	// General buttons	
	public string mmMenuExitButton = "EXIT";	
	public string mmMenuBackButton="< BACK TO MENU";
	
		
	// MULTIPLAYER VIEW GAMES 	
	// --- Text
	public string mmViewTilte ="VIEW GAMES";

	public string mmViewRefreshButton ="REFRESH LIST";
	public string mmViewGameNameTxt = "Game name";
	public string mmViewHostTxt= "Host name";
	public string mmViewHostTypeTxt= "Host type";
	public string mmViewPingTxt= "Ping";
	public string mmViewPlayerTxt= "Players";
	public string mmViewMapTxt= "Map";
	public string mmViewTypeTxt= "Type";
	public string mmViewStatusTxt = "Status";
	public string mmViewSearchTxt = "Search : ";
	public string mmViewSearchButton = "GO";	
	public string mmViewJoinButton ="JOIN";
	public string mmViewStatusWait ="Waiting";
	public string mmViewStatusGame ="In game";	
	public string mmViewTypePublic = "Public";
	public string mmViewTypePrivate = "Private";
	public string mmViewPortTxt = "Network games on port : ";
	public string mmViewPasswordTxt= "Password";
	public string mmViewNoPassTxt = "No";
	public string mmViewHostPlayer = "Player";
	public string mmViewHostServer = "Server";
	
	// --- Messages
	public string mmViewSearch = "... Search network games...";
	public string mmViewNoGame = "No game was found";
	public string mmViewFailed = " Games search failed : ";
	public string mmViewOneGame = "One game was found";
	public string mmViewManyGame = " games was found";	
	public string mmViewSearchNoMatch = "No game matching to search";
	public string mmViewSearchOne = "One game matching to search";
	public string mmViewSearchMany = " games matching to search";
	public string mmViewSearchInvalid = "Invalid search";
	public string mmErrorViewPassword = "invalid password";
	public string mmErrorView = "Error : ";
	public string mmErrorViewEmptyPassword ="enter the password for join this game";	
	
	// MULTIPLAYER FILTERS
	public string mmFilterTilte ="FILTERS";
	public string mmFilterMap ="Map:";
	public string mmFilterPassword ="Password:";
	public string mmFilterYes =" Yes";
	public string mmFilterNo =" No";
	public string mmFilterPing ="Ping max:";
	public string mmFilterPlayers ="Players number:";
	public string mmFilterStarted ="Started game:";
	public string mmFilterHostType = "Host type:";
	public string mmFilterHostTypePlayer = "Player";
	public string mmFilterHostTypeServer = "Server";
	public string mmFilterFullGames = "Show full games";
	
	/*****************************************************/	
	/*****************************************************/	

	
	/******************  MLMenu ******************/	
	/*****************************************************/	
	
	// PROFIL 
	// text
	public string mlmProfilTitle = "PROFIL SETTINGS";
	public string mlmProfilSave = "Save";
	public string mlmProfilNameTxt = "UserName: ";
	// --- Error & info messages
	public string mlmProfilSuccess = "your profil have been successful modified. ";
	public string mlmProfilFailed = "Change profil failed : ";
	public string mlmErrorName = "your userName must contain only alphanumeric characters or underscrores. ";
	
	// MULTIPLAYER  GAMES MENU
	public string[] mlmMultiButtonsOptions = new string[3]{"View games", "Join game from IP", "Create game"};	
		
	// MULTIPLAYER JOIN BY IP 	
	public string mlmJoinTitle ="JOIN GAME FROM IP";
	public string mlmJoinIpTxt ="Server ip : ";
	public string mlmJoinPortTxt ="Connection port : ";	
	public string mlmJoinButton = "Connect";
	
	// --- Error & info messages
	public string mlmErrorJoinFailed = "Error : ";
	public string mlmErrorJoinIp = "invalid IP format";
	
	// MULTIPLAYER CREATE GAME
	public string mlmCreateTilte ="CREATE GAME";
	public string mlmCreateNameTxt = "Game name :";
	public string mlmCreatePortTxt = "Port : ";
	public string mlmCreatePlayersTxt = "Maximum players : ";
	public string mlmCreateButton = "Create game";
	public string mlmCreatePrivateIpTxt = "Server private IP : ";
	public string mlmCreatePublicIpTxt = "Server public IP :";
	public string mlmCreateTypeTxt="Game type : ";
	public string mlmCreatePrivateGameMessage = "Only people on your network can join the game";
	public string mlmCreatePublicGameMessage = "Everyone can join the game with your public IP";
	
	// --- Form fields	
	public string[] mlmFormCreateTypeButtons = new string[2]{"Private", "Public"};
	
	// --- Error & info messages
	public string mlmErrorCreateFailed = "Error : "; 
	public string mlmErrorCreateName = "the game name must contain only alphanumeric characters or underscrores. ";
	public string mlmErrorPort ="the port is not numeric types. ";	

	
	/*****************************************************/	
	/*****************************************************/	

	
	/******************  MOMenu *****************/	
	/*****************************************************/	
	
	// LOGIN / REGISTER 
	// --- Text	
	public string[] momLoginButtons = new string[3]{"Login", "Regsiter", "Forgot login"};
	public string momLoginTxt = "You must be login to access on this menu";
	public string momLoginNameTxt = "UserName : ";
	public string momLoginMailTxt = "E-mail : ";
	public string momLoginPassTxt = "Password : ";
	public string momLoginPassConfTxt = "Confirm : ";
	public string momLoginLogButton = "Login";
	public string momRegisterButton = "Regsiter";
	public string momForgotLoginButton = "Receive login";
	public string momLoginSaveDataTxt = " Remember me";
	public string momLoginForgotMailTxt = "Account e-mail : ";
	public string momForgotLoginNameTxt = " Forgot userName";
	public string momForgotLoginPassTxt = " Forgot password";	

	// --- Error & info messages
	public string momErrorName = "Your userName must contain only alphanumeric characters or underscrores. ";
	public string momErrorPass = "Your password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string momErrorMail = "Invalid e-mail format. ";
	public string momErrorLoginPass = "Error on your password. ";
	public string momErrorLoginUserName = "Error on your userName. ";
	public string momErrorLoginAlreadyLog = "This player is already login. ";
	public string momErrorNameExist = "This userName is already used, please choose another. ";
	public string momErrorMailExist = "This e-mail address is already used, please choose another. ";
	public string momErrorPassConf = "Error on your password confirmation. ";
	public string momLoginFailed = "Login failed : ";
	public string momRegisterFailed = "Register failed : ";
	public string momForgotLoginFailed = "Error : ";
	public string momRegisterSuccess = "Registration completed successfully. You can login now.";
	public string momForgotLoginSuccessUserName = "You will receive an e-mail with your userName. ";
	public string momForgotLoginSuccessPass = "You will receive an e-mail with your new password. ";
	public string momErrorForgotLoginMail = "Error, there is not account with this e-mail. ";
	
	// PROFIL
	public string[] momSubMenuProfilButtons = new string[3]{"Account settings", "FriendsList", "Blacklist"};	
	
	// --- Text	
	public string momProfilTitle = "PROFIL SETTINGS";
	public string momProfilSave = "Save";
	public string momProfilMailTxt = "E-mail: ";
	public string momProfilNameTxt = "UserName: ";
	
	// --- Error & info messages
	public string momErrorLoginData= "Login error, impossible to change profil data. ";
	public string momProfilSuccess = "Your profil have been successful modified. ";
	public string momProfilFailed = "Change profil failed : ";
	
	// CHANGE PASSWORD
	// --- Text	
	public string momPasswordTilte = "CHANGE PASSWORD";
	public string momPasswordButton = "Save";
	public string momPasswordTxt = "Current password : ";
	public string momPasswordNewTxt = "New password : ";
	public string momPasswordNewConfTxt = "Confirm : ";
	
	// --- Error & info messages
	public string momErrorCurrentPass = "Error on you current password. ";
	public string momErrorNewPass = "Your new password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string momErrorNewPassConf = "Error on your new password confirmation. ";
	public string momPasswordSuccess = "Your password have been modified. ";
	public string momPasswordFailed = "The modification of your password failed : ";
	
	// BLACKLIST
	// --- Text
	public string momBlacklistNameTxt="Player name";
	public string momBlacklistDateTxt="Blacklist date";
	public string momBlacklistRemoveTxt="Remove";
	public string momBlacklistSearchTxt="Search : ";
	public string momBacklistAddTxt="Add player in blacklist :";
	public string momBacklistAddButton="Add";	
	public string momBlacklistSearchButton="GO";
	// Columns size parameters
	public int momBlacklistNameSizeX = 140;
	public int momBlacklistDateSizeX = 100;
	public int momBlacklistRemoveSizeX = 80;
	public int momBacklistAddSizeX = 130;
	// --- Error & info messages
	public string momBlacklistSearch = "... Searching...";
	public string momBlacklistEmpty = "Your blacklist is emplty";
	public string momBlacklistFailed = " Getting blacklist failed : ";
	public string momBlacklistMany=" entries on your blacklist";
	public string momBlacklistOne="One entry on your blacklist";
	public string momBlacklistAddFailed = "Added on blacklist failed : ";
	public string momErrorBlacklistName="this player name doesn't exist";
	public string momBlacklistRemoveFailed = "Removing on blacklist failed : ";
	public string momErrorBlacklistId=" player cannont be found";	
	public string momBlacklistSearchOne = "One entry matching to search";
	public string momBlacklistSearchMany = " entries matching to search";
	public string momBlacklistSearchNoMatch= "No player matching to search";		
	public string momBlacklistSearchInvalid="Invalid search";	
	
	// LOGOUT 
	// --- Text
	public string momLogoutTitle = "LOGOUT";
	public string momLogoutButton = "LOGOUT";
	
	// FRIENDLIST
		// --- Text
	public string momFriendlistNameTxt ="PlayerName";
	public string momFriendlistDateTxt="Add date";
	public string momFriendlistSatutTxt="Status";
	public string momFriendlistGameTxt="Game";
	public string momFriendlistGamePassTxt = "Game pass";
	public string momFriendlistJoinGameTxt="Join";
	public string momFriendlistSatutInGame="In game";
	public string momFriendlistSatutLogin="Online";
	public string momFriendlistSatutLogout="Logout";
	public string momFriendlistRemoveTxt="Remove";
	public string momFriendlistEmptyField ="-";
	public string momRefreshFriendlistButton = "Refresh";
	public string momSearchFriendlistButton = "GO";
	public string momFriendlistSearchTxt = "Search";
	// Columns size parameters
	public int momFriendlistNameSizeX = 140;
	public int momFriendlistDateSizeX = 100;
	public int momFriendlistSatutSizeX = 100;
	public int momFriendlistGameSizeX = 80;
	public int momFriendlistGamePassSizeX = 80;
	public int momFriendlistJoinGameSizeX = 60;
	public int momFriendlistRemoveSizeX = 80;
	
	// --- Error & info messages
	public string momFriendlistSearch ="... Searching... ";
	public string momFriendlistSearchNoMatch= "No friend matching to search";		
	public string momFriendlistSearchInvalid="Invalid search";
	public string momFriendlistSearchOne = "One entry matching to search";
	public string momFriendlistSearchMany = " entries matching to search";
	public string momFriendlistMany=" entries on your friendlist";
	public string momFriendlistOne="One entry on your friendlist";
	public string momFriendlistEmpty="Your friendlist is emplty";
	public string momFriendlistFailed = " Getting friendlist failed : ";
	
	// MULTIPLAYER MENU 
	public string[] momSubMenuMultiButtons = new string[2]{"View games", "Create game"};
	
	
	// MULTIPLAYER  CREATE GAMES 
	// ---Text
	public string momCreateTilte ="CREATE GAME";
	public string momCreateNameTxt = "Game name :";
	public string momCreatePortTxt = "Port : ";
	public string momCreateInfoPortTxt = "IMPORTANT: you must open the game port in your router, else nobody will be able to join your game.";
	public string momCreatePlayersTxt = "Maximum players : ";
	public string momCreateUsePassTxt = "Use password : ";
	public string momCreatePassTxt = "Password : ";
	public string momCreatePassConfTxt = "Confirm : ";
	public string momCreateButton = "Create game"; 
	
	// --- Error & info messages
	public string momErrorCreateFailed = "Error : "; 
	public string momErrorCreateName = "the game name must contain only alphanumeric characters or underscrores. ";
	public string momErrorCreatePort ="the port is not numeric types. ";	
	public string momErrorCreatePlayers = "the maximum players is not in numeric type. ";
	public string momErrorCreatePass ="the password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string momErrorCreatePassConf ="error on the password confirmation. ";
	/*******************************************************/	
	/*******************************************************/	
	
			
	/******************  WaitRoom MESSAGES ******************/	
	/*******************************************************/	
	// Player list
	public string wrPlayerListTitle= "PLAYER LIST";
	public string wrPlayerListHostTxt= "[host]";
	public string wrPlayerListClientTxt= "";
	public string wrPlayerListStatutGameTxt= "in game";
	public string wrPlayerListStatWaitTxt= "wait";
	
	// Game panel
	public string wrGamePanelTitle=  "GAME SETTINGS";
	public string wrGamePanelPlayersRequireTxt =  "Players require to start game:  ";
	public string wrGamePanelNameTxt =  "Game name:  ";
	public string wrGamePanelPlayersMaxTxt =  "Players max: ";
	public string wrGamePanelPlayersNbrTxt = "Players : ";
	public string wrGamePanelMapTxt =  "Map : ";		
	public string wrGamePanelMapTitle = "SELECT THE MAP";
	public string wrGamePanelLoadMessage = "THE GAME IS GOING TO START";
	public string wrGamePanelLoadButton = "START GAME";
	public string wrGamePanelJoinButton = "JOIN GAME";
	public string wrGamePanelWaitButton = "WAIT...";
	
	// Chat
	public string wrChatTitle="CHAT";
	public string wrChatButtonTxt="Send";
	
	// Exit
	public string wrExitButtonTxt= "Exit game";	
	public string wrExitTitle = "EXIT GAME";
	public string wrExitTxt = "Are you really sure that you want to exit this game ?";
	public string wrExitConfirmButtonTxt =  "YES";
	public string wrExitCancelButtonTxt = "NO";
	/*******************************************************/	
	/*******************************************************/	
	
	
	/******************  MNetwork MESSAGES *****************/	
	/*******************************************************/	
		// ERROR MESSAGES 
	public string nmErrorGameCreation = "IMPOSSIBLE TO CREATE A GAME ";
	public string nmErrorConnexion =  "IMPOSSIBLE TO CONNECT TO THIS HOST";
	public string nmErrorMaxPlayers =  " : maximum number of players reached.";
	public string nmErrorPassword =  " : invalid password.";
	public string nmErrorAlreadyConnect =  " : you are already connected on this host.";
	public string nmErrorToAnotherServer =  " : you are already connected to another host.";
	public string nmErrorStartedGame = "Impossible to join the host : the game is already started";
	public string nmErrorPrivateGame = "Impossible to join the host : the game is private";
	public string nmErrorUsedPort = " : the port is already use";
	public string nmErrorOnlineGame = "Impossible to join the host : the game is online and you are not connected";
	
	public string nmRebuildSearchWaitSecTxt = " seconds...";
	public string nmRebuildFailedInfoText =  "Impossible to find a new host";
	public string nmRebuildSearchTxt = "Searching new host...";
	public string nmRebuildSearchWaitTxt = "Wait ";
	
	
	/******************  GameMenu **************************/	
	/*******************************************************/	
	// GAME MENU TEXTS 	
	// -- Game menu buttons
	public string gmMenuExit= "Exit";
	public string gmMenuChat= "Chat";
	public string gmMenuPL= "Player list";
	
	// --- Rebuild
	public string gmRebuildTitle = "SEARCHING NEW HOST";
	public string gmRebuildSubTitle = "CONNECTION HAS BEEN LOST";
	public string gmRebuildInfoTxt = "The connexion with the game's host have been lost";
	
	public string gmRebuildSeachNewHostTxt = "Seaching new host...";
	public string gmRebuildTimerTxt = "Wait ";
	public string gmRebuildTimerSecTxt = " seconds";
	public string gmRebuildSearchHostTxt = "You are the new host of the game";
	public string gmRebuildSearchHostWaitTxt = "Waiting for players...";
	public string gmRebuildFailedExitButton= "EXIT GAME";	
	public string gmRebuildFailedTryButton = "TRY AGAIN";
	
	// --- Dedicated server break down
	public string gmBreakDownTitle = "THE SERVER IS DOWN";
	public string gmBreakDownTxt = "The connexion with the server have been lost";
	
	public string gmYes = "YES";
	public string gmNo = "NO";
	
	// --- Exit
	public string gmExitTitle = "EXIT GAME";
	public string gmExitMessage = "Are you really sure that you want to exit this game ?";
	public string gmExitButton = "YES";
	public string gmExitCancelButton = "NO";
	
	// --- Exculde player
	public string gmExcludeTitle = "EXCULDE PLAYER";
	public string gmExcludeText = "Are you sure that you want to exculde this player of your game ?";
	public string gmExcludeBlacklist = " Blacklist this player";
	
	// --- Exculded player
	public string gmExcludePlayerTitle = "EXCULSION";
	public string gmExcludePlayerText = "You have been exculde of the game by the host.";
	public string gmExcludePlayerButton = "Back on menu";	
	
	// --- Friend player
	public string gmFriendTitle = "ADD PLAYER IN FRIENDLIST";
	public string gmFriendText = "Are you sure that you want to add this player as friend ?";
	
	public string gmFriendAskTitle = "FRIEND INVITATION";
	public string gmFriendAskText = " ask you to be his friend.";
	public string gmFriendAskQuestion = "Do you accept his request ?";
	
	public string gmFriendAnswerTitle = "FRIEND INVITATION";
	public string gmFriendAnswerTextYes = " has accepted your request.";
	public string gmFriendAnswerTextNo = " has declined your request.";
	public string gmOk="OK";
	
	
	/******************  GameChat MESSAGES *****************/	
	/*******************************************************/	
	public string gmChatButtonTxt = "Send";
	
	
	/******************  MODSMenu **************************/	
	/*******************************************************/
	// Menu buttons & title
	public string[] dsLoginButtons= new string[2]{"Login", "Create account"};
	public string[] dsMenuButtons= new string[3]{"Host game", "Server settings", "Logout"};
	public string dsTitle = "DEDICATED SERVER";
	
	// REGISTER & LOGIN	
	// Form login
	public string dsLogNameTxt = "Server name: ";
	public string dsLogPassTxt = "Password: ";
	public string dsLogRememberTxt = " Remember me";
	public string dsLogButton = "Login";	
	// Form register 
	public string dsRegNameTxt = "Server name: ";
	public string dsRegLocationTxt = "Server location: ";
	public string dsRegMailTxt = "Admin e-mail: ";
	public string dsRegPassTxt = "Password: ";
	public string dsRegPassConfTxt = "Password confirmation: ";
	public string dsRegButton = "Register";	
	// Error and other messages
	public string dsRegErr="Error:";
	public string dsRegErrEmptyField=" Please, fill all the fields.";
	public string dsRegErrName=" Invalid format in server name.";
	public string dsRegErrLoc=" Invalid format in server location.";
	public string dsRegErrMail=" Invalid format in admin e-mail.";
	public string dsRegErrPass=" Invalid format in password.";
	public string dsRegErrPassConf=" Bad password conformation.";
	public string dsRegErrNameExist=" This server name is already use, please, choose an other";
	public string dsRegSuccess="Your server is well register, you can now login";
	public string dsLogErrPass=" Bad password";
	public string dsLogErrName=" This server doesn't exists.";
	
	// CREATE GAME
	// Form create
	public string dsCreateNameTxt =  "Game name: ";
	public string dsCreatePortTxt = "Game port: ";
	public string dsCreatePlayersTxt = "Players: ";
	public string dsCreateMapTxt = "Map";
	public string dsCreateUsePassTxt = "Use password";
	public string dsCreatePassTxt = "Password: ";
	public string dsCreatePassConfTxt = "Confirm password: ";
	public string dsCreateButton = "Create";
	// Game info pannel
	public string dsGameTitle = "GAME STATUS";
	public string dsGameNameTxt = "Game name: ";
	public string dsGamePlayerTxt = "Players number: ";
	public string dsGameMapTxt =  "Map: ";
	
	public string dsLogTitle = "Log";
	public string dsPlayersTitle = "Players";
	public string dsCloseButton = "Close server";
		// Error and other messages
	public string dsCreateErr="Error:";
	public string dsCreateErrEmptyField=" Please, fill all the fields.";
	public string dsCreateErrName=" Invalid format in game name.";
	public string dsCreateErrPort=" Invalid game port.";
	public string dsCreateErrPlayers=" Invalid player number.";
	public string dsCreateErrPass=" Invalid format in password.";
	public string dsCreateErrPassConf=" Bad in password conformation.";
	public string dsCreateErrHost = " Invalid host id";
	public string dsCreateSuccess = "Game created";
	public string dsCreateErrUsedPort = " This port is alreay used, please choose another";
	public string dsCreateErrImpossible =" Impossible to create this game";
	
	
	// PROFIL
	// Form
	public string dsProTitle = "ACCOUNT DATA";
	public string dsProNameTxt = "Server name: ";
	public string dsProLocationTxt = "Server location: ";
	public string dsProMailTxt = "Admin e-mail: ";
	public string dsProChangeButton = "Save change";
	public string dsProPassTitle = "CHANGE PASSWORD";
	public string dsProPassCurrentTxt = "Current password: ";
	public string dsProPassNewTxt = "New password: ";
	public string dsProPassNewConfTxt = "New password confirmation: ";
	public string dsProPassChangeTxt = "Save new password";	
	// Error and other messages	
	public string dsProErr = "Error:";
	public string dsProErrEmptyField = " Please, fill all the fields.";
	public string dsProErrName = " Invalid format in server name.";
	public string dsProErrLoc=" Invalid format in server location.";
	public string dsProErrMail= " Invalid format in admin e-mail.";
	public string dsProErrNameExist=" This server name is already use, please, choose an other";
	public string dsProErrId=" ID error, impossible to update the profil";
	public string dsProErrPass=" Invalid format in password.";
	public string dsProErrBadPass=" Error on your current password.";
	public string dsProErrPassConf=" Bad password conformation.";
	public string dsProSuccess = "Profil updated";
	public string dsProPassSuccess = "New pass updated";
	
	// Stop server pannel
	public string dsStopTitle = "STOP SERVER";
	public string dsStopTxt =  "Caution, all the players will be disconnected";
	public string dsStop2Txt = "The server has currently ";
	public string dsStopPlayerTxt=  " players";
	public string dsStopConfirmButton = "Confirm";
	public string dsStopCancelButton = "Cancel";
	
	// LOG MESSAGES
	public string dsLogInstantiatedTxt = " - Server instantiated";
	public string dsLogPlayerConnectedTxt =  " - Player connected : ";
	public string dsLogPlayerDisconnectedTxt = " - Player disconnected : ";
	
	
	
	
	public void Awake(){
		DontDestroyOnLoad(this);	
		this.name = "MText";
	}//Awake
}//MText

using UnityEngine;
using System;
using MultiPlayer;

public class MMenu : MonoBehaviour {		
	public GameObject dataObj;
	public GameObject networkObj;	
	public MPlayerData playerDataSrc;	
	public MNetwork networkSrc;	
	public MText text;	
	public MGameParameter parameters;	
	
	/********* LAN & ONLINE PARAMETERS **************/
	public bool useOnline = false;
	public bool useLan = false;
	
	// USE IT ONLY IF YOU HAVE MULTILAN : 
	private MLMenu menuLan;
	
	// USE IF ONLY IF YOU HAVE MULTIONLINE : 
	//private MOMenu menuOnline;
	
	// Size parameters menu
	public int menuButtonNumber = 1;	
	private int sizeY;
	public int buttonBackMenuSizeY;
	
	private int menuSizeX;
	private int menuSizeY;
	private int menuPosX;
	private int menuPosY;
	private Rect mainMenu;
	
	public int menuPageSizeX;
	public int menuPageSizeY;
	public float menuPagePosX;
	public float menuPagePosY;	
	public Rect menuPage;			

	
	// Buttons
	public bool	displayProfilLan = false;					
	public bool	displayProfilOnline = false;
	public bool	displayGameOnline = false;
	public bool	displayGameLan = true;					
	public bool	displayMainMenu = false;	
	
	// Network messages
	public string[] networkJoinMessage;
	public string[] networkCreateMessage;
	
	void Awake(){
		// Instantiate useful objects if they are not yet instantiated	
		try{			
			dataObj = GameObject.Find ("MPlayerData");
			playerDataSrc = dataObj.GetComponent<MPlayerData>();
		} catch(NullReferenceException){
			dataObj = Instantiate(Resources.Load("MPlayerData")) as GameObject;
			playerDataSrc = dataObj.GetComponent<MPlayerData>();
		}		
		try{			
			text = GameObject.Find ("MText").GetComponent<MText>(); 
		} catch(NullReferenceException){
			text = (Instantiate(Resources.Load("MText")) as GameObject).GetComponent<MText>(); 
		}
		try{
            parameters = GameObject.Find("LevelOption").GetComponent<MGameParameter>();	
		} catch(NullReferenceException){
            parameters = (Instantiate(Resources.Load("LevelOption")) as GameObject).GetComponent<MGameParameter>();	
		}
	}//Awake
	
	void Start(){	
		networkJoinMessage = new string[2]{null,null};
		networkCreateMessage = new string[2]{null,null};
		DefineUsedMenu();

        displayMainMenu = false;
        displayGameLan = true;
	}//Start
	
	void OnGUI(){
		GUI.skin = text.mmCustomSkin;
		DefinePositions();
        //if(displayMainMenu){
        //    GUI.BeginGroup(mainMenu, "");
        //    sizeY=0;
        //    DisplayMainMenu();
        //    GUI.EndGroup();
        //} else {
        //        DisplayBackButton();
        //}
		DisplaySubMenu();
		}//OnGUI
	
	private void DisplayMainMenu(){
		// DISPLAY LAN PROFIL
		if(useLan){
			if(GUI.Button(new Rect(0,sizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuProfilLanButton)){
				DefineDisplayMenu(true, false, false, false, false);
			}
			sizeY+=text.buttonMenuSizeY+text.margin;
		}	
		// DISPLAY ONLINE ACCOUT
		if(useOnline){
			if(GUI.Button(new Rect(0,sizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuProfilOnlineButton)){
				DefineDisplayMenu(false, true, false, false, false);
			}
			sizeY+=text.buttonMenuSizeY+text.margin;
		}		
		// DISPLAY ONLINE GAMES
		if(useOnline){
			if(GUI.Button(new Rect(0,sizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuMultiOnlineButton)){
				DefineDisplayMenu(false, false, true, false, false);
			}
			sizeY+=text.buttonMenuSizeY+text.margin;
		}
		// DISPLAY NETWORK GAMES
		if(useLan){
			if(GUI.Button(new Rect(0,sizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuMultiLanButton)){
				DefineDisplayMenu(false, false, false, true, false);
			}
			sizeY+=text.buttonMenuSizeY+text.margin;
		}
		
		// EXIT
        //if(GUI.Button(new Rect(0,sizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuExitButton)){
        //    Application.Quit();
        //} 			
	}//DisplayMainMenu
	
	public void DisplayBackButton(){
		if(GUI.Button(new Rect(text.margin,buttonBackMenuSizeY,text.buttonMenuSizeX,text.buttonMenuSizeY), text.mmMenuBackButton)){
				DefineDisplayMenu(false, false, false, false, true);						
			} 
	}//DisplayBackButton
	
	public void DisplaySubMenu(){
		// USE IF ONLY IF YOU HAVE MULTILAN :
		// Display lan menu		
		if(menuLan != null && useLan){		
			menuLan.DisplayMenu();
		}
		
		// USE IF ONLY IF YOU HAVE MULTIONLINE : 
		// Display online menu
		/*if(menuOnline != null && useOnline){
			menuOnline.DisplayMenu();
		}*/
	}//DisplaySubMenu
	
	private void DefineUsedMenu(){
		// USE IT ONLY IF YOU HAVE MULTILAN : 
		// Search if we've got the MLMenu script
		try{
			menuLan = this.GetComponent<MLMenu>();
			if(menuLan != null && menuLan.enabled != false){
				// If we have it :
				useLan = true; // Put useLan on true
				menuButtonNumber+=2;
			}
		} catch(NullReferenceException){}
		// USE IF ONLY IF YOU HAVE MULTIONLINE : 
		// Search if we've got the MOMenu script
		/*try{
			menuOnline = this.GetComponent<MOMenu>();
			if(menuOnline != null && menuOnline.enabled != false){
				// If we have it :
				useOnline = true; // Put useOnline on true	
				menuButtonNumber+=2;
			}
		} catch(NullReferenceException){}*/
	}//DefineUsedMenu
	
	private void DefinePositions(){
		menuSizeX = Screen.width;
		menuSizeY = Screen.height;
		menuPosX = (Screen.width - text.buttonMenuSizeX )/2;
		menuPosY = (Screen.height/2) - (((text.buttonMenuSizeY*menuButtonNumber)+(text.margin*(menuButtonNumber-1)))/2);
		mainMenu = new Rect(menuPosX, menuPosY, menuSizeX, menuSizeY);	
		
		menuPageSizeX = Screen.width - text.margin*2;
		menuPageSizeY = Screen.height- text.margin*3 - text.buttonMenuSizeY;
		menuPagePosX = text.margin;
		menuPagePosY = text.margin;
		menuPage = new Rect(menuPagePosX, menuPagePosY, menuPageSizeX, menuPageSizeY);	
		buttonBackMenuSizeY = Screen.height - text.margin - text.buttonMenuSizeY;
	}//DefinePositions
	
	
	private void DefineDisplayMenu(bool displayProfilLan, bool displayProfilOnline, bool displayGameOnline, bool displayGameLan, bool displayMainMenu){
		this.displayProfilLan = displayProfilLan;
		this.displayProfilOnline = displayProfilOnline;
		this.displayGameOnline = displayGameOnline;
		this.displayGameLan = displayGameLan;
		this.displayMainMenu = displayMainMenu;
	}//DefineDisplayMenu
	
	public void LanGetNetworkGames(bool arg){
		// USE IT ONLY IF YOU HAVE MULTILAN
		/*if(useLan && menuLan != null){
			menuLan.GetNetworkGames(arg);
		}*/
	} //LanGetNetworkGames
	
}//MMenu

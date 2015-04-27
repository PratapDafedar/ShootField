using UnityEngine;
using System.Collections;
using System;

public class MPlayerNetwork : MonoBehaviour {	
	public GameObject cameraPlayer;
	private MNetwork networkSrc;

	void Awake(){		
		DontDestroyOnLoad(this);		
		this.name = "Player";
	}//Awake
	
	void Start () {	
		if (!GetComponent<NetworkView>().isMine) { // If this is not my player
			Destroy(cameraPlayer); // Destroy camera
			this.enabled = false; // Disabled this script	
			this.GetComponent<MPlayerMove>().enabled = false; // Disabled PlayerMove script			
		
		} else  {// If it's my player : search the networkManager component	
			networkSrc = GameObject.Find("MNetwork").GetComponent<MNetwork>();
			networkSrc.PlayerSpawned(); // Informs the other players that I'm spawned			
		}	
	}//Start
	
	void Update (){
		SavePosition();
	}//Update
	
	void OnGUI(){
		if(networkSrc.parameters.displayPlayerName){
			for(int i= 0; i < networkSrc.positionList.Count; i++){
				if(DisplayPlayerName(transform.position, networkSrc.positionList[i].position)){
					Vector3 namePosition = this.GetComponentInChildren<Camera>().WorldToScreenPoint(networkSrc.positionList[i].position);// gets screen position
					GUI.Box(new Rect(namePosition.x-5, namePosition.y-namePosition.z, 100, 22),"");
					GUI.Label(new Rect(namePosition.x, namePosition.y-namePosition.z, 80, 20), networkSrc.positionList[i].name);
				}
			}
		}
	}//OnGUI
	
	private void SavePosition(){
		networkSrc.playerPrefabPosition = transform.position;
		networkSrc.playerPrefabRotation = transform.rotation;
	}//SavePosition
	
	private bool DisplayPlayerName(Vector3 pos1, Vector3 pos2){
		float diffX = Math.Abs(pos1.x-pos2.x);
		float diffZ = Math.Abs(pos1.z-pos2.z);		
		if(diffX <= networkSrc.parameters.playerNameVisibility  || diffZ <= networkSrc.parameters.playerNameVisibility){
			return true;
		} 
		return false;
	}//DisplayPlayerName
}//MPlayerNetwork

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class User 
{
    public int   id;
    public string   name;

	public int      gameId;
	public string   privateIp;
	public string   publicIp;		
	public int      playerPing;
	public bool     isPlayerInGame;
	public bool     isGameHost;

    public enum Team
    {
        RED = 0,
        BLUE,
    };
    public Team cTeam;

	/******************* CONSTRUCTORS *******************/
    public User()
    {

    }

	// Constructor with variable in string 
	public User(string id, string gameId, string privateIp, string publicIp, string playerName, string playerPing, string isPlayerInGame, string isGameHost){
		this.id = int.Parse(id);
		this.gameId = int.Parse(gameId);
		this.privateIp = privateIp;
		this.publicIp = publicIp;
        this.name = playerName;	
		this.playerPing = int.Parse(playerPing);	
		this.isPlayerInGame = bool.Parse(isPlayerInGame);	
		this.isGameHost = bool.Parse(isGameHost);
	}//User
		
	// Constructor with variable in real types 
	public User(int id, int gameId, string privateIp, string publicIp, string playerName, int playerPing, bool isPlayerInGame, bool isGameHost){
		this.id = id;
		this.gameId = gameId;
		this.privateIp = privateIp;
		this.publicIp = publicIp;
        this.name = playerName;
		this.playerPing = playerPing;
		this.isPlayerInGame = isPlayerInGame;			
		this.isGameHost = isGameHost;
	}//User		
					
	/******************* CONSTRUCTORS - END *******************/

    public void ExitGame()
    {
        this.isPlayerInGame = false;
    }

	// Transform the User object parameters on a string
	public string UserToString(){
		return id.ToString() + "$"
            +gameId.ToString()+"$"
            +privateIp+"$"
            +publicIp+"$"
            +name+"$"
            +playerPing.ToString()+"$"
            +isPlayerInGame+"$"
            +isGameHost.ToString();
	}//UserToString
		
	// Create an User object with parameters from a string
	public static User UserToObject(string parseUser){
		string[] values = parseUser.Split('$');
		return new User(values[0], values[1], values[2],values[3], values[4], values[5], values[6], values[7]);
	}//parseUser
		
	// Transform a userList (List<User>) on a string
	public static string ListToString(List<User> userList){
		string values = "";
		for(int i = 0; i < userList.Count; i++){
			values+= userList[i].UserToString();
			if(i < userList.Count-1){
				values+="#";
			}				
		}
		return values;
	}//userList
		
	// Create a userList (List<User>) from a string
	public static List<User> ListToObject(string parseUser){
		List<User> userList = new List<User>();
		string[] values = parseUser.Split('#');
		for(int i = 0; i < values.Length; i++){
			userList.Add(UserToObject(values[i]));
		}
		return userList;
	}//ListToObject
		
	// Remove a user from his M - Return the modified userList
	public static List<User> RemoveFromId(List<User> userList, int gameId){
		User removeUser = null;
		for(int i = 0; i < userList.Count; i++){
			if(gameId == userList[i].gameId){					
				removeUser = userList[i];
			}
		}			
		if(removeUser != null){
			userList.Remove(removeUser);
		}			
		return userList;
	}//RemoveFromId
		
	// Remove the server of the userList (call OnDisconnectedFromServer) - Return the modified userList
	public static List<User> RemoveServer(List<User> userList){
		User removeUser = null;
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].isGameHost == true){
				removeUser = userList[i];
			}
		}			
		if(removeUser != null){
			userList.Remove(removeUser);
		}			
		return userList;
	}//RemoveServer
				
	// Save the new status of a player - Return the modified userList
	public static List<User> SavePlayerStatus(int gameId, bool isPlayerInGame, List<User> userList) {
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].gameId == gameId){
				userList[i].isPlayerInGame = isPlayerInGame;
			}
		}	
		return userList;
	}//SavePlayerStatus
				
	// Search a player from his game id (return him as User object)
	public static User SearchPlayer(int gameId, List<User> userList) {
		User searchUser = new User();
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].gameId == gameId){
				searchUser = userList[i];
			}
		}	
		return searchUser;
	}//SearchPlayer
		
	// Search a player from his id (return him as User object)
	public static User SearchPlayerId(int id, List<User> userList) {
		User searchUser = new User();
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].id == id){
				searchUser = userList[i];
			}
		}	
		return searchUser;
	}//SearchPlayer
		
	// Check if a specific player is on the userList
	public static bool inList(List<User> userList, int gameId){
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].gameId == gameId){
				return true;
			}
		}	
		return false;
	}//userList	
		
	public static List<User> PingSort(List<User> userList){
		return userList.OrderBy(x=>x.playerPing).ToList();	
	}//PingSort
		
	public static void PingPlayers(NetworkManager networkSrc){
		networkSrc.PingPlayers();
	}//PingPlayers
		
	public static string GetHostMessage(int key, NetworkManager networkSrc){
		if(networkSrc.playerList[key].gameId == networkSrc.gameInfo.hostId){
			return "[host]";
		}
		return "";
	}//GetHostMessage
		
	public static string GetPlayerPing(int key, NetworkManager networkSrc){
		return networkSrc.playerList[key].playerPing.ToString();			
	}//GetPlayerPing
}

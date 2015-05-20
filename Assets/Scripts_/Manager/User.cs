using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class User 
{
    public int      id;
    public string   name;

	public string   gameId;
	public string   privateIp;
	public string   publicIp;		
	public int      playerPing;
	public bool     isPlayerInGame;
	public bool     isGameHost;

    //game-play properties.
    public float    health;
    public int      score;

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
	public User(string id, string gameId, Team team, int score, string privateIp, string publicIp, string playerName, string playerPing, string isPlayerInGame, string isGameHost){
		this.id = int.Parse(id);
		this.gameId = gameId;
        this.cTeam = team;
		this.privateIp = privateIp;
		this.publicIp = publicIp;
        this.name = playerName;	
		this.playerPing = int.Parse(playerPing);	
		this.isPlayerInGame = bool.Parse(isPlayerInGame);	
		this.isGameHost = bool.Parse(isGameHost);
	}//User
		
	// Constructor with variable in real types 
	public User(int id, string gameId, Team team, string privateIp, string publicIp, string playerName, int playerPing, bool isPlayerInGame, bool isGameHost){
		this.id = id;
		this.gameId = gameId;
        this.cTeam = team;
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
            +(int)cTeam + "$"
            + score + "$"
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
		return new User(values[0], values[1], (Team)int.Parse(values[2]), int.Parse(values[3]), values[4], values[5], values[6], values[7], values[8], values[9]);
	}//parseUser
		
	// Transform a userList (List<User>) on a string
	public static string TableToString(Dictionary<int, User> userList){
		string values = "";

        int i = 0;
		foreach(int key in userList.Keys)
        {
			values+= userList[key].UserToString();
			if(i < userList.Count-1){
				values+="#";
			}
            i++;
		}
		return values;
	}//userList

    public static string ListToString(List<User> userList)
    {
        string values = "";

        int i = 0;
        if (userList == null)
            return "";

        foreach (User user in userList)
        {
            values += user.UserToString();
            if (i < userList.Count - 1)
            {
                values += "#";
            }
            i++;
        }
        return values;
    }//userList
		
	// Create a userList (List<User>) from a string
	public static Dictionary<int, User> ListToObject(string parseUser)
    {
		Dictionary<int, User> userList = new Dictionary<int, User>();
		string[] values = parseUser.Split('#');
		for(int i = 0; i < values.Length; i++)
        {
            User user = UserToObject(values[i]);
			userList.Add(user.id, user);
		}
		return userList;
	}//ListToObject
		
	// Remove a user from his M - Return the modified userList
	public static Dictionary<int, User> RemoveFromId(Dictionary<int, User> userList, string gameId){
		User removeUser = null;

        int id = 0;
		foreach(int key in userList.Keys)
        {
			if(gameId == userList[key].gameId)
            {					
				removeUser = userList[key];
                id = key;
			}
		}
        if(removeUser != null){
			userList.Remove(id);
		}			
		return userList;
	}//RemoveFromId
		
	// Remove the server of the userList (call OnDisconnectedFromServer) - Return the modified userList
	public static Dictionary<int, User> RemoveServer(Dictionary<int, User> userList)
    {
		int removeUserId = -1;
		foreach(int id in userList.Keys)
        {
			if(userList[id].isGameHost == true)
            {
				removeUserId = id;
			}
		}			
		if(removeUserId != -1)
        {
			userList.Remove(removeUserId);
		}			
		return userList;
	}//RemoveServer
				
	// Save the new status of a player - Return the modified userList
	public static Dictionary<int, User> SavePlayerStatus(string gameId, bool isPlayerInGame, Dictionary<int, User> userList) 
    {
		foreach (int id in userList.Keys)
        {
			if(userList[id].gameId == gameId)
            {
				userList[id].isPlayerInGame = isPlayerInGame;
			}
		}	
		return userList;
	}//SavePlayerStatus
				
	// Search a player from his game id (return him as User object)
	public static User SearchPlayer(string gameId, List<User> userList) {
		User searchUser = new User();
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].gameId == gameId){
				searchUser = userList[i];
			}
		}	
		return searchUser;
	}//SearchPlayer
		
	// Search a player from his id (return him as User object)
	public static User SearchPlayerId(int id, List<User> userList) 
    {
		for(int i = 0; i < userList.Count; i++){
			if(userList[i].id == id){
				return userList[i];
			}
		}	
		return null;
	}//SearchPlayer
		
	// Check if a specific player is on the userList
	public static bool inList(Dictionary<int, User> userList, string gameId)
    {
		foreach(int key in userList.Keys)
        {
			if(userList[key].gameId == gameId){
				return true;
			}
		}	
		return false;
	}//userList	
		
	public static List<User> PingSort(Dictionary<int, User> userList)
    {
		return userList.Values.ToList<User>().OrderBy(x=>x.playerPing).ToList();	
	}//PingSort
		
	public static void PingPlayers(NetworkManager networkSrc){
		networkSrc.PingPlayers();
	}//PingPlayers
		
	public static string GetHostMessage(int key, NetworkManager networkSrc){
		if(networkSrc.playerTable[key].gameId == networkSrc.gameInfo.hostId){
			return "[host]";
		}
		return "";
	}//GetHostMessage
		
	public static string GetPlayerPing(int key, NetworkManager networkSrc){
		return networkSrc.playerTable[key].playerPing.ToString();			
	}//GetPlayerPing
}

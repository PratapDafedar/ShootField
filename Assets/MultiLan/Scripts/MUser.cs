using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MultiPlayer {
	public class MUser {	
		public int id;
		public int gameId;
		public string privateIp;
		public string publicIp;		
		public int playerPing;
		public string playerName;
		public int isPlayerInGame;
		public int isGameHost;
		
		/******************* CONSTRUCTORS *******************/
		// Constructor whitout parameters
		public MUser(){}//MUser
		
		// Constructor with variable in string 
		public MUser(string id, string gameId, string privateIp, string publicIp, string playerName, string playerPing, string isPlayerInGame, string isGameHost){
			this.id = int.Parse(id);
			this.gameId = int.Parse(gameId);
			this.privateIp = privateIp;
			this.publicIp = publicIp;
			this.playerName = playerName;	
			this.playerPing = int.Parse(playerPing);	
			this.isPlayerInGame = int.Parse(isPlayerInGame);	
			this.isGameHost = int.Parse(isGameHost);
		}//MUser
		
		// Constructor with variable in real types 
		public MUser(int id, int gameId, string privateIp, string publicIp, string playerName, int playerPing, int isPlayerInGame, int isGameHost){
			this.id = id;
			this.gameId = gameId;
			this.privateIp = privateIp;
			this.publicIp = publicIp;			
			this.playerName = playerName;
			this.playerPing = playerPing;
			this.isPlayerInGame = isPlayerInGame;			
			this.isGameHost = isGameHost;
		}//MUser		
					
		/***********************************************************/
		
		// Transform the MUser object parameters on a string
		public string UserToString(){
			return id.ToString()+"$"+gameId.ToString()+"$"+privateIp+"$"+publicIp+"$"+playerName+"$"+playerPing.ToString()+"$"+isPlayerInGame+"$"+isGameHost.ToString();
		}//UserToString
		
		// Create an MUser object with parameters from a string
		public static MUser UserToObject(string parseUser){
			string[] values = parseUser.Split('$');
			return new MUser(values[0], values[1], values[2],values[3], values[4], values[5], values[6], values[7]);
		}//parseUser
		
		// Transform a userList (List<MUser>) on a string
		public static string ListToString(List<MUser> userList){
			string values = "";
			for(int i = 0; i < userList.Count; i++){
				values+= userList[i].UserToString();
				if(i < userList.Count-1){
					values+="#";
				}				
			}
			return values;
		}//userList
		
		// Create a userList (List<MUser>) from a string
		public static List<MUser> ListToObject(string parseUser){
			List<MUser> userList = new List<MUser>();
			string[] values = parseUser.Split('#');
			for(int i = 0; i < values.Length; i++){
				userList.Add(UserToObject(values[i]));
			}
			return userList;
		}//ListToObject
		
		// Remove a user from his M - Return the modified userList
		public static List<MUser> RemoveFromId(List<MUser> userList, int gameId){
			MUser removeUser = null;
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
		public static List<MUser> RemoveServer(List<MUser> userList){
			MUser removeUser = null;
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].isGameHost == 1){
					removeUser = userList[i];
				}
			}			
			if(removeUser != null){
				userList.Remove(removeUser);
			}			
			return userList;
		}//RemoveServer
				
		// Save the new status of a player - Return the modified userList
		public static List<MUser> SavePlayerStatus(int gameId, int isPlayerInGame, List<MUser> userList) {
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].gameId == gameId){
					userList[i].isPlayerInGame = isPlayerInGame;
				}
			}	
			return userList;
		}//SavePlayerStatus
				
		// Search a player from his game id (return him as MUser object)
		public static MUser SearchPlayer(int gameId, List<MUser> userList) {
			MUser searchUser = new MUser();
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].gameId == gameId){
					searchUser = userList[i];
				}
			}	
			return searchUser;
		}//SearchPlayer
		
		// Search a player from his id (return him as MUser object)
		public static MUser SearchPlayerId(int id, List<MUser> userList) {
			MUser searchUser = new MUser();
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].id == id){
					searchUser = userList[i];
				}
			}	
			return searchUser;
		}//SearchPlayer
		
		// Check if a specific player is on the userList
		public static bool inList(List<MUser> userList, int gameId){
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].gameId == gameId){
					return true;
				}
			}	
			return false;
		}//userList	
		
		public static List<MUser> PingSort(List<MUser> userList){
			return userList.OrderBy(x=>x.playerPing).ToList();	
		}//PingSort

        public static void PingPlayers(NetworkManager networkSrc)
        {
			networkSrc.PingPlayers();
		}//PingPlayers

        public static string GetHostMessage(int key, NetworkManager networkSrc)
        {
			if(networkSrc.playerList[key].gameId == networkSrc.gameInfo.hostId){
				return "[host]";
			}
			return "";
		}//GetHostMessage

        public static string GetPlayerPing(int key, NetworkManager networkSrc)
        {
			return networkSrc.playerList[key].playerPing.ToString();			
		}//GetPlayerPing
	}//MUser
	
	public class MPlayersPosition{
		public int id;
		public string name;
		public Vector3 position;
		
		public MPlayersPosition(int id, string name, Vector3 position){
			this.id = id;
			this.name = name;
			this.position = position;
		}//MPlayersPosition
		
		public static List<MPlayersPosition> UpdatePosition(List<MPlayersPosition> list, int id, string name, Vector3 position){
			for(int i = 0; i < list.Count; i++){
				if(list[i].id == id){
					list.Remove(list[i]);
				}
			}
			list.Add (new MPlayersPosition(id, name, position));
			return list;
		}//UpdatePosition
		
		public static List<MPlayersPosition> RemoveFromId(List<MPlayersPosition> list, int id){
			for(int i = 0; i < list.Count; i++){
				if(list[i].id == id){
					list.Remove(list[i]);
				}
			}
			return list;
		}//UpdatePosition
		
		// Transform the MPlayersPosition object parameters on a string
		public string PositionToString(){
			return id.ToString()+"$"+name
				+"$"+position.x.ToString()
				+"$"+position.y.ToString()
				+"$"+position.z.ToString();
		}//PositionToString
		
		// Create an MPlayersPosition object with parameters from a string
		public static MPlayersPosition PositionToObject(string parse){
			string[] values = parse.Split('$');
			return new MPlayersPosition(int.Parse(values[0]), values[1], new Vector3(float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4])));
		}//MPlayersPosition
		
		// Transform a list (List<MPlayersPosition>) on a string
		public static string ListToString(List<MPlayersPosition> list){
			string values = "";
			for(int i = 0; i < list.Count; i++){
				values+= list[i].PositionToString();
				if(i < list.Count-1){
					values+="#";
				}				
			}
			return values;
		}//ListToString
		
		// Create a list (List<MPlayersPosition>) from a string
		public static List<MPlayersPosition> ListToObject(string parseList){
			List<MPlayersPosition> positionList = new List<MPlayersPosition>();
			string[] values = parseList.Split('#');
			for(int i = 0; i < values.Length; i++){
				positionList.Add(PositionToObject(values[i]));
			}
			return positionList;
		}//ListToObject
	
	}//MPlayersPosition
}//MultiPlayer

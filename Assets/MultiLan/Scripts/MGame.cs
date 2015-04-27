using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MultiPlayer {
	public class MGame {		
		public int id;
		public string name;
		public string mapName;
		public int mapKey;
		public int port;
		public int totalPlayer;
		public int maxPlayer;	
		public bool isUsePassword;
		public string password;
		public bool isStarted;		
		public string register;		
		public string registerDate;
		public bool isOnline;		
		public bool isPrivate;
		public bool isOnNetwork;
		public bool isOnDedicatedServer;
		
		public int hostId;
		public string hostName;
		public string hostPrivateIp;
		public string hostPublicIp;
		public string hostLocation;
		// MultiLan games ping
		public int ping;
		// MultiOnline games ping
		public Ping OPing;
			
		public MGame(){
			this.id = 0;
		}
		
		// Consctructor for MultiLan
		public MGame(string name,
			int port,
			int totalPlayer,
			int maxPlayer,
			bool isStarted,
			bool isPrivate,
			string hostName,
			int hostId,
			string mapName,
			string hostPrivateIp,
			string hostPublicIp,			
			int ping){
			
			this.name = name; 
			this.port = port;	
			this.totalPlayer = totalPlayer;
			this.maxPlayer = maxPlayer;	
			this.isStarted = isStarted;		
			this.isPrivate = isPrivate;	
			this.name = name;
			this.hostId = hostId;
			this.mapName = mapName; 
			this.hostName = hostName; 
			this.hostPrivateIp = hostPrivateIp; 
			this.hostPublicIp = hostPublicIp; 
			this.ping = ping; 
		}
		
		// Consctructor for MultiOnline
		public MGame(string id,
			string name,
			string mapName,
			string port,
			string totalPlayer,
			string maxPlayer,
			string isUsePassword,
			string status,
			string register,
			string registerDate,
			string hostId,
			string hostName,
			string hostPrivateIp,
			string hostPublicIp,
			string isOnDedicatedServer,
			string playerPublicIp){		
			try{
				this.id = int.Parse(id);	
			} catch(FormatException){}
			this.name = name; 
			this.mapName = mapName;
			try{
				this.port = int.Parse(port);		
			} catch(FormatException){}
			try{
				this.totalPlayer = int.Parse(totalPlayer);
			} catch(FormatException){}
			try{
				this.maxPlayer = int.Parse(maxPlayer);
			} catch(FormatException){}	
			this.isUsePassword = isUsePassword.Equals("1");			
			this.isStarted = status.Equals("1");			
			this.register = register; 
			this.registerDate = registerDate; 
				try{
				this.hostId = int.Parse(hostId); 
			} catch(FormatException){}			
			this.hostName = hostName; 
			this.hostPrivateIp = hostPrivateIp; 
			this.hostPublicIp = hostPublicIp; 
			this.isOnDedicatedServer = isOnDedicatedServer.Equals("0");
			this.isOnline = true;
			string pingIp = "";
			if(playerPublicIp == hostPublicIp) {
				pingIp = hostPrivateIp;
			} else {
				pingIp = hostPublicIp;
			}
			this.OPing = new Ping(pingIp);
		}//MGame
		
		// Consctructor for MNetowrk
		public MGame(int id,
			string name,
			int port,
			string mapName,
			int mapKey,
			int totalPlayer,
			int maxPlayer,
			bool isUsePassword,
			bool isStarted,
			string register,
			string registerDate,
			bool isOnline,
			bool isPrivate,
			bool isOnNetwork,
			bool isOnDedicatedServer,
			int hostId,
			string hostName,
			string hostPrivateIp,
			string hostPublicIp,
			string hostLocation){
			
			this.id = id;
			this.name = name ;
			this.mapName = mapName;
			this.mapKey = mapKey;
			this.port = port;
			this.totalPlayer = totalPlayer;
			this.maxPlayer = maxPlayer;
			this.isUsePassword = isUsePassword;
			this.isStarted = isStarted;
			this.register = register;	
			this.registerDate = registerDate;
			this.isOnline = isOnline;	
			this.isPrivate = isPrivate;
			this.isOnNetwork = isOnNetwork;
			this.isOnDedicatedServer = isOnDedicatedServer;		
			this.hostId = hostId;
			this.hostName = hostName;
			this.hostPrivateIp = hostPrivateIp;
			this.hostPublicIp = hostPublicIp;
			this.hostLocation = hostLocation;
		}//MGame
		
		public MGame(string name, int maxPlayer, int port, bool isUsePassword, string password, bool isOnline, bool isPrivate, bool isOnNetwork, bool isOnDedicatedServer){
			this.name = name;
			this.port = port;
			this.maxPlayer = maxPlayer;
			this.isUsePassword = isUsePassword;
			this.password = password;
			this.isOnline = isOnline;
			this.isPrivate = isPrivate;
			this.isOnNetwork = isOnNetwork;
			this.isOnDedicatedServer = isOnDedicatedServer;
		}//MGame
		
		// Construcotr for create a game form a parsed game
		public MGame(string parseGame){
			string[] val = parseGame.Split('$');
			this.id = int.Parse(val[0]);
			this.name = val[1];
			this.mapName = val[2];
			this.mapKey = int.Parse(val[3]);
			this.port = int.Parse(val[4]);
			this.totalPlayer = int.Parse(val[5]);
			this.maxPlayer = int.Parse(val[6]);
			if(val[7] == "1") { this.isUsePassword= true; } else { this.isUsePassword=false; };
			if(val[8] == "1") { this.isStarted= true; } else { this.isStarted=false; };
			this.register = val[9];
			this.registerDate = val[10];
			if(val[11] == "1") { this.isOnline= true; } else { this.isOnline=false; };
			if(val[12] == "1") { this.isPrivate= true; } else { this.isPrivate=false; };
			if(val[13] == "1") { this.isOnNetwork= true; } else { this.isOnNetwork=false; };
			if(val[14] == "1") { this.isOnDedicatedServer= true; } else { this.isOnDedicatedServer=false; };
			this.hostId = int.Parse(val[15]);
			this.hostName = val[16];
			this.hostPrivateIp = val[17];
			this.hostPublicIp = val[18];
			this.hostLocation = val[19];
		}//MGame
		
		// FilterGames : overload function for ML
		public static List<MGame> FilterGames(List<MGame> gameList, List<int> playersList, List<string> mapList, int ping, bool startedNo, bool startedYes, bool fullGameYes){
			List<MGame> filterGames = new List<MGame>();
			filterGames = gameList.FindAll(
            delegate(MGame g){				
				bool started = false;
				bool filterStarted = false;
				bool error= false;
				bool filterFull = !fullGameYes;
				if (startedNo ^ startedYes) {
					started = startedYes;		
					filterStarted = true;
				} else if(!startedNo && !startedYes){
					error = true;
				}				
				 if (error){
					return false;
				} else if(filterStarted && filterFull){
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isStarted.Equals(started) && g.totalPlayer < g.maxPlayer;
				} else if(filterStarted && !filterFull){
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isStarted.Equals(started);
				} else if(!filterStarted && filterFull){
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.totalPlayer < g.maxPlayer;
				} else {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping;
				}				
			});
			return filterGames;
		}//FilterGames
		
		// FilterGames : overload function for MO
		public static List<MGame> FilterGames(List<MGame> gameList, List<int> playersList, List<string> mapList, int ping, bool startedNo, bool startedYes, bool passNo, bool hostServer, bool hostPlayer, bool passYes, bool fullGameYes){
			List<MGame> filterGames = new List<MGame>();
			filterGames = gameList.FindAll(
            delegate(MGame g){
				bool dedicated = false;
				bool filterDedicated = false;
				bool filterStarted = false;
				bool started = false;	
				bool filterPass = false;
				bool pass = false;
				bool error= false;
				bool filterFull = !fullGameYes;
				if(hostPlayer ^ hostServer){
					dedicated = hostServer;
					filterDedicated = true;
				} else if(!hostPlayer && !hostServer){
					error = true;
				}
				if (startedNo ^ startedYes) {
					started = startedYes;		
					filterStarted = true;
				} else if(!startedNo && !startedYes){
					error = true;
				}				
				if (passNo ^ passYes) {
					pass = passYes;		
					filterPass = true;
				}else if(!passNo && !passYes){
					error = true;
				}
				
				 if (error){
					return false;
				} else if (filterStarted && filterPass && filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isStarted.Equals(started) && g.isUsePassword.Equals(pass) && g.isOnDedicatedServer.Equals(dedicated);
				} else if (filterStarted && filterPass && filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isStarted.Equals(started) && g.isUsePassword.Equals(pass) && g.isOnDedicatedServer.Equals(dedicated) && g.totalPlayer < g.maxPlayer;
				} else if (!filterStarted && filterPass && !filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isUsePassword.Equals(pass);
				} else if (!filterStarted && filterPass && !filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isUsePassword.Equals(pass) && g.totalPlayer < g.maxPlayer;
				} else if (!filterStarted && filterPass && filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isUsePassword.Equals(pass) && g.isOnDedicatedServer.Equals(dedicated);
				} else if (!filterStarted && filterPass && filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping  && g.isUsePassword.Equals(pass) && g.isOnDedicatedServer.Equals(dedicated)  && g.totalPlayer < g.maxPlayer;
				} else if (filterStarted && !filterPass && !filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isStarted.Equals(started);
				} else if (filterStarted && !filterPass && !filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isStarted.Equals(started) && g.totalPlayer < g.maxPlayer;
				} else if (filterStarted && !filterPass && filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isStarted.Equals(started) && g.isOnDedicatedServer.Equals(dedicated);
				} else if (filterStarted && !filterPass && filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isStarted.Equals(started) && g.isOnDedicatedServer.Equals(dedicated) && g.totalPlayer < g.maxPlayer;
				} else if (!filterStarted && !filterPass && filterDedicated && !filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isOnDedicatedServer.Equals(dedicated);
				} else if (!filterStarted && !filterPass && filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.isOnDedicatedServer.Equals(dedicated) && g.totalPlayer < g.maxPlayer;
				} else if (!filterStarted && !filterPass && !filterDedicated && filterFull) {
					return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping && g.totalPlayer < g.maxPlayer;
				}else {
					 return playersList.Contains(g.maxPlayer) && mapList.Contains(g.mapName) && g.ping <= ping;
				}
			});
			return filterGames;
		}//FilterGames
		
		// Sort the game list passed in parameter according the sort parameters (column and type)
		// return the sorted game list
		public static List<MGame> SortGames(List<MGame> list, int sortColumn, int sortType){
			List<MGame> sortedList = new List<MGame>();
			if(sortColumn == 0 && sortType == 0){
				sortedList = list.OrderBy(x=>x.name).ToList();	
			} else if(sortColumn == 0 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.name).ToList();	
			} else if(sortColumn == 1 && sortType == 0){
				sortedList = list.OrderBy(x=>x.hostName).ToList();	
			} else if(sortColumn == 1 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.hostName).ToList();	
			} else if(sortColumn == 2 && sortType == 0){
				sortedList = list.OrderBy(x=>x.mapName).ToList();	
			} else if(sortColumn == 2 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.mapName).ToList();	
			}else if(sortColumn == 3 && sortType == 0){
				sortedList = list.OrderBy(x=>x.totalPlayer).ToList();	
			} else if(sortColumn == 3 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.totalPlayer).ToList();	
			}else if(sortColumn == 4 && sortType == 0){
				sortedList = list.OrderBy(x=>x.isPrivate).ToList();	
			} else if(sortColumn == 4 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.isPrivate).ToList();	
			}else if(sortColumn == 5 && sortType == 0){
				sortedList = list.OrderBy(x=>x.isStarted).ToList();	
			} else if(sortColumn == 5 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.isStarted).ToList();	
			}else if(sortColumn == 6 && sortType == 0){
				sortedList = list.OrderBy(x=>x.ping).ToList();	
			} else if(sortColumn == 6 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.ping).ToList();	
			} else if(sortColumn == 7 && sortType == 0){
				sortedList = list.OrderBy(x=>x.OPing.time).ToList();	
			} else if(sortColumn == 7 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.OPing.time).ToList();	
			} else if(sortColumn == 8 && sortType == 0){
				sortedList = list.OrderBy(x=>x.isUsePassword).ToList();	
			} else if(sortColumn == 8 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.isUsePassword).ToList();	
			} else if(sortColumn == 9 && sortType == 0){
				sortedList = list.OrderBy(x=>x.isOnDedicatedServer).ToList();	
			} else if(sortColumn == 9 && sortType == 1){
				sortedList = list.OrderByDescending(x=>x.isOnDedicatedServer).ToList();	
			} else {
				sortedList = list.OrderBy(x=>x.name).ToList();	
			}
			return sortedList;
		}//SortGames
			
		public static List<MGame> SearchGame(List<MGame> list, string search){
			List<MGame> searchList = new List<MGame>();
			search = search.Trim();
			for(int i = 0; i < list.Count; i++){
				if(list[i].name.ToLower().Equals(search.ToLower()) || list[i].hostName.ToLower().Equals(search.ToLower())){
					searchList.Add(list[i]);
				} 			
			}
			return searchList;
		}//SearchGame
			
		// Transform the MUser object parameters on a string
		public string GameToString(){
			string pass = "0";
			if(isUsePassword){
				pass="1";
			}
			
			string start = "0";
			if(isStarted){
				start="1";
			}
			
			string onlineGame = "0";
			if(isOnline){
				onlineGame="1";
			}
			
			string privateGame = "0";
			if(isPrivate){
				privateGame="1";
			}
			
			string onNetworkGame = "0";
			if(isOnNetwork){
				onNetworkGame="1";
			}
			
			string dedicated = "0";
			if(isOnDedicatedServer){
				dedicated="1";
			}
				return id.ToString()
					+"$"+name
					+"$"+mapName
					+"$"+mapKey.ToString()
					+"$"+port.ToString()
					+"$"+totalPlayer.ToString()
					+"$"+maxPlayer.ToString()
					+"$"+pass
					+"$"+start
					+"$"+register
					+"$"+registerDate
					+"$"+onlineGame
					+"$"+privateGame
					+"$"+onNetworkGame
					+"$"+dedicated
					+"$"+hostId.ToString()
					+"$"+hostName
					+"$"+hostPrivateIp
					+"$"+hostPublicIp
					+"$"+hostLocation;
		}//GameToString	
			
		public void SetPing(int ping){
			this.ping = ping;
		}//SetPing
		
		public void SetMap(string mapName, int mapKey){
			this.mapName = mapName;
			this.mapKey = mapKey;
		}//SetMap
		
	}//MGame

	
	// Excusions games structure
	public struct MExcusionGame{
		public string hostPublicIp;
		public string hostPrivateIp;
		public int port;
		
		public MExcusionGame(string hostPublicIp, string hostPrivateIp, int port){
			this.hostPublicIp = hostPublicIp;
			this.hostPrivateIp = hostPrivateIp;
			this.port = port;
		}//MExcusionGame
	}//MExcusionGame
	
}//MultiPlayer
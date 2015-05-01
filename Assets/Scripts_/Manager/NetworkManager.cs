using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;

public class NetworkManager : MonoBehaviour
{
    // Global parameters
    public string mainMenuName = "Lobby";
    public string waitRoomName = "Room";
    public string networkMessage = "";

    // SCRIPTS AND OBJECTS 
    public MMenu menuSrc;
    public MGameParameter parameters;

    // Game parameters
    public MGame gameInfo;
    public bool isGameServerRebuild = false;
    public bool isGameServerWaitingPlayers = false;
    public bool isGameServerRebuildSuccess = false;
    public bool isGameServerRebuildFailed = false;
    public bool isGameHost;

    // Player parameters	
    public bool isSearchGame = false;
    public bool isPlayerExitGame = false;
    public Vector3 playerPrefabPosition;
    public Quaternion playerPrefabRotation;
    private GameObject playerPrefab;

    // Chat (waitroom and gamechat)
    public List<String> chatContent;

    // Playerlist
    public List<User> playerList;
    private List<User> playerMigrationList;

    // Player exclusion
    public bool isExculdePlayer = false;
    public bool isPlayerExculde = false;
    public User excludedPlayer;
    public NetworkPlayer excludedNetworkPlayer;
    public bool alwaysExcudePlayer = false;

    // FrienList
    public bool isFriendPlayer = false;
    public bool isFriendAsk = false;
    public bool isFriendAskYes = false;
    public bool isFriendAskNo = false;
    public User friendPlayer;
    public User friendAskPlayer;

    //Dedicated
    public bool isDedicatedServerBreakDown = false;


    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("NetworkManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<NetworkManager>();
                _instance.InitNetworkManager();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private static NetworkManager _instance;

    public NetworkView networkView;

    private void InitNetworkManager()
    {
        networkView = this.gameObject.AddComponent<NetworkView>();
        networkView.stateSynchronization = NetworkStateSynchronization.Off;
        networkView.observed = this.transform;
        networkView.viewID = new NetworkViewID ();
        gameInfo = new MGame();
    }

    public static void LoadNetworkManager()
    {

    }

    public static void DestroyNetworkManager()
    {
        Application.LoadLevel("Lobby");
        DestroyImmediate(NetworkManager.Instance.gameObject);
    }

    //------------------- BASIC FUNCTIONS  ------------------ //	
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        if (_instance != this)
        {
            DestroyImmediate (this.gameObject);
        }

        DontDestroyOnLoad(this);
        playerList = new List<User>(); 

        Network.sendRate = 30;
    }

    void Start()
    {
        menuSrc = GameObject.Find("Menu").GetComponent<MMenu>();
        parameters = GameObject.Find("LevelOption").GetComponent<MGameParameter>();

        chatContent = new List<string>();
    }

    //---BASIC FUNCTIONS
    //------------------------------------------------------
    //------------------- EVENTS FUNCTIONS  ------------------ //	
    void OnServerInitialized()
    {
        // Save host parameters
        isGameHost = true;
        // Server save his id around the game
        GameManager.Instance.cPlayer.id = int.Parse(Network.player.ToString());
        this.gameInfo.totalPlayer = 1;

        // Add server to playerList
        bool inGame = false;
        if (GameManager.Instance.cPlayer.isPlayerInGame)
        {
            inGame = true;
        }

        playerList.Clear(); // Clear the playerList 
        // Save the host on the playerList
        playerList.Add(new User(GameManager.Instance.cPlayer.id, 
            GameManager.Instance.cPlayer.id,
        GameManager.Instance.cPlayer.privateIp, 
        GameManager.Instance.cPlayer.publicIp,
        "true", 
        0, 
        inGame, 
        true));
        StartGame(false); // Try to start the game 
    }//OnServerInitialized	

    // OnFailedToConnect : call on the client
    void OnFailedToConnect(NetworkConnectionError error)
    {
        if (!isSearchGame)
        {
            if (isGameServerRebuild)
            {
                return;
            }
            else
            {
                string errorMsg = "";
                if (error == NetworkConnectionError.TooManyConnectedPlayers)
                {
                    errorMsg = menuSrc.text.nmErrorMaxPlayers;
                }
                else if (error == NetworkConnectionError.InvalidPassword)
                {
                    errorMsg = menuSrc.text.nmErrorPassword;
                }
                else if (error == NetworkConnectionError.AlreadyConnectedToServer)
                {
                    errorMsg = menuSrc.text.nmErrorAlreadyConnect;
                }
                else if (error == NetworkConnectionError.AlreadyConnectedToAnotherServer)
                {
                    errorMsg = menuSrc.text.nmErrorToAnotherServer;
                }
                menuSrc.networkJoinMessage[0] = gameInfo.isOnline.ToString();
                menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorConnexion + errorMsg;
                Destroy(gameObject);
            }
        }
        else if (isSearchGame && menuSrc.useLan)
        { // If we are searching games 
            // This IP has no game, so :		
            menuSrc.LanGetNetworkGames(false); // Go to next IP	
        }

    }//OnFailedToConnect

    // OnPlayerConnected : call on the server
    void OnPlayerConnected(NetworkPlayer player)
    {
        GetComponent<NetworkView>().RPC("DisabledPlayers", player);
    }//OnPlayerConnected	

    // OnPlayerDisconnected : call on the server
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (User.inList(playerList, int.Parse(player.ToString())))
        {
            gameInfo.totalPlayer--;
            GetComponent<NetworkView>().RPC("RefreshPlayerCount", RPCMode.All, gameInfo.totalPlayer);
            
            playerList = User.RemoveFromId(playerList, int.Parse(player.ToString()));
            playerList = User.PingSort(playerList);	// Sort the list by Ping
            GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, User.ListToString(playerList));
            GetComponent<NetworkView>().RPC("RemoveInPositionList", RPCMode.Others, int.Parse(player.ToString()));
        }

        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }//OnPlayerDisconnected

    // OnConnectedToServer : call on the client
    void OnConnectedToServer()
    {
        if (!isSearchGame)
        {	 // If we just searching games 
            // Client save his id around the game
            GameManager.Instance.cPlayer.id = int.Parse(Network.player.ToString());
            networkView.RPC("AddPlayer", RPCMode.Server,
                GameManager.Instance.cPlayer.id,
                GameManager.Instance.cPlayer.gameId,
                true,
                GameManager.Instance.cPlayer.privateIp,
                GameManager.Instance.cPlayer.publicIp,
                gameInfo.isOnNetwork,
                GameManager.Instance.cPlayer.isPlayerInGame,
                false, Network.player);
        }
        else
        {
            // Else, if the client is just searching a game : call SearchGame on the server
            GetComponent<NetworkView>().RPC("SearchGameInfo", RPCMode.Server, Network.player);
        }
    } //OnConnectedToServer


    // OnDisconnectedFromServer : call on the client
    void OnDisconnectedFromServer()
    {
        // If we are not just searching a game or if we have not been exculded 
        if (!isSearchGame && !isPlayerExculde && Network.peerType != NetworkPeerType.Server)
        {
            // All the game will be rebuilt with a new host (if we find one)			
            gameInfo.totalPlayer = 0;// Put the total player on 0	
            CleanGame(); // Clean the game (remove all players prefab) (they will be re-loaded on the new host)				
            playerList = User.RemoveServer(playerList);// Remove server from playerList (because he have leave the game)
            if (gameInfo.isOnDedicatedServer && !isPlayerExitGame)
            {
                isDedicatedServerBreakDown = true;
            }
            NetworkManager.DestroyNetworkManager();
        }
    }//OnDisconnectedFromServer

    // OnDestroy : call when we destroy the current gameObject
    void OnDestroy()
    {
        CleanGame(); // Clean the game (remove all players prefab) (they will be re-loaded on the new host)	
        DestroyGameMenu();
    }//OnDestroy

    // OnLevelWasLoaded 
    void OnLevelWasLoaded()
    {
        // save the playerPrefab in local variable
        if (Application.loadedLevelName != waitRoomName &&
            Application.loadedLevelName != "Lobby")
        {
            if (!gameInfo.isStarted)
            {
                chatContent.Clear(); // Clear the chat (because it contains the waitroom Thread and timer)
            }
            EnabledPlayer(); // Enabled the render of the other players
            if (GameObject.Find("Spawns") != null)
            {
                MSpawn spawnSrc = GameObject.Find("Spawns").GetComponent<MSpawn>();
                playerPrefab = spawnSrc.playerPrefab;
            }

            gameInfo.isStarted = true;
            GameManager.Instance.cPlayer.isPlayerInGame = true;

            // Save the server new statut (in game)
            if (isGameHost)
            {
                playerList = User.SavePlayerStatus(GameManager.Instance.cPlayer.gameId, true, playerList);
                // Send my new status to the clients
                GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, User.ListToString(playerList));
            }
            else
            {
                // Send to the server that I'm now in game
                GetComponent<NetworkView>().RPC("RefreshPlayerStatus", RPCMode.Server, GameManager.Instance.cPlayer.gameId, true);
            }
        }
    }//OnLevelWasLoaded		

    //---EVENTS FUNCTIONS
    //------------------------------------------------------
    //------------------- PUBLIC FUNCTIONS  ------------------ //	
    public void StartServer(int id, string name, int maxPlayer, int port, bool usePass, string pass, string register, string registerDate, bool isPrivate, bool isOnline, bool isOnNetwork, bool isOnDedicatedServer)
    {
        if (usePass)
        {
            Network.incomingPassword = pass;
        }
        NetworkConnectionError error = Network.InitializeServer(maxPlayer, port, false);
        if (error != NetworkConnectionError.NoError)
        {
            menuSrc.networkCreateMessage[0] = isOnline.ToString();
            menuSrc.networkCreateMessage[1] = menuSrc.text.nmErrorGameCreation;
            if (error == NetworkConnectionError.CreateSocketOrThreadFailure)
            {
                menuSrc.networkCreateMessage[1] += menuSrc.text.nmErrorUsedPort;
            }
            else if (error == NetworkConnectionError.InvalidPassword)
            {
                menuSrc.networkCreateMessage[1] += menuSrc.text.nmErrorPassword;
            }
            else if (error == NetworkConnectionError.AlreadyConnectedToAnotherServer)
            {
                menuSrc.networkCreateMessage[1] += menuSrc.text.nmErrorToAnotherServer;
            }
        }
        else
        {
            this.gameInfo = new MGame(id, name, port, parameters.maps[0], 0, 1, maxPlayer, usePass,
            false, register, registerDate, isOnline, isPrivate, isOnNetwork, isOnDedicatedServer,
            int.Parse(Network.player.ToString()), GameManager.Instance.cPlayer.name, GameManager.Instance.cPlayer.privateIp, GameManager.Instance.cPlayer.publicIp, null);
        }
    }//StartServer


    public void JoinServer(string ip, int port, bool isGameUsePassword, string password, bool isOnNetwork, bool isOnlineGame)
    {
        if (ip != "" && !port.Equals(null))
        {
            gameInfo.isOnNetwork = isOnNetwork;
            if (isOnlineGame)
            {
                gameInfo.isOnline = true;
            }
            if (isGameUsePassword)
            {
                Network.Connect(ip, port, password);
            }
            else
            {
                Network.Connect(ip, port);
            }
        }
    }//Join Server

    public void SearchGame(int port, string ip)
    {
        isSearchGame = true;
        Network.Connect(ip, port);
    }//SearchGame

    public void StartGame(bool canLoadGame)
    {
        if (isGameServerRebuildSuccess)
        { // If server has been rebuilt	
            if (gameInfo.isStarted)
            {
                LoadPlayer(); // Load the player on map	
            }
            isGameServerRebuildSuccess = false;
        }
        else if (canLoadGame)
        { // else if the game is ready to be load	
            if (Application.loadedLevelName != gameInfo.mapName)
            { // If the map is not already load
                Application.LoadLevel(gameInfo.mapName);
            }
        }
        else if (!GameManager.Instance.cPlayer.isPlayerInGame)
        { // Else, load the waitroom
            if (Application.loadedLevelName != waitRoomName && waitRoomName != "" && waitRoomName != null)
            { // If the map is not already load
                Application.LoadLevel("Room");
            }
        }
    }//StartGame	

    public void PingPlayers()
    {
        if (playerList.Count > 1)
        { // If the list is not emtpy :			
            for (int i = 0; i < Network.connections.Length; i++)
            {
                for (int j = 0; j < playerList.Count; j++)
                {
                    if (Network.connections[i].ToString().Equals(playerList[j].gameId.ToString()))
                    {
                        playerList[j].playerPing = Network.GetAveragePing(Network.connections[i]);
                    }
                }
            }
        }

    }//PingPlayers

    public NetworkPlayer SearchNetworkPlayer(int gameId)
    {
        for (int i = 0; i < Network.connections.Length; i++)
        {
            if (Network.connections[i].ToString().Equals(gameId.ToString()))
            {
                return Network.connections[i];
            }
        }
        return new NetworkPlayer();
    }//SearchNetworkPlayer

    // Chat function : call from the waitRoom or from the gameChat form sycronise chat
    public void SendChatMessage(string message)
    {
        GetComponent<NetworkView>().RPC("ChatMessage", RPCMode.All, message);
    }//SendChatMessage


    // Destroy all player GameObject (because they will be reloaded)
    public void CleanGame()
    {
        GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < gameArray.Length; i++)
        {
            if (gameArray[i].name == "Player")
            {
                GameObject.Destroy(gameArray[i]);
            }
        }
    }//CleanGame	

    public void DestroyGameMenu()
    {
        try
        {
            Destroy(GameObject.Find("MGameMenu"));
        }
        catch { }
    }

    // When my player spwaned 
    public void PlayerSpawned()
    {
        // Send DisabledPlayers to all other players, so that players in waitroom can disabled my player
        GetComponent<NetworkView>().RPC("DisabledPlayers", RPCMode.Others);
        gameInfo.isStarted = true;
        GameManager.Instance.cPlayer.isPlayerInGame = true;
    }//PlayerSpawned

    public void ExitGame(bool onlineLogout)
    {
        Network.Disconnect();  // Disconnect from the network
        GameManager.Instance.cPlayer.isPlayerInGame = false; // Put isInGame at false
        Destroy(gameObject); // Desrtoy the NetworkManager
        Application.LoadLevel("Lobby"); // Load the main menu	

        GameManager.Instance.cPlayer.ExitGame();
    }//ExitGame	


    //---PUBLIC FUNCTIONS
    //------------------------------------------------------
    //------------------- PRIVATE FUNCTIONS  ------------------ //

    private void LoadPlayer()
    {
        // Load the player in his previous position and rotation	
        try
        {
            Network.Instantiate(playerPrefab, playerPrefabPosition, playerPrefabRotation, 0);
        }
        catch (NullReferenceException)
        {
            return;
        }
    }


    //  DISABLED / ENABLED player 
    private void DisabledPlayer()
    {
        if (!GetComponent<NetworkView>().isMine)
        {
            GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < gameArray.Length; i++)
            {
                if (gameArray[i].name == "Player")
                {
                    // Make player invisible : disabled renderer on him and his childrens
                    gameArray[i].GetComponent<Renderer>().enabled = false;
                    gameArray[i].GetComponent<Rigidbody>().isKinematic = false; // Disable the physic on the playes
                    Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
                    foreach (Transform child in chidrens)
                    {
                        if (child.GetComponent<Renderer>() != null)
                        {
                            child.GetComponent<Renderer>().enabled = false;
                        }
                    }
                }
            }
        }
    }//DisabledPlayer

    private void EnabledPlayer()
    {
        if (!GetComponent<NetworkView>().isMine)
        {
            GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < gameArray.Length; i++)
            {
                if (gameArray[i].name == "Player")
                {
                    // Make player visible : enabled renderer on him and his childrens
                    gameArray[i].GetComponent<Renderer>().enabled = true;
                    gameArray[i].GetComponent<Rigidbody>().isKinematic = true; // Disable the physic on the playes, since we don't want that they move alone
                    Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
                    foreach (Transform child in chidrens)
                    {
                        if (child.GetComponent<Renderer>() != null)
                        {
                            child.GetComponent<Renderer>().enabled = true;
                        }
                    }
                    // Ask player position to the server
                    GetComponent<NetworkView>().RPC("AskPosition", RPCMode.Server, gameArray[i].GetComponent<NetworkView>().viewID, Network.player);
                }
            }
        }
    }//EnabledPlayer
    //---PRIVATE FUNCTIONS  	
    //------------------------------------------------------
    //------------------- RPC FUNCTIONS  ------------------ //
    // Call by the client to the server to received game informations
    [RPC]
    void SearchGameInfo(NetworkPlayer player)
    {
        // If it's not an online game
        if (!gameInfo.isOnline)
        {
            // Go search the games informations
            GetComponent<NetworkView>().RPC("GetGameInfos", player, gameInfo.GameToString(), false);
        }
        else
        { // Else			
            // Call GoNextGame to the client
            GetComponent<NetworkView>().RPC("GoNextGame", player);
        }
    }//SearchGameInfo

    // Call by the player, to the server, to be add on the playerList	
    [RPC]
    void AddPlayer(int id, int gameId, string playerName, string privateIp, string publicIp, bool isOnNetwork, bool isPlayerInGame, bool isPlayerOnline, NetworkPlayer player)
    {
        // If the game is private and that we are not on the network of the host
        // (check by comparing public ip)
        if (gameInfo.isPrivate && publicIp != GameManager.Instance.cPlayer.publicIp && !isOnNetwork)
        {
            GetComponent<NetworkView>().RPC("CantJoinGame", player, 2); // We cannont join the game
            return; // Exit the function here
        }

        // If the game is Online but the player is not connected
        if (gameInfo.isOnline && !isPlayerOnline)
        {
            GetComponent<NetworkView>().RPC("CantJoinGame", player, 3); // We cannont join the game
            return; // Exit the function here
        }
        // If the game is not yet started or if we don't use the wait room
        //or if the player is already on game = if the server has been rebuild
        if (!gameInfo.isStarted || isPlayerInGame || parameters.canJoinStartedGame)
        {
            gameInfo.totalPlayer++;	// Increments player count				
            bool inGame = false;
            if (isPlayerInGame)
            {
                inGame = true;
            }
            // Add the new player on the playerList
            playerList.Add(new User(id, gameId, privateIp, publicIp, playerName, Network.GetAveragePing(player), inGame, false));
            // Sort the list by Ping
            playerList = User.PingSort(playerList);

            // Refresh the lists of the clients : send on everybody except the server
            GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, User.ListToString(playerList));

            // Send the game informations to the new player
            GetComponent<NetworkView>().RPC("GetGameInfos", player, gameInfo.GameToString(), true);

            // Send the game state and try to start game
            GetComponent<NetworkView>().RPC("RefreshPlayerCount", RPCMode.Others, gameInfo.totalPlayer);

        }
        else
        {
            // Else, we cannot join the game : 
            GetComponent<NetworkView>().RPC("CantJoinGame", player, 1);
        }
    }//AddPlayer	
   
    // Call by the server on the client who asked it
    [RPC]
    void GetGameInfos(string gameInfo, bool startGame)
    {
        this.gameInfo = new MGame(gameInfo);
        // If we are allowed to start the game
        if (startGame)
        {
            // If the game is not yet started 	
            if (!this.gameInfo.isStarted || parameters.canJoinStartedGame || isGameServerRebuildSuccess)
            {
                StartGame(false); // Try to start game
            }
        }
        // If we are searching parties
        if (isSearchGame && menuSrc.useLan)
        {
            // We have now every informations we need, so :
            // This IP has no game, so :
            // This IP has no game, so :	
            menuSrc.LanGetNetworkGames(false); // Go to next IP			
        }
    }//GetGameInfos

    // Call by the server to all clients when the list has changed
    [RPC]
    void RefreshUserList(string list)
    {
        playerList = User.ListToObject(list);// Save the new list		
    }//RefreshUserList
                
    // Call by ther server to all clients when a player quit the game
    [RPC]
    void RefreshPlayerCount(int totalPlayer)
    {
        gameInfo.totalPlayer = totalPlayer;
    }//RefreshPlayerCount
        
    //By by the server on the client when a fall on an Online game when he searchs network games
    [RPC]
    void GoNextGame()
    {
        if (menuSrc.useLan)
        {
            menuSrc.LanGetNetworkGames(false); // Go to next IP		
        }
    }//GoNextGame

    // Call by the server to the client, when the connexion is started
    [RPC]
    void DisabledPlayers()
    {
        // If I'm not in game : disabled the rendrer of the other players
        // (Else I can see them before have load the game level)
        if (!GameManager.Instance.cPlayer.isPlayerInGame)
        {
            DisabledPlayer();
        }
    }//DisabledPlayers

    // Send to the client, when he cannot join the game
    [RPC]
    void CantJoinGame(int index)
    {
        CleanGame(); // Delete all players on the client scene

        // Define a message
        menuSrc.networkJoinMessage[0] = gameInfo.isOnline.ToString();
        if (index == 1)
        {
            menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorStartedGame;
        }
        else if (index == 2)
        {
            menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorPrivateGame;
        }
        else if (index == 3)
        {
            menuSrc.networkJoinMessage[1] = menuSrc.text.nmErrorOnlineGame;
        }
        Destroy(gameObject); // Destroy the client network GameObject
    }//CantJoinGame

    // Call on ererybody : send message from chat on waitRoom or gameChat
    [RPC]
    void ChatMessage(string message)
    {
        // Add the message on the messages list
        chatContent.Add(message);

        // Make the chat scroll down
        try
        {
            // Search if the waiting room is open
            MWaitRoom waitRoomScr = GameObject.Find("WaitRoom").GetComponent<MWaitRoom>();
            if (waitRoomScr != null)
            {
                // Update the waiting room chat scroll
                waitRoomScr.chatScroll = new Vector2(0, (20 * (chatContent.Count + 1)) - waitRoomScr.chatBox.height);
            }
        }
        catch (NullReferenceException)
        {
            try
            {
                // Search if the game chat is open
                MGameChat gameChatSrc = GameObject.Find("MGameMenu").GetComponent<MGameChat>();
                if (gameChatSrc != null)
                {
                    // Update the game chat scroll
                    gameChatSrc.chatScroll = new Vector2(0, (20 * (chatContent.Count + 1)) - gameChatSrc.chatContentSizeY);
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
    }//ChatMessage

    // Call by the server to the other players when the server is well rebuid
    [RPC]
    void RebuildSuccess()
    {
        // Put all the rebuild parameter on the good value
        isGameServerRebuildSuccess = true;
        isGameServerRebuild = false;
        isGameServerWaitingPlayers = false;
        isGameServerRebuildFailed = false;
    }//RebuildSuccess

    // Call by a client to ther server, when a load the game
    [RPC]
    void RefreshPlayerStatus(int playerId, bool isPlayerInGame)
    {
        // Save the new status of the player
        playerList = User.SavePlayerStatus(playerId, isPlayerInGame, playerList);
        // Call RefreshUserList on the other players
        GetComponent<NetworkView>().RPC("RefreshUserList", RPCMode.Others, User.ListToString(playerList));
    }//RefreshPlayerStatus
        
    //---RPC FUNCTIONS
}

using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class MultiplayerLobbyManager : NetworkLobbyManager 
{
	public static MultiplayerLobbyManager Instance;

	public static Action<NetworkConnection> OnClientConnectAction;
	public static Action<NetworkConnection> OnClientDisconnectAction;
	public static Action<NetworkConnection> OnServerConnectAction;
	public static Action<NetworkConnection> OnServerDisconnectAction;

	private ulong _currentMatchID;
	public ulong CurrentMatchID {
		get {
			return _currentMatchID;
		}
		set {
			_currentMatchID = value;
		}
	}

	void Start ()
	{
		Instance = this;
	}

	public void JoinServer(string ip, int port)
	{
		this.networkAddress = ip;
		this.matchPort = port;
		this.StartClient();
	}

	public void CreateServer()
	{
		this.StartHost ();
	}

	public override void OnStartHost()
	{
		base.OnStartHost();
		SceneManager.Instance.LoadLobbyScreen ();
	}

	public void OnDestroyMatch(BasicResponse extendedInfo)
	{
		StopMatchMaker();
		StopHost();
		SceneManager.Instance.LoadFindRoomScreen ();
	}

	// ----------------- Server callbacks ------------------

	//we want to disable the button JOIN if we don't have enough player
	//But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
	public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId)
	{
		GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

		Player newPlayer = obj.GetComponent<Player>();

		for (int i = 0; i < lobbySlots.Length; ++i)
		{
			Player p = lobbySlots[i] as Player;
			if (p != null)
			{
//				p.RpcUpdateRemoveButton();
//				p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
			}
		}

		return obj;
	}

	public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
	{
		for (int i = 0; i < lobbySlots.Length; ++i)
		{
			Player p = lobbySlots[i] as Player;

			if (p != null)
			{
//				p.RpcUpdateRemoveButton();
//				p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
			}
		}
	}

	public override void OnLobbyServerDisconnect(NetworkConnection conn)
	{
		for (int i = 0; i < lobbySlots.Length; ++i)
		{
			Player p = lobbySlots[i] as Player;

			if (p != null)
			{
//				p.RpcUpdateRemoveButton();
//				p.ToggleJoinButton(numPlayers >= minPlayers);
			}
		}
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		//This hook allows you to apply state data from the lobby-player to the game-player
		//just subclass "LobbyHook" and add it to the lobby object.

//		if (_lobbyHooks)
//			_lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

		return true;
	}

	// --- Countdown management

	public override void OnLobbyServerPlayersReady()
	{
		bool allready = true;
		for(int i = 0; i < lobbySlots.Length; ++i)
		{
			if(lobbySlots[i] != null)
				allready &= lobbySlots[i].readyToBegin;
		}

		if(allready)
			StartCoroutine(ServerCountdownCoroutine());
	}

	public IEnumerator ServerCountdownCoroutine()
	{
		float remainingTime = GameManager.COUNTDOWN_TIME;
		int floorTime = Mathf.FloorToInt(remainingTime);

		while (remainingTime > 0)
		{
			yield return null;

			remainingTime -= Time.deltaTime;
			int newFloorTime = Mathf.FloorToInt(remainingTime);

			if (newFloorTime != floorTime)
			{//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
				floorTime = newFloorTime;

				for (int i = 0; i < lobbySlots.Length; ++i)
				{
					if (lobbySlots[i] != null)
					{//there is maxPlayer slots, so some could be == null, need to test it before accessing!
						(lobbySlots[i] as Player).RpcUpdateCountdown(floorTime);
					}
				}
			}
		}

		for (int i = 0; i < lobbySlots.Length; ++i)
		{
			if (lobbySlots[i] != null)
			{
				(lobbySlots[i] as Player).RpcStartGamePlay();
			}
		}

		ServerChangeScene(playScene);
	}

	// ----------------- Client callbacks ------------------

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);

		Debug.Log ("OnClientConnect : " + conn.address);
		if (OnClientConnectAction != null)
			OnClientConnectAction (conn);
	}


	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);

		Debug.Log ("OnClientDisconnect : " + conn.address);
		if (OnClientDisconnectAction != null)
			OnClientDisconnectAction (conn);
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		Debug.LogError("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()));
	}

	#region network_callbacks
		   
	public override void OnClientNotReady (NetworkConnection conn)
	{
		Debug.Log ("OnClientNotReady : " + conn.address);
	}
		   
	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		Debug.Log ("OnClientSceneChanged : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		Debug.Log ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		Debug.Log ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerConnect (NetworkConnection conn)
	{
		Debug.Log ("OnServerConnect : " + conn.address);
		if (OnServerConnectAction != null)
			OnServerConnectAction (conn);
	}
		   
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.Log ("OnServerDisconnect : " + conn.address);
		if (OnServerDisconnectAction != null)
			OnServerDisconnectAction (conn);
	}
		   
	public override void OnServerError (NetworkConnection conn, int errorCode)
	{
		Debug.Log ("OnServerError : " + conn.address);
	}
		   
	public override void OnServerReady (NetworkConnection conn)
	{
		Debug.Log ("OnServerReady : " + conn.address);
	}
		   
	public override void OnServerRemovePlayer (NetworkConnection conn, PlayerController player)
	{
		Debug.Log ("OnServerRemovePlayer : " + conn.address);
	}
		   
	public override void OnServerSceneChanged (string sceneName)
	{
		Debug.Log ("OnServerSceneChanged : " + sceneName);
	}
		   
	public override void OnStartClient (NetworkClient client)
	{
		Debug.Log ("OnStartClient.");
	}
		   
	public override void OnStartServer ()
	{
		Debug.Log ("OnStartServer.");
	}
		   
	public override void OnStopClient ()
	{
		Debug.Log ("OnStopClient.");
	}
		   
	public override void OnStopHost ()
	{
		Debug.Log ("OnStopHost");
	}
		   
	public override void OnStopServer ()
	{
		Debug.Log ("OnStopServer.");
	}
	#endregion network_callbacks
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkDiscovery))]
public class LocalNetworkDiscovery : MonoBehaviour
{
	public static LocalNetworkDiscovery Instance;

	private int m_portNumber;
	public int PortNumber
	{
		get {
			return m_portNumber;
		}
		set {
			m_portNumber = value;
			networkDiscovery.broadcastPort = m_portNumber;
		}
	}

	public RoomUIController findServerUIController;

	private NetworkDiscovery networkDiscovery;
	private Dictionary <string, NetworkBroadcastResult> broadCastResult;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} 
		else {
			Destroy (this.gameObject);
			return;
		}

		Init ();
	}

	void Init ()
	{
		networkDiscovery = gameObject.GetComponent<NetworkDiscovery> ();
		networkDiscovery.Initialize ();
		networkDiscovery.useNetworkManager = true;
		InvokeRepeating ("RefreshServer", 0, 5);
	}

	void Start ()
	{
		if (networkDiscovery != null) {
			PortNumber = networkDiscovery.broadcastPort;
		}
		MultiplayerLobbyManager.OnClientConnectAction = OnClientConnect;
		MultiplayerLobbyManager.OnServerConnectAction = OnServerConnect;
		MultiplayerLobbyManager.OnClientDisconnectAction = OnClientDisconnect;
		MultiplayerLobbyManager.OnServerDisconnectAction = OnServerDisConnect;

		GameManager.playerType = GameManager.PlayerType.None;
	}

	public void RefreshServer ()
	{
		if (findServerUIController != null) 
		{
			broadCastResult = networkDiscovery.broadcastsReceived;
			findServerUIController.CreateServerList (broadCastResult);
		} 
		else {
			CancelInvoke ("RefreshServer");
		}
	}
	
	public void StartAsServer (int port)
	{
		PortNumber = port;
		if (!networkDiscovery.isServer)
		{
			if (networkDiscovery.running)
				networkDiscovery.StopBroadcast ();
			networkDiscovery.Initialize ();
			networkDiscovery.StartAsServer ();
			MPLobbyManager.Instance.CreateServer();
			GameManager.playerType = GameManager.PlayerType.Master;
		}
	}

	public void StartAsClient ()
	{
		if (this == null)
			return;
		if (!networkDiscovery.isClient)
		{
			if (networkDiscovery.running)
				networkDiscovery.StopBroadcast ();
			networkDiscovery.Initialize ();
			networkDiscovery.StartAsClient ();
		}
		CancelInvoke ("RefreshServer");
		InvokeRepeating ("RefreshServer", 0, 5);
	}

	public void ConnectToServer (string ip)
	{
		if (networkDiscovery.running)
			networkDiscovery.StopBroadcast ();
		MPLobbyManager.Instance.JoinServer(ip, PortNumber);
		GameManager.playerType = GameManager.PlayerType.Client;
	}

	public void OnClientConnect (NetworkConnection conn)
	{
		if (!NetworkServer.active) {
			//SceneManager.Instance.LoadLobbyScreen ();
			RoomUIController.Instance.UpdatePanelState (RoomUIController.State.Lobby);
			//MultiplayerLobbyManager.Instance.TryToAddPlayer ();
		}
	}

	public void OnServerConnect (NetworkConnection conn)
	{
		
	}

	public void OnClientDisconnect (NetworkConnection conn)
	{
		//Erase client related info here.

	}

	public void OnServerDisConnect (NetworkConnection conn)
	{		
		SceneManager.Instance.LoadLobbyScreen ();
	}

	void OnDestroy ()
	{
		CancelInvoke ();
	}
}
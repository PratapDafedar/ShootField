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

	public FindCreateRoomUIController findServerUIController;

	private NetworkDiscovery networkDiscovery;
	private Dictionary <string, NetworkBroadcastResult> broadCastResult;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} 
		else {
			Destroy (Instance);
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

	public void RefreshServer ()
	{
		broadCastResult = networkDiscovery.broadcastsReceived;
		findServerUIController.CreateServerList (broadCastResult);
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

			NetworkManager.singleton.StartHost();
		}
	}

	public void StartAsClient ()
	{
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
		NetworkManager.singleton.networkAddress = ip;
		NetworkManager.singleton.networkPort = m_portNumber;
		NetworkManager.singleton.StartClient();
	}

	public void OnSuccessfulConnect (NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}
}
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Collections;

public class FindCreateRoomUIController : MonoBehaviour 
{
	List<MatchDesc> matchList = new List<MatchDesc>();
	bool matchCreated;
	NetworkMatch networkMatch;

	public bool isAtStartup = true;
	NetworkClient myClient;
	
	void Awake()
	{
		networkMatch = gameObject.AddComponent<NetworkMatch>();
	}

	void Start () 
	{
		RefreshFindServer ();
	}
	
	public void RefreshFindServer ()
	{

	}

	public void CreateServer ()
	{

	}

	// Create a server and listen on a port
	public void SetupServer()
	{
		NetworkServer.Listen(GameManager.PORT_NUM);
		isAtStartup = false;
	}

	// Create a client and connect to the server port
	public void SetupClient()
	{
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);     
		myClient.Connect(GameManager.IP_ADDRESS, GameManager.PORT_NUM);
		isAtStartup = false;
	}
		
	public void OnConnected(NetworkMessage msg)
	{
		Debug.Log("Connected!");
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MPLobbyManager : MonoBehaviour 
{
	public static MPLobbyManager Instance;

	public Dictionary <string, Player> lobbyPlayerMap;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		lobbyPlayerMap = new Dictionary<string, Player> ();
	}

	public void JoinServer(string ip, int port)
	{
		NetworkLobbyManager.singleton.networkAddress = ip;
		NetworkLobbyManager.singleton.matchPort = port;
		NetworkLobbyManager.singleton.StartClient();
	}

	public void CreateServer()
	{
		NetworkLobbyManager.singleton.matchPort = LocalNetworkDiscovery.Instance.PortNumber;
		NetworkLobbyManager.singleton.StartHost ();
	}

	public void DisConnect ()
	{
		if (NetworkLobbyManager.singleton != null) 
		{
			if (GameManager.playerType == GameManager.PlayerType.Master) {
				NetworkLobbyManager.singleton.StopHost ();
			} else if (GameManager.playerType == GameManager.PlayerType.Client) {
				NetworkLobbyManager.singleton.client.Disconnect();
			}
			GameManager.playerType = GameManager.PlayerType.None;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MPLobbyManager : NetworkLobbyManager 
{
	public static MPLobbyManager Instance;

	public Dictionary <string, Player> lobbyPlayerMap;

	private NetworkLobbyManager networkLobbyManager;

	void Start ()
	{
		Instance = this;
		lobbyPlayerMap = new Dictionary<string, Player> ();
		networkLobbyManager = NetworkLobbyManager.singleton as NetworkLobbyManager;
	}

	public void JoinServer(string ip, int port)
	{
		networkLobbyManager.networkAddress = ip;
		networkLobbyManager.matchPort = port;
		networkLobbyManager.StartClient();
		networkLobbyManager.client.RegisterHandler (MsgType.Connect, OnConnect);
		networkLobbyManager.client.RegisterHandler (MsgType.Disconnect, OnDisconnect);
	}

	public void CreateServer()
	{
		networkLobbyManager.matchPort = LocalNetworkDiscovery.Instance.PortNumber;
		networkLobbyManager.StartHost ();
		networkLobbyManager.client.RegisterHandler (MsgType.Connect, OnConnect);
		networkLobbyManager.client.RegisterHandler (MsgType.Disconnect, OnDisconnect);
	}

	public void OnConnect (NetworkMessage netMsg)
	{
		networkLobbyManager.TryToAddPlayer ();
	}

	public void OnDisconnect (NetworkMessage netMsg)
	{
		if (networkLobbyManager != null) 
		{
			if (GameManager.playerType == GameManager.PlayerType.Master) {
				networkLobbyManager.StopHost ();
			} else if (GameManager.playerType == GameManager.PlayerType.Client) {
				networkLobbyManager.client.Disconnect();
			}
			GameManager.playerType = GameManager.PlayerType.None;
		}
	}

	public void AddToLobby (string id, Player player)
	{
		if (!lobbyPlayerMap.ContainsKey (id)) {
			lobbyPlayerMap.Add (id, player);
		}
	}

	public void RemoveFromLobby (string id)
	{
		if (!lobbyPlayerMap.ContainsKey (id)) {
			lobbyPlayerMap.Remove(id);
		}
	}

	public void LoadGameScene ()
	{
		GameManager.Instance.localLobbyPlayer.RpcStartGamePlay ();
		//ServerChangeScene (playScene);
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log (lobbyPlayer.name + " : Adding player : " + gamePlayer.name);
		SoldierController soldierController = gamePlayer.GetComponent <SoldierController> ();
		Player playerInfo = lobbyPlayer.GetComponent<Player> ();
		soldierController.player = playerInfo;

		if (playerInfo.team == Player.Team.Blue) {
			gamePlayer.transform.position = SpawnManager.Instance.spawnPointTeamA [Random.Range (0, SpawnManager.Instance.spawnPointTeamA.Length)].position;
		} 
		else {
			gamePlayer.transform.position = SpawnManager.Instance.spawnPointTeamB [Random.Range (0, SpawnManager.Instance.spawnPointTeamB.Length)].position;
		}
		gamePlayer.SetActive (true);
		if (playerInfo.isLocalPlayer) {
			CameraFollow.Instance.target = gamePlayer.transform;
		}
		return false;
	}

	public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
	{
		Debug.Log ("OnLobbyServerCreateGamePlayer : " + playerControllerId);
		GameObject player = Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		return player;
	}
}
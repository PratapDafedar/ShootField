using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MPLobbyManager : MonoBehaviour 
{
	public static MPLobbyManager Instance;

	public Dictionary <string, Player> lobbyPlayerMap;

	private NetworkLobbyManager networkLobbyManager;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		lobbyPlayerMap = new Dictionary<string, Player> ();
		networkLobbyManager = NetworkLobbyManager.singleton as NetworkLobbyManager;
	}

	public void JoinServer(string ip, int port)
	{
		networkLobbyManager.networkAddress = ip;
		networkLobbyManager.matchPort = port;
		networkLobbyManager.StartClient();
	}

	public void CreateServer()
	{
		networkLobbyManager.matchPort = LocalNetworkDiscovery.Instance.PortNumber;
		networkLobbyManager.StartHost ();
	}

	public void Disconnect ()
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

	public void OnLobbyServerPlayersReady()
	{
		Debug.LogError ("OnLobbyServerPlayersReady");
		bool allready = true;
		for(int i = 0; i < networkLobbyManager.lobbySlots.Length; ++i)
		{
			if(networkLobbyManager.lobbySlots[i] != null)
				allready &= networkLobbyManager.lobbySlots[i].readyToBegin;
		}

		if(allready)
			StartCoroutine(ServerCountdownCoroutine());
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

				for (int i = 0; i < networkLobbyManager.lobbySlots.Length; ++i)
				{
					if (networkLobbyManager.lobbySlots[i] != null)
					{//there is maxPlayer slots, so some could be == null, need to test it before accessing!
						(networkLobbyManager.lobbySlots[i] as Player).RpcUpdateCountdown(floorTime);
					}
				}
			}
		}

		for (int i = 0; i < networkLobbyManager.lobbySlots.Length; ++i)
		{
			if (networkLobbyManager.lobbySlots[i] != null)
			{
				(networkLobbyManager.lobbySlots[i] as Player).RpcStartGamePlay();
			}
		}

		networkLobbyManager.ServerChangeScene(networkLobbyManager.playScene);
	}
}

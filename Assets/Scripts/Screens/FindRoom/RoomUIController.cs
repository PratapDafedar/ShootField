using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class RoomUIController : MonoBehaviour 
{	
	public static RoomUIController Instance;

	public RectTransform findServerPanel;
	public RectTransform createServerPanel;
	public RectTransform lobbyServerPanel;
	public Button startButton;

	public InputField portField;
	public Toggle findRoomToggle;
	public Toggle createRoomToggle;

	public ServerInfoCell infoCellPrefab;

	public GameObject playerCellPrefab;
	public GameObject teamBlueTableParent;
	public GameObject teamRedTableParent;

	public List<PlayerCellView> playerCellList;

	public enum State
	{
		FindRoom,
		CreateRoom,
		Lobby,
	}
	public State curState;

	void Awake()
	{
		Instance = this;	
	}

	void Start ()
	{
		Init ();
	}

	void Init ()
	{
		LocalNetworkDiscovery.Instance.StartAsClient ();
		portField.text = LocalNetworkDiscovery.Instance.PortNumber.ToString ();
	}

	public void OnBackButtonTapped ()
	{
		SceneManager.Instance.LoadLoginScreen ();
	}

	public void OnSelectFindServerTab ()
	{
		if (findRoomToggle.isOn) {
			findServerPanel.gameObject.SetActive (true);
			createServerPanel.gameObject.SetActive (false);

			LocalNetworkDiscovery.Instance.StartAsClient ();
		}
	}

	public void OnSelectCreateServerTab ()
	{
		if (createRoomToggle.isOn) 
		{
			findServerPanel.gameObject.SetActive (false);
			createServerPanel.gameObject.SetActive (true);
		}
	}

	public void OnSelectCreateServer ()
	{
		LocalNetworkDiscovery.Instance.StartAsServer (int.Parse (portField.text));
	}

	public void OnPortNumberUpdate (InputField inputField)
	{
		LocalNetworkDiscovery.Instance.PortNumber = int.Parse(inputField.text);
	}

	public void RefreshServer ()
	{
		LocalNetworkDiscovery.Instance.RefreshServer ();
	}

	public void OnLobbyDisconnectButtonPressed ()
	{
		UpdatePanelState (State.FindRoom);
		MPLobbyManager.Instance.DisConnect ();
	}

	public void CreateServerList (Dictionary <string, NetworkBroadcastResult> broadCastResult)
	{
		List<Transform> childs = new List<Transform> ();
		foreach (Transform child in infoCellPrefab.transform.parent) {
			childs.Add (child);
		}
		childs.ForEach((Transform t) =>
			{
				if (t != infoCellPrefab.transform)
				{
					Destroy(t.gameObject);
				}
			});

		if (broadCastResult == null)
			return;

		foreach (string key in broadCastResult.Keys) 
		{
			NetworkBroadcastResult result = broadCastResult [key];
			string infoText = key.Replace ("::ffff:", "");

			GameObject cloneCell = Instantiate (infoCellPrefab.gameObject) as GameObject;
			cloneCell.gameObject.SetActive (true);
			cloneCell.transform.SetParent (infoCellPrefab.transform.parent, false);
			ServerInfoCell infoCell = cloneCell.GetComponent<ServerInfoCell> ();
			infoCell.Init (infoText, infoText);
		}
	}

	public void UpdatePanelState (State screenType)
	{
		findServerPanel.gameObject.SetActive (screenType == State.FindRoom);
		createServerPanel.gameObject.SetActive (screenType == State.CreateRoom);
		lobbyServerPanel.gameObject.SetActive (screenType == State.Lobby);

		switch (screenType)
		{
		case State.FindRoom:
			{				
				findRoomToggle.isOn = true;
			}
			break;
		case State.CreateRoom:
			{
				
			}
			break;
		case State.Lobby:
			{
				startButton.gameObject.SetActive (GameManager.playerType == GameManager.PlayerType.Master);
			}
			break;
		}
	}

	public void CreatePlayerList()
	{
		ClearPlayerList ();
		if (MPLobbyManager.Instance.lobbyPlayerMap == null)
			return;
		playerCellList = new List<PlayerCellView> ();
		foreach (string playerId in MPLobbyManager.Instance.lobbyPlayerMap.Keys) 
		{
			Player player = MPLobbyManager.Instance.lobbyPlayerMap [playerId];
			GameObject clone = Instantiate (playerCellPrefab) as GameObject;

			clone.transform.SetParent (player.team == Player.Team.Red ? teamRedTableParent.transform : teamBlueTableParent.transform, false);

			PlayerCellView playerCellView = clone.GetComponent<PlayerCellView> ();
			playerCellView.AssignPlayerInfo (player.playerName, player.netId.ToString (), player);
			playerCellList.Add (playerCellView);
		}
	}

	public void OnDisconnectButtonPressed ()
	{
		if (NetworkManager.singleton != null) 
		{
			if (GameManager.playerType == GameManager.PlayerType.Master) {
				NetworkManager.singleton.StopHost ();
			} else if (GameManager.playerType == GameManager.PlayerType.Client) {
				NetworkManager.singleton.client.Disconnect();
			}
		}
		SceneManager.Instance.LoadLobbyScreen ();
	}

	public void OnPlayButtonPressed ()
	{
		SceneManager.Instance.LoadGamePlayScreen ();
	}

	public void ClearIndividualPlayerList(string id)
	{
		foreach (PlayerCellView playerCell in playerCellList)
		{
			if (playerCell.player.id == id) {
				DestroyImmediate (playerCell.gameObject);
				playerCellList.Remove (playerCell);
			}
		}
	}

	public void ClearPlayerList()
	{
		if (playerCellList != null) 
		{
			for (int i = 0; i < playerCellList.Count; i++) {
				DestroyImmediate (playerCellList [i].gameObject);
			}
			playerCellList.Clear ();
		}
	}

	public void SwitchTeam ()
	{
		RoomUIController.Instance.CreatePlayerList ();
	}
}
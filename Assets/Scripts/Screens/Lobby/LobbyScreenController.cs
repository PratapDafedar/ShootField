using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LobbyScreenController : MonoBehaviour 
{
	public GameObject team;
	public GameObject indiviualPlayerData;
	public GameObject indiviualPlayerBlueTeamParent;
	public GameObject indiviualPlayerRedTeamParent;

	private GameObject obj;

	private PlayerCellView playerCellView;

	private List<GameObject> playerList;

	void Start()
	{
		Init ();
	}

	void Init()
	{
		playerList = new List<GameObject> ();
		ListOfPlayerInTeam ();
	}

	public void ListOfPlayerInTeam()
	{
		obj = Instantiate (indiviualPlayerData) as GameObject;
		obj.transform.SetParent (indiviualPlayerRedTeamParent.transform , false);
		playerCellView = obj.GetComponent<PlayerCellView> ();
		playerList.Add (obj);
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

	public void ClearIndividualPlayerList(int i)
	{
		DestroyImmediate (playerList[i]);
		playerList.RemoveAt(i);
	}

	public void ClearPlayerList()
	{
		for (int i = 0; i < playerList.Count; i++) 
		{
			DestroyImmediate (playerList[i]);
		}
		playerList.Clear ();
	}
}

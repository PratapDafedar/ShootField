using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LobbyScreenController : MonoBehaviour 
{
	public GameObject indiviualPlayerData;
	public GameObject indiviualPlayerBlueTeamParent;
	public GameObject indiviualPlayerRedTeamParent;

	private GameObject obj;

	private PlayerCellView playerCellView;

	private List<GameObject> playerList;

	void Start()
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
			NetworkManager.singleton.StopHost ();
		}
		GameManager.Instance.LoadFindRoomScreen ();
	}

	public void OnPlayButtonPressed ()
	{
		GameManager.Instance.LoadGamePlayScreen ();
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

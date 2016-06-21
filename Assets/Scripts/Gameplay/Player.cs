using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class Player : NetworkLobbyPlayer
{
	[SyncVar]
	public string playerName;

	public SoldierController soldier;

	public enum Team
	{
		Blue,
		Red,
	}

	[SyncVar(hook = "OnTeamChange")]
	public Team team;

	[SyncVar]
	public string id;

	[SyncVar]
	public int score;

	public void Init (string name, Team team)
	{
		this.playerName = name;
		this.team = team;
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby ();
		Debug.Log ("OnClientEnterLobby");

		//if (this.isLocalPlayer) 
		{
			playerName = GameManager.Instance.playerName;
			team = GameManager.Instance.playerTeam;
			id = "LobbyPlayer-" + this.netId;
		}
		this.gameObject.name = "LobbyPlayer-" + this.netId;
		RoomUIController.Instance.UpdatePanelState (RoomUIController.State.Lobby);

		MPLobbyManager.Instance.lobbyPlayerMap.Add (id, this);
		RoomUIController.Instance.CreatePlayerList ();
	}

	public override void OnClientExitLobby()
	{
		base.OnClientExitLobby ();
		Debug.Log ("OnClientExitLobby");

		MPLobbyManager.Instance.lobbyPlayerMap.Remove (id);
		RoomUIController.Instance.CreatePlayerList ();
	}

	public override void OnClientReady(bool readyState)
	{
		base.OnClientReady (readyState);
		Debug.Log ("OnClientReady : " + readyState);
	}

	[ClientRpc]
	public void RpcUpdateCountdown(int countdown)
	{
		Debug.Log ("Coutdown timer : " + countdown);
	}

	[ClientRpc]
	public void RpcStartGamePlay()
	{
		SceneManager.Instance.LoadGamePlayScreen ();
	}

	void OnTeamChange (Team team)
	{
		this.team = team;
		RoomUIController.Instance.SwitchTeam ();
	}
}

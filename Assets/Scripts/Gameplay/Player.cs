using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[NetworkSettings(channel=1,sendInterval=0.2f)]
public class Player : NetworkLobbyPlayer
{
	[SyncVar(hook = "OnPlayerNameChange")]
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
		Debug.Log (this.netId + ".OnClientEnterLobby : " + isLocalPlayer);

		this.readyToBegin = true;
		Invoke("UpdatePlayerStatus", 1f);
	}

	void UpdatePlayerStatus ()
	{
		Debug.Log (this.netId + ".UpdatePlayerStatus : " + isLocalPlayer);
		RoomUIController.Instance.UpdatePanelState (RoomUIController.State.Lobby);
		if (isLocalPlayer) {
			playerName = GameManager.Instance.playerName;
			team = GameManager.Instance.playerTeam;
			GameManager.Instance.localPlayer = this;
			if (GameManager.playerType == GameManager.PlayerType.Client) {
				this.CmdUpdateName (playerName);
			} 
		} else {
			OnPlayerUpdate ();
		}
	}

	void OnPlayerUpdate ()
	{
		id = "LobbyPlayer-" + this.netId;
		this.gameObject.name = "LobbyPlayer-" + this.netId;
		MPLobbyManager.Instance.AddToLobby (id, this);
		RoomUIController.Instance.CreatePlayerList ();
	}

	public override void OnClientExitLobby()
	{
		base.OnClientExitLobby ();
		Debug.Log ("OnClientExitLobby");

		MPLobbyManager.Instance.RemoveFromLobby (id);
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

	[Command]
	public void CmdSwitchTeam() 
	{
		Player.Team team = (this.team == Player.Team.Blue) ? Player.Team.Red : Player.Team.Blue;
		this.team = team;
	}
	
	[Command]
	public void CmdUpdateName(string playerName) 
	{
		Debug.Log ("Receive Name : " + playerName);
		this.playerName = playerName;
		OnPlayerUpdate ();
	}

	void OnTeamChange (Team team)
	{
		this.team = team;
		RoomUIController.Instance.SwitchTeam ();
	}

	void OnPlayerNameChange (string playerName)
	{
		this.playerName = playerName;
		OnPlayerUpdate ();
	}

	void OnDestroy ()
	{
		if (isLocalPlayer) {
			MPLobbyManager.Instance.Disconnect ();
		}
	}
}

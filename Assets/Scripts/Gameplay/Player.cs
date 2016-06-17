using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class Player : NetworkLobbyPlayer
{
	[SyncVar]
	public string _name;

	public SoldierController soldier;

	public enum Team
	{
		Blue,
		Red,
	}
	[SyncVar]
	public Team team;

	[SyncVar]
	public string id;

	[SyncVar]
	public int score;

	public Player (string name, Team team)
	{
		this._name = name;
		this.team = team;
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby ();
		Debug.Log ("OnClientEnterLobby");
	}

	public override void OnClientExitLobby()
	{
		base.OnClientExitLobby ();
		Debug.Log ("OnClientExitLobby");
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
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public const string IP_ADDRESS = "127.0.0.1";
	public const int PORT_NUM = 22222;
	public const float COUNTDOWN_TIME = 5;
	private const string PLAYER_ID_PREFIX = "Player ";

	public static GameManager Instance;

	public string playerName
	{
		get {
			return PlayerPrefs.GetString ("PLAYER_NAME");
		}
		set {
			PlayerPrefs.SetString ("PLAYER_NAME", value);
		}
	}
	public Player.Team playerTeam;

	private static Dictionary<string, SoldierController> playerDict;

	public enum PlayerType
	{
		None = -1,
		Master,
		Client,
	}
	public static PlayerType playerType;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (this.gameObject);
		}
	}

	void Start () 
	{
		playerDict = new Dictionary<string, SoldierController> ();
	}
	
	#region Player tracking

	public static void RegisterPlayer (string _netID, SoldierController _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		playerDict.Add(_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnRegisterPlayer (string _playerID)
	{
		playerDict.Remove(_playerID);
	}

	public static SoldierController GetPlayer (string _playerID)
	{
		return playerDict[_playerID];
	}

	void OnGUI ()
	{
		GUILayout.BeginArea(new Rect(200, 200, 200, 500));
		GUILayout.BeginVertical();

		foreach (string _playerID in playerDict.Keys)
		{
			GUILayout.Label(_playerID + "  -  " + playerDict[_playerID].transform.name);
		}

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	#endregion
}

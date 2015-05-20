using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                DontDestroyOnLoad(go);
                return go.AddComponent<GameManager>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private static GameManager _instance;

    [SerializeField]
    public User cPlayer;

    public List<GameObject> proxyPlayerList;

    public int teamScoreBlue;
    public int teamScoreRed;

    void Awake()
    {
        if (_instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            if (cPlayer == null)
            {
                cPlayer = new User();
            }
        }
        else
        {
            DestroyImmediate (this.gameObject);
        }
    }

    public static void LoadLobbyScreen()
    {
        Application.LoadLevel("Lobby");
    }
    
    public static void LoadGameRoom ()
    {
        Application.LoadLevel("Room");

        //TODO://
        Instance.teamScoreBlue = 0;
        Instance.teamScoreRed = 0;
        Instance.proxyPlayerList = new List<GameObject>();
    }

    public static void LoadLogin ()
    {
        Application.LoadLevel("Login");
    }

    public void RespawnAllPlayers ()
    {
        NetworkManager.Instance.networkView.RPC("RPC_RespawnAllPlayers", uLink.RPCMode.All, teamScoreBlue, teamScoreRed);

        NetworkManager.Instance.networkView.RPC("RefreshUserList", uLink.RPCMode.Others, User.TableToString(NetworkManager.Instance.playerTable));
    }

    public void CheckRoundFinish()
    {
        bool isAllDeadBlue = false;
        bool isAllDeadRed = false;

        isAllDeadBlue = CheckTeamAliveState (User.Team.BLUE);
        isAllDeadRed = CheckTeamAliveState(User.Team.RED);

        if (isAllDeadBlue || isAllDeadRed)
        {
            if (isAllDeadRed)
                teamScoreBlue ++;

            if (isAllDeadBlue)
                teamScoreRed ++;
            
            RespawnAllPlayers ();
        }
    }

    private static bool CheckTeamAliveState(User.Team team)
    {
        int deadCount = 0;
        int playerCount = 0;

        foreach (int i in NetworkManager.Instance.playerTable.Keys)
        {
            User tempPlayer = NetworkManager.Instance.playerTable[i];
            if (tempPlayer.cTeam == team)
            {
                if (tempPlayer.health <= 0)
                {
                    deadCount++;
                }
                playerCount++;
            }
        }

        if (deadCount == playerCount)
        {
            return true;
        }
        return false;
    }
}

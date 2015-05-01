using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

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

    public User cPlayer;

    public List<User> onlinePlayers;

    void Awake()
    {
        if (_instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            if (cPlayer == null)
                cPlayer = new User();
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
    
    public static void LoadGameArena ()
    {
        Application.LoadLevel("GameArena");
    }

    public static void LoadLogin ()
    {
        Application.LoadLevel("Login");
    }
}

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
        }
        else
        {
            DestroyImmediate (this.gameObject);
        }
    }

    public void LoadLobbyScreen()
    {
        Application.LoadLevel("Lobby");
    }
    
    public void LoadGameArena ()
    {
        Application.LoadLevel("GameArena");
    }

    public void LoadLogin ()
    {
        Application.LoadLevel("Login");
    }
}

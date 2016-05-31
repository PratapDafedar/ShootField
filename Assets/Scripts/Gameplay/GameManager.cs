using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public const string IP_ADDRESS = "127.0.0.1";
	public const int PORT_NUM = 8888;

	public static GameManager Instance;

	public Player cPlayer;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (this);
		}
	}

	void Start () 
	{
	
	}
	
	public void LoadFindRoomScreen()
	{
		SceneManager.LoadScene ("FindRoom");
	}
}

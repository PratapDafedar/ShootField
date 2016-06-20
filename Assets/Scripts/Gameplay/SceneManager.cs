using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour 
{
	public static SceneManager Instance;
	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (this.gameObject);
		}
	}

	public void LoadFindRoomScreen()
	{
		LoadScene ("02.FindRoom");
	}

	public void LoadLoginScreen()
	{
		LoadScene ("01.Login");
	}

	public void LoadGamePlayScreen()
	{
		LoadScene ("1.Maze");
	}

	public void LoadScene (string scene)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (scene);
	}

	public static AsyncOperation LoadSceneAsync(string newSceneName)
	{
		return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (newSceneName);
	}

	public static UnityEngine.SceneManagement.Scene GetSceneAt (int index)
	{
		return UnityEngine.SceneManagement.SceneManager.GetSceneAt (index);
	}
}

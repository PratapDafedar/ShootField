using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyScreenController : MonoBehaviour 
{
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
}

using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkEventManager : NetworkManager 
{
	public static Action<NetworkConnection> OnClientConnectAction;
	public static Action<NetworkConnection> OnClientDisconnectAction;
	public static Action<NetworkConnection> OnServerConnectAction;
	public static Action<NetworkConnection> OnServerDisconnectAction;

	public override void OnClientConnect (NetworkConnection conn)
	{
		Debug.Log ("OnClientConnect : " + conn.address);
		if (OnClientConnectAction != null)
			OnClientConnectAction (conn);
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log ("OnClientDisconnect : " + conn.address);
		if (OnClientDisconnectAction != null)
			OnClientDisconnectAction (conn);
	}
		   
	public override void OnClientNotReady (NetworkConnection conn)
	{
		Debug.Log ("OnClientNotReady : " + conn.address);
	}
		   
	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		Debug.Log ("OnClientSceneChanged : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		Debug.Log ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		Debug.Log ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerConnect (NetworkConnection conn)
	{
		Debug.Log ("OnServerConnect : " + conn.address);
		if (OnServerConnectAction != null)
			OnServerConnectAction (conn);
	}
		   
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.Log ("OnServerDisconnect : " + conn.address);
		if (OnServerDisconnectAction != null)
			OnServerDisconnectAction (conn);
	}
		   
	public override void OnServerError (NetworkConnection conn, int errorCode)
	{
		Debug.Log ("OnServerError : " + conn.address);
	}
		   
	public override void OnServerReady (NetworkConnection conn)
	{
		Debug.Log ("OnServerReady : " + conn.address);
	}
		   
	public override void OnServerRemovePlayer (NetworkConnection conn, PlayerController player)
	{
		Debug.Log ("OnServerRemovePlayer : " + conn.address);
	}
		   
	public override void OnServerSceneChanged (string sceneName)
	{
		Debug.Log ("OnServerSceneChanged : " + sceneName);
	}
		   
	public override void OnStartClient (NetworkClient client)
	{
		Debug.Log ("OnStartClient.");
	}
		   
	public override void OnStartHost ()
	{
		Debug.Log ("OnStartHost.");
	}
		   
	public override void OnStartServer ()
	{
		Debug.Log ("OnStartServer.");
	}
		   
	public override void OnStopClient ()
	{
		Debug.Log ("OnStopClient.");
	}
		   
	public override void OnStopHost ()
	{
		Debug.Log ("OnStopHost");
	}
		   
	public override void OnStopServer ()
	{
		Debug.Log ("OnStopServer.");
	}
}

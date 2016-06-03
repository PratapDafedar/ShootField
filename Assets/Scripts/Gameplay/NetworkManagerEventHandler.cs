using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkManagerEventHandler : NetworkManager 
{
	public static Action<NetworkConnection> OnClientConnectAction;
	public static Action<NetworkConnection> OnClientDisconnectAction;
	public static Action<NetworkConnection> OnServerConnectAction;
	public static Action<NetworkConnection> OnServerDisconnectAction;

	public override void OnClientConnect (NetworkConnection conn)
	{
		Debug.LogWarning ("OnClientConnect : " + conn.address);
		if (OnClientConnectAction != null)
			OnClientConnectAction (conn);
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.LogWarning ("OnClientDisconnect : " + conn.address);
		if (OnClientDisconnectAction != null)
			OnClientDisconnectAction (conn);
	}
		   
	public override void OnClientNotReady (NetworkConnection conn)
	{
		Debug.LogWarning ("OnClientNotReady : " + conn.address);
	}
		   
	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		Debug.LogWarning ("OnClientSceneChanged : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		Debug.LogWarning ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		Debug.LogWarning ("OnServerAddPlayer : " + conn.address);
	}
		   
	public override void OnServerConnect (NetworkConnection conn)
	{
		Debug.LogWarning ("OnServerConnect : " + conn.address);
		if (OnServerConnectAction != null)
			OnServerConnectAction (conn);
	}
		   
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.LogWarning ("OnServerDisconnect : " + conn.address);
		if (OnServerDisconnectAction != null)
			OnServerDisconnectAction (conn);
	}
		   
	public override void OnServerError (NetworkConnection conn, int errorCode)
	{
		Debug.LogWarning ("OnServerError : " + conn.address);
	}
		   
	public override void OnServerReady (NetworkConnection conn)
	{
		Debug.LogWarning ("OnServerReady : " + conn.address);
	}
		   
	public override void OnServerRemovePlayer (NetworkConnection conn, PlayerController player)
	{
		Debug.LogWarning ("OnServerRemovePlayer : " + conn.address);
	}
		   
	public override void OnServerSceneChanged (string sceneName)
	{
		Debug.LogWarning ("OnServerSceneChanged : " + sceneName);
	}
		   
	public override void OnStartClient (NetworkClient client)
	{
		Debug.LogWarning ("OnStartClient.");
	}
		   
	public override void OnStartHost ()
	{
		Debug.LogWarning ("OnStartHost.");
	}
		   
	public override void OnStartServer ()
	{
		Debug.LogWarning ("OnStartServer.");
	}
		   
	public override void OnStopClient ()
	{
		Debug.LogWarning ("OnStopClient.");
	}
		   
	public override void OnStopHost ()
	{
		Debug.LogWarning ("OnStopHost");
	}
		   
	public override void OnStopServer ()
	{
		Debug.LogWarning ("OnStopServer.");
	}
}

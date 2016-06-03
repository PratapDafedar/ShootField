using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class FindCreateRoomUIController : MonoBehaviour 
{	
	public RectTransform findServerPanel;
	public RectTransform createServerPanel;

	public InputField portField;

	public ServerInfoCell infoCellPrefab;

	void Awake()
	{
		
	}

	void Start ()
	{
		LocalNetworkDiscovery.Instance.StartAsClient ();
	}

	public void OnBackButtonTapped ()
	{
		GameManager.Instance.LoadLoginScreen ();
	}

	public void OnSelectFindServerTab ()
	{
		findServerPanel.gameObject.SetActive (true);
		createServerPanel.gameObject.SetActive (false);

		LocalNetworkDiscovery.Instance.StartAsClient ();
	}

	public void OnSelectCreateServerTab ()
	{
		findServerPanel.gameObject.SetActive (false);
		createServerPanel.gameObject.SetActive (true);
	}

	public void OnSelectCreateServer ()
	{
		LocalNetworkDiscovery.Instance.StartAsServer (int.Parse (portField.text));
	}

	public void OnPortNumberUpdate (InputField inputField)
	{
		LocalNetworkDiscovery.Instance.PortNumber = int.Parse(inputField.text);
	}

	public void RefreshServer ()
	{
		LocalNetworkDiscovery.Instance.RefreshServer ();
	}

	public void CreateServerList (Dictionary <string, NetworkBroadcastResult> broadCastResult)
	{
		List<Transform> childs = new List<Transform> ();
		foreach (Transform child in infoCellPrefab.transform.parent) {
			childs.Add (child);
		}
		childs.ForEach((Transform t) =>
			{
				if (t != infoCellPrefab.transform)
				{
					Destroy(t.gameObject);
				}
			});

		if (broadCastResult == null)
			return;

		foreach (string key in broadCastResult.Keys) 
		{
			NetworkBroadcastResult result = broadCastResult [key];
			string infoText = key.Replace ("::ffff:", "");

			GameObject cloneCell = Instantiate (infoCellPrefab.gameObject) as GameObject;
			cloneCell.gameObject.SetActive (true);
			cloneCell.transform.SetParent (infoCellPrefab.transform.parent, false);
			ServerInfoCell infoCell = cloneCell.GetComponent<ServerInfoCell> ();
			infoCell.Init (infoText, result.serverAddress);
		}
	}
}
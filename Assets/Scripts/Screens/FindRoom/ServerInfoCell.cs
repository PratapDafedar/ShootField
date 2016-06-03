using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerInfoCell : MonoBehaviour 
{
	public Text infoText;
	private string ip;

	public void Init (string info, string ip) 
	{
		this.infoText.text = info;
		this.ip = ip;
	}

	public void OnConnectButtonTapped ()
	{
		print ("Pressed : " + ip);
		LocalNetworkDiscovery.Instance.ConnectToServer (ip);
	}
}
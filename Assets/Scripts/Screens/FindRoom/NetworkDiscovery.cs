using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace TEST
{
	public class NetworkDiscovery : MonoBehaviour
	{
		const int kMaxBroadcastMsgSize = 1024;

		// config data
		[SerializeField]
		public int broadcastPort = 47777;

		[SerializeField]
		public int broadcastKey = 1000;

		[SerializeField]
		public int broadcastVersion = 1;

		[SerializeField]
		public int broadcastSubVersion = 1;

		[SerializeField]
		public string broadcastData = "HELLO";

		[SerializeField]
		public int offsetX;

		[SerializeField]
		public int offsetY;

		// runtime data
		public int hostId = -1;
		public bool running = false;
		public bool isServer = false;
		public bool isClient = false;

		byte[] msgOutBuffer = null;
		byte[] msgInBuffer = null;

		static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		public bool Initialize()
		{
			if (broadcastData.Length >= kMaxBroadcastMsgSize)
			{
				Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + kMaxBroadcastMsgSize);
				return false;
			}

			if (!NetworkTransport.IsStarted)
			{
				NetworkTransport.Init();
			}


			if (NetworkManager.singleton != null)
			{
				broadcastData = "NetworkManager:"+NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
			}

			DontDestroyOnLoad(gameObject);
			msgOutBuffer = GetBytes(broadcastData);
			msgInBuffer = new byte[kMaxBroadcastMsgSize];
			return true;
		}

		// listen for broadcasts
		public bool StartAsClient()
		{
			if (hostId != -1 || running)
			{
				Debug.LogWarning("NetworkDiscovery StartAsClient already started");
				return false;
			}

			ConnectionConfig cc = new ConnectionConfig();
			cc.AddChannel(QosType.Unreliable);
			HostTopology defaultTopology = new HostTopology(cc, 1);

			hostId = NetworkTransport.AddHost(defaultTopology, broadcastPort);
			if (hostId == -1)
			{
				Debug.LogError("NetworkDiscovery StartAsClient - addHost failed");
				return false;
			}

			byte error;
			NetworkTransport.SetBroadcastCredentials(hostId, broadcastKey, broadcastVersion, broadcastSubVersion, out error);

			running = true;
			isClient = true;
			Debug.Log("StartAsClient Discovery listening");
			return true;
		}

		// perform actual broadcasts
		public bool StartAsServer()
		{
			if (hostId != -1 ||running)
			{
				Debug.LogWarning("NetworkDiscovery StartAsServer already started");
				return false;
			}

			ConnectionConfig cc = new ConnectionConfig();
			cc.AddChannel(QosType.Unreliable);
			HostTopology defaultTopology = new HostTopology(cc, 1);

			hostId = NetworkTransport.AddHost(defaultTopology, 0);
			if (hostId == -1)
			{
				Debug.LogError("NetworkDiscovery StartAsServer - addHost failed");
				return false;
			}

			byte err;
			if (!NetworkTransport.StartBroadcastDiscovery(hostId, broadcastPort, broadcastKey, broadcastVersion, broadcastSubVersion, msgOutBuffer, msgOutBuffer.Length, 1000, out err))
			{
				Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err);
				return false;
			}

			running = true;
			isServer = true;
			Debug.Log("StartAsServer Discovery broadcasting");
			return true;
		}

		public void StopBroadcast()
		{
			if (hostId == -1)
			{
				Debug.LogError("NetworkDiscovery StopBroadcast not initialized");
				return;
			}

			if (!running)
			{
				Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
				return;
			}
			if (isServer)
			{
				NetworkTransport.StopBroadcastDiscovery();
			}

			NetworkTransport.RemoveHost(hostId);
			hostId = -1;
			running = false;
			isServer = false;
			isClient = false;
			Debug.Log("Stopped Discovery broadcasting");
		}

		void Update()
		{
			if (hostId == -1)
				return;

			if (isServer)
				return;

			int connectionId;
			int channelId;
			int receivedSize;
			byte error;
			NetworkEventType networkEvent = NetworkEventType.DataEvent;

			do
			{
				networkEvent = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

				if (networkEvent == NetworkEventType.BroadcastEvent)
				{
					NetworkTransport.GetBroadcastConnectionMessage(hostId, msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

					string senderAddr;
					int senderPort;
					NetworkTransport.GetBroadcastConnectionInfo(hostId, out senderAddr, out senderPort, out error);

					OnReceivedBroadcast(senderAddr, GetString(msgInBuffer));
				}
			} while (networkEvent != NetworkEventType.Nothing);

		}

		public virtual void OnReceivedBroadcast(string fromAddress, string data)
		{
			Debug.Log("Got broadcast from [" + fromAddress + "] " + data);
			var items = data.Split(':');
			if (items.Length == 3 && items[0] == "NetworkManager")
			{
				if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
				{
					NetworkManager.singleton.networkAddress = items[1];
					NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
					NetworkManager.singleton.StartClient();
				}
			}

		}

		void OnGUI()
		{
			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 24;

			if (msgInBuffer == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Initialize Broadcast"))
				{
					Initialize();
				}
				return;
			}
			else
			{
				GUI.Label(new Rect(xpos, ypos, 200, 20), "initialized");
			}
			ypos += spacing;

			if (running)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop"))
				{
					StopBroadcast();
				}
				ypos += spacing;
			}
			else
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Start Broadcasting"))
				{
					StartAsServer();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Listen for Broadcast"))
				{
					StartAsClient();
				}
				ypos += spacing;
			}
		}
	}
}
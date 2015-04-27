using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using SocketBase;

public class BaseClient : MonoBehaviour
{
    public string m_IPAdress = "127.0.0.1";
    public const int kPort = 37500;

    private static BaseClient singleton;

    private Socket m_Socket;
    private string msgToServer = "";

    void OnApplicationQuit()
    {
        m_Socket.Close();
        m_Socket = null;
    }

    void Connect()
    {
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // System.Net.PHostEntry ipHostInfo = Dns.Resolve("host.contoso.com");
        // System.Net.IPAddress remoteIPAddress = ipHostInfo.AddressList[0];
        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);

        System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, kPort);

        singleton = this;
        m_Socket.Connect(remoteEndPoint);

        //StopCoroutine (ListenData ());
        StartCoroutine (ListenData ());
    }

    IEnumerator ListenData()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (m_Socket.Poll(0, SelectMode.SelectRead))
            {
                // Read data from the connections!
                byte[] receivedbytes = new byte[512];

                ArrayList buffer = new ArrayList();

                int read = m_Socket.Receive(receivedbytes);
                for (int i = 0; i < read; i++)
                    buffer.Add(receivedbytes[i]);

                while (true && buffer.Count > 0)
                {
                    int length = (byte)buffer[0];

                    if (length < buffer.Count)
                    {
                        ArrayList thismsgBytes = new ArrayList(buffer);
                        thismsgBytes.RemoveRange(length + 1, thismsgBytes.Count - (length + 1));
                        thismsgBytes.RemoveRange(0, 1);

                        if (thismsgBytes.Count != length)
                            Debug.LogError("Bug");

                        buffer.RemoveRange(0, length + 1);
                        byte[] readbytes = (byte[])thismsgBytes.ToArray(typeof(byte));

                        OnReceiveData(readbytes);
                    }
                    else
                        break;
                }
            }
        }
    }

    void OnReceiveData (byte[] byteData)
    {
        List<byte> newData = new List<byte>(byteData);
        //newData.RemoveRange(0, 10);

        string msg = MessageData.ToStringArray(newData.ToArray());

        if (msg.Length < 0)
            return;

        IPEndPoint sourceInfo = m_Socket.RemoteEndPoint as IPEndPoint;
        string serialisedMessage;
        if (sourceInfo != null)
        {
            serialisedMessage = System.String.Format("{0}:{1}:>>> {2}", sourceInfo.Address, sourceInfo.Port, msg);
        }
        else
        {
            serialisedMessage = System.String.Format("Unknown:>>> {0}", msg);
        }

        Debug.Log(serialisedMessage);
    }

    static public void Send(string msg)
    {
        if (singleton.m_Socket == null)
            return;

        byte[] sendData = MessageData.ToByteArray (msg);
        byte[] prefix = new byte[1];
        prefix[0] = (byte)sendData.Length;
        singleton.m_Socket.Send(prefix);
        singleton.m_Socket.Send(sendData);
    }

    void OnGUI()
    {
        //if connection has not been made, display button to connect
        if (m_Socket == null)
        {
            if (GUILayout.Button("Connect"))
            {
                //try to connect
                Debug.Log("Attempting to connect..");
                Connect ();
            }
        }
        //once connection has been made, display editable text field with a button to send that string to the server (see function below)
        else
        {
            msgToServer = GUILayout.TextField(msgToServer);
            if (GUILayout.Button("Write to server", GUILayout.Height(30)))
            {
                try
                {
                    Send(msgToServer);
                    msgToServer = "";
                }
                catch (System.Exception e)
                {
                    m_Socket = null;
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}
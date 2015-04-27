#if IS_UNITY
using UnityEngine;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketBase
{
    public class BaseSocketServer
    {
        const int port = 37500;

        protected static BaseSocketServer instance;

        private Socket m_Socket;

        ThreadJob m_Thread;

        List<Socket> m_Connections = new List<Socket>();
        ArrayList m_ByteBuffer = new ArrayList();

        public bool isUp
        {
            get
            {
                return !m_Thread.IsDone;
            }
        }

        public BaseSocketServer()
        {
            if (instance == null)
            {
                instance = this;
                Init();
            }
            else
            {
#if IS_UNITY
                Debug.LogError("Error -- BaseSocketServer is meant to be singleton instance.");
#else
                System.Console.WriteLine("Error -- BaseSocketServer is meant to be singleton instance.");
#endif
            }
        }

        ~BaseSocketServer()
        {
            Cleanup();
        }

#if IS_UNITY
        void OnApplicationQuit()
        {
            Cleanup();
        }
#endif

        private void Init()
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);

            m_Socket.Bind(ipLocal);

            //start listening...
            m_Socket.Listen(100);

            m_Thread = new ThreadJob(ListenData, 2000);
        }

        void Cleanup()
        {
            if (m_Socket != null)
                m_Socket.Close();
            m_Socket = null;

            foreach (Socket con in m_Connections)
                con.Close();
            m_Connections.Clear();
        }

        public void ListenData()
        {
            // Accept any new incoming connections!
            List<Socket> listenList = new List<Socket>();
            listenList.Add(m_Socket);
            Socket.Select(listenList, null, null, 1000);
            for (int i = 0; i < listenList.Count; i++)
            {
                Socket newSocket = ((Socket)listenList[i]).Accept();
                m_Connections.Add(newSocket);
                m_ByteBuffer.Add(new ArrayList());
                Print("Did connect" + newSocket.RemoteEndPoint);
            }

            // Read data from the connections!
            if (m_Connections.Count != 0)
            {
                List<Socket> connections = new List<Socket>(m_Connections);
                Socket.Select(connections, null, null, 1000);
                // Go through all sockets that have data incoming!
                foreach (Socket socket in connections)
                {
                    byte[] receivedbytes = new byte[512];

                    ArrayList buffer = (ArrayList)m_ByteBuffer[m_Connections.IndexOf(socket)];
                   
                    int read;
                    try
                    {
                        read = socket.Receive(receivedbytes);
                    }
                    catch (SocketException se)
                    {
                        socket.Close();
                        m_Connections.Remove(socket);
                        continue;
                    }
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
                                Print("Bug");

                            buffer.RemoveRange(0, length + 1);
                            byte[] readbytes = (byte[])thismsgBytes.ToArray(typeof(byte));

                            string msg = MessageData.ToStringArray(readbytes);

                            IPEndPoint sourceInfo = socket.RemoteEndPoint as IPEndPoint;
                            if (sourceInfo != null)
                            {
                                Print(System.String.Format("{0}:{1}:>>> {2}", sourceInfo.Address, sourceInfo.Port, msg));
                            }
                            else
                            {
                                 Print(System.String.Format("Unknown:>>> {0}", msg));
                            }
                            //Sending message to all the listening(Client) ports.
                            SendMessageToAllListeners(connections, MessageData.ToByteArray (msg));

                            if (instance != this)
                                Print("Bug");
                        }
                        else
                            break;
                    }
                }
            }
        }

        public void SendMessageToAllListeners(List<Socket> sockets, byte[] msg)
        {
            byte[] prefix = new byte[1];
            prefix[0] = (byte)msg.Length;
            foreach (Socket socket in sockets)
            {
                if (socket.Connected)
                {
                    try
                    {
                        socket.Send(prefix);
                        socket.Send(msg);
                    }
                    catch (SocketException se)
                    {
                        socket.Close();
                        continue;
                    }
                }
            }
        }
                
        public static void Print(string msg)
        {
#if IS_UNITY
            Debug.Log(msg);
#else
            System.Console.WriteLine(msg);
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class ServerLauncher
    {
        static void Main(string[] args)
        {
            SocketBase.BaseSocketServer server;
            server = new SocketBase.BaseSocketServer();

            //To get rid from "SecurityException: Unable to connect, as no valid crossdomain policy was found" error in web build.
            SocketPolicyServer.RunSocketPolicyServer("--all");

            while (server.isUp)
            {

            }
        }
    }
}

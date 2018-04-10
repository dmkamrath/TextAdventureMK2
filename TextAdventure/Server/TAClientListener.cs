using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace TextAdventure.Server
{
    public class TAClientListener
    {
        private TAServer server;
        private TcpListener clientListener;
        private bool listening;

        public static TAClientListener startListenerThread(TAServer server)
        {
            TAClientListener l = new TAClientListener(server);
            new Thread(l.listen).Start();
            return l;
        }

        private TAClientListener(TAServer server)
        {
            this.server = server;
            TAServerLog.log("Listening for clients...", LogType.SERVER_ACTION);
            clientListener = new TcpListener(WebsocketUtility.getLocalIP(), TAServer.CLIENT_LISTEN_PORT);
            clientListener.Start();
            listening = true;
        }

        public void listen()
        {
            while(listening)
            {
                //Check if this client is already connected or not, possibly create a seperate time for connection before actual play begins
                TcpClient newClient = clientListener.AcceptTcpClient();

                Console.WriteLine("Listnener had pending...");
                server.addClient(newClient);
                //bool existingClient = myServer.checkClientConnected(newClient);
                //if (false)//existingClient)
                //{
                //    Console.WriteLine("Client Connected Already, not accepted");
                //}
                //else
                //{
                //    Console.WriteLine("Client Accepted");
                //    myServer.addClient(newClient);
                //}
            }
            clientListener.Stop();
        }

        public void shutdown()
        {
            listening = false;
        }
    }
}

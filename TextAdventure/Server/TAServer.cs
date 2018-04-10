using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TextAdventure.Game;

namespace TextAdventure.Server
{
    public delegate bool ShouldSend(TAClient c);

    public partial class TAServer
    {
        public List<TAClient> clients;
        public TAClientListener clientListener;
        public bool listeningForClients;
        public bool alive;

        public TAWorld world;

        public static int CLIENT_LISTEN_PORT = 55777;
        public HttpServer.SimpleHttpServer httpServer;

        private Dictionary<MessageType, string> messageTypeDictionary;

        public TAServer()
        {
            alive = true;
            initMessageCodeDictionary();
            world = new TAWorld();
            world.createScenes();
            clients = new List<TAClient>();
            clientListener = TAClientListener.startListenerThread(this);
            new TAServerInterface(this);
        }

        public void addClient(TcpClient c)
        {
            var newClient = TAClient.createClient(this, c);
            clients.Add(newClient);
            TAServerLog.log("Added new client: " + newClient.clientName + ", ID: " + newClient.clientID + " at " + DateTime.Now, LogType.CLIENT_MESSAGE_RX);
            sendSelectiveStory("Welcome to the server!", newClient.clientName + " has joined the server!", newClient);
            newClient.locationUpdate();
            sendProgressBarEntry("Loading Character!", 100, newClient);
        }

        public void receiveMessage(Byte[] data, TAClient sender)
        {
            TAServerLog.log("RX msg from client: " + sender.clientName + ", ID: " + sender.clientID + " at " + DateTime.Now, LogType.CLIENT_MESSAGE_RX);
            Byte[] decodedMsg = WebsocketUtility.decodeWebsocketMessage(data);
            string messageString = Encoding.UTF8.GetString(decodedMsg);
            sender.parseMessage(messageString);
            //do whatever with the message string from here
        }

        private void sendMessage(string msgStr, params TAClient[] targets)
        {
            Byte[] data = Encoding.UTF8.GetBytes(msgStr);
            Byte[] encodedMessage = WebsocketUtility.encodeWebsocketMessage(data);
            foreach (var c in targets)
            {
                try
                {
                    if (c.clientStream.CanWrite)
                        c.clientStream.Write(encodedMessage, 0, encodedMessage.Length);
                    else
                        Console.WriteLine("could not write to client stream");
                }
                catch (System.IO.IOException e)
                {
                    TAServerLog.log("Send message Failed: " + e.Message, LogType.ERROR);
                }
            }
        }

        public void sendAll(string data)
        {
            sendMessage(data, clients.ToArray());
        }

        public void sendAllBut(string data, TAClient exclude)
        {
            //var filtered = clients.ToArray();
            //var filList = filtered.ToList();
            //filList.Remove(exclude);
            //sendMessage(data, filList.ToArray());
            //The below should be able to replace the above, but im not sure yet
            sendIf(data, delegate(TAClient client){ return client != exclude; });
        }

        public void sendIf(string data, ShouldSend condition)
        {
            List<TAClient> targets = new List<TAClient>();
            foreach(var c in clients)
            {
                if (condition(c))
                    targets.Add(c);
            }
            sendMessage(data, targets.ToArray());
        }

        public void shutDown()
        {
            clientListener.shutdown();
            sendStoryAll("The server has shutdown. You have been disconnected");
            foreach(var c in clients)
            {
                c.clientAlive = false;
            }
        }

        public void initMessageCodeDictionary()
        {
            messageTypeDictionary = new Dictionary<MessageType, string>();
            messageTypeDictionary.Add(MessageType.STORY, "s");
            messageTypeDictionary.Add(MessageType.LOCATION, "l");
            messageTypeDictionary.Add(MessageType.INVENTORY, "i");
            messageTypeDictionary.Add(MessageType.TIME, "t");
        }

        public void httpListenerOn()
        {
            httpServer = new HttpServer.SimpleHttpServer();
            httpServer.run();
        }

        public void httpListenerOff()
        {
            httpServer.stop();
        }
    }
}


public enum MessageType
{
    STORY,
    LOCATION,
    INVENTORY,
    TIME,
};

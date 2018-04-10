using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using TextAdventure.Game.Actor.Character;

namespace TextAdventure.Server
{
    public class TAClient
    {
        public int clientID;
        public string clientName = "Unnamed";
        public TAServer server;
        public TcpClient clientConnection;
        public NetworkStream clientStream;
        public bool clientAlive;

        public TAPlayer playerCharacter;

        public static TAClient createClient(TAServer server, TcpClient clientConnection)
        {
            TAClient c = new TAClient(server, clientConnection);
            WebsocketUtility.websocketHandshake(clientConnection);
            new Thread(c.clientListenCycle).Start();
            return c;
        }

        private TAClient(TAServer server, TcpClient clientConnection)
        {
            playerCharacter = new TAPlayer(server, server.world.startScene, this);
            clientAlive = true;
            this.server = server;
            this.clientConnection = clientConnection;
            clientStream = clientConnection.GetStream();
            clientID = ClientIDManager.getNextID();
            //locationUpdate();
        }

        public void parseMessage(string message)
        {
            string[] msgParts = message.Split(new[] { ' ' },2);
            if (msgParts.Length < 2)
                return;
            string verb = msgParts[0];
            string noun = msgParts[1];
            playerCharacter.tryActionVerb(verb, noun);
        }

        public void locationUpdate()
        {
            server.sendStoryMessage(playerCharacter.currentScene.getSceneDescription(playerCharacter), this);
        }

        public void clientListenCycle()
        {
            while(clientAlive)
            {
                while (!clientStream.DataAvailable);
                Byte[] data = new Byte[clientConnection.Available];
                clientStream.Read(data, 0, data.Length);
                server.receiveMessage(data, this);
            }
        }

        public void stopClient()
        {
            Console.WriteLine("Tried to send message");
            server.sendStoryMessage("You have been disconnected.", this);
            clientAlive = false;
        }
    }
}

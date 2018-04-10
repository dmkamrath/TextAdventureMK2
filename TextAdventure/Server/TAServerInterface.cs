using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TextAdventure.Server
{
    public class TAServerInterface
    {
        public TAServer server;
        public List<InputOption> availableOptions;

        public TAServerInterface(TAServer server)
        {
            this.server = server;
            new Thread(rcvInput).Start();
            addInputOptions();
        }

        public void addInputOptions()
        {
            availableOptions = new List<InputOption>();
            availableOptions.Add(new NumClientsOption(this));
            availableOptions.Add(new HelpOption(this));
            availableOptions.Add(new ExitOption(this));
            availableOptions.Add(new KickOption(this));
            availableOptions.Add(new HttpOn(this));
            availableOptions.Add(new HttpOff(this));
        }

        public void rcvInput()
        {
            while(server.alive)
            {
                string input = Console.ReadLine();
                InputOption o = getOptionFromString(input);
                if(o == null)
                {
                    Console.WriteLine("Invalid Input. Try typing help.");
                }
                else
                {
                    o.doOption();
                }
            }
        }

        public InputOption getOptionFromString(string s)
        {
            foreach(var o in availableOptions)
            {
                if (o.optionString == s)
                    return o;
            }
            return null;
        }
    }

    public class InputOption
    {
        public string optionString;
        public string optionDescription;
        public TAServerInterface serverInterface;
        public InputOption(TAServerInterface serverInterface)
        {
            this.serverInterface = serverInterface;
        }
        public virtual void doOption() { }
    }

    public class NumClientsOption : InputOption
    {
        public NumClientsOption(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "num clients";
            optionDescription = "Displays the number of clients that are currently connected";
        }

        public override void doOption()
        {
            Console.WriteLine(serverInterface.server.clients.Count + " clients are currenctly connected");
        }
    }

    public class HelpOption : InputOption 
    {
        public HelpOption(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "help";
            optionDescription = "Desplays all input options and describes their funcitons.";
        }
        public override void doOption()
        {
            foreach(var io in serverInterface.availableOptions)
            {
                Console.WriteLine(io.optionString);
                Console.WriteLine("\t" + io.optionDescription + "\n");
            }
        }
    }

    public class ExitOption : InputOption
    {
        public ExitOption(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "exit";
            optionDescription = "Shuts down the server";
        }
        public override void doOption()
        {
            serverInterface.server.shutDown();
        }
    }

    public class KickOption : InputOption
    {
        public KickOption(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "kick";
            optionDescription = "Begin the process of kicking a player from the server.";
        }
        public override void doOption()
        {
            Console.WriteLine("Who would you like to kick? Enter the player ID.");
            foreach(var p in serverInterface.server.clients)
            {
                Console.WriteLine(p.clientName + ", ID: " + p.clientID + "\n");
            }
            string targetID = Console.ReadLine();
            foreach(var p in serverInterface.server.clients)
            {
                if(Convert.ToInt32(targetID) == p.clientID)
                {
                    p.stopClient();
                    TAServerLog.log(p.clientName + " was kicked from the server", LogType.PLAYER_KICKED);
                    return;
                }
            }
            Console.WriteLine("No player was found matching that ID.");
        }
    }

    public class HttpOn : InputOption
    {
        public HttpOn(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "httpon";
            optionDescription = "Turn on the http listener that provides the user webpage";
        }
        public override void doOption()
        {
            serverInterface.server.httpListenerOn();
            Console.WriteLine("Http server is now running");
        }
    }

    public class HttpOff : InputOption
    {
        public HttpOff(TAServerInterface serverInterface) : base(serverInterface)
        {
            optionString = "httpoff";
            optionDescription = "Turn off the http listener that provides the user webpage";
        }
        public override void doOption()
        {
            serverInterface.server.httpListenerOff();
            Console.WriteLine("Http server has been stopped");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TextAdventure.Game.Actor.Actions;
using TextAdventure.Server;
using TextAdventure.Game;

namespace TextAdventure.Game
{
    public class TAActor
    {
        public string actorName;

        private TAServer server;
        public TAScene currentScene;
        public List<TAAction> availableActions = new List<TAAction>();
        public List<TALiveAction> ongoingActions = new List<TALiveAction>();
        public bool awake;

        public TAActor(TAServer server, TAScene scene)
        {
            awake = true;
            this.server = server;
            currentScene = scene;
            new Thread(tickLoop).Start();
        }

        public void tickLoop()
        {
            while(awake)
            {
                Thread.Sleep(50);
                actorTick();
            }
        }

        public virtual void actorTick()
        {
            for(int i = 0; i < ongoingActions.Count; i++)
            {
                if(ongoingActions[i].isActive())
                    ongoingActions[i].tickAction();
            }
        }

        public TAServer getServer() { return server; }
    }
}

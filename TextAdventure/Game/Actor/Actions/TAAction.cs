using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Server;
using TextAdventure.Game.Actor;

namespace TextAdventure.Game.Actor.Actions
{
    public class TAAction
    {
        public string actionName;
        public TAActor sourceActor;
        public object target;
        public int actionSpeed;
        private ShouldSend rcvBroadcastCheck;
        public object[] actionArgs;

        public TAAction(TAActor relevantActor)
        {
            this.sourceActor = relevantActor;
            rcvBroadcastCheck = delegate (TAClient c) { return true; };
        }

        public virtual bool checkArgs(params object[] args)
        {
            return true;
        }

        public TALiveAction makeLive(params object[] args)
        {
            if (!checkArgs(args))
                return null;
            this.actionArgs = args;
            return new TALiveAction(this, actionSpeed);
        }

        public virtual void actionStart()
        {

        }

        public virtual void actionUpdate()
        {

        }

        public virtual void actionEnd()
        {

        }

        public void broadcastAction()
        {
            var server = sourceActor.getServer();
            server.sendIf("Action Said", rcvBroadcastCheck);
        }
    }
}

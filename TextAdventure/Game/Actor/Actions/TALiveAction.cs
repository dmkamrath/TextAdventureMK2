using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Game.Actor.Actions
{
    public class TALiveAction
    {
        private TAAction action;
        private int ticksRemaining;
        private bool active;

        public TALiveAction(TAAction action, int ticks)
        {
            this.action = action;
            action.actionStart();
            ticksRemaining = ticks;
            active = true;
        }

        public void tickAction()
        {
            action.actionUpdate();
            ticksRemaining--;
            if(ticksRemaining == 0)
            {
                action.actionEnd();
            }
        }

        public void pauseAction()
        {
            active = false;
        }

        public void playAction()
        {
            active = true;
        }

        public bool isActive() { return active; }
    }
}

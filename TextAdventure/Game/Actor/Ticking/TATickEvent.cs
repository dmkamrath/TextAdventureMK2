using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Game.Actor.Ticking
{
    public delegate void TickEventDelegate();

    class TATickEvent
    {
        private TickEventDelegate beginDelegate;
        private TickEventDelegate updateDelegate;
        private TickEventDelegate endDelegate;
        private int tickDuration;
        private bool tickDone;

        public TATickEvent(int tickDuration)
        {
            this.tickDuration = tickDuration;
        }

        public void incrementTick()
        {
            tickDuration--;
            if(tickDuration == 0)
            {
                endDelegate();
                tickDone = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Server
{
    class ClientIDManager
    {
        private static int nextID = 0;
        public static int getNextID()
        {
            nextID++;
            return nextID;
        }
    }
}

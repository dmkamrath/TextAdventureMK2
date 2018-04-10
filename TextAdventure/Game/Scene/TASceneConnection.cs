using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Game.Scene
{
    public class TASceneConnection
    {
        public string inDescription;
        public string outDescription;
        public TAScene inScene;
        public TAScene outScene;
        public TADir[] inDirs;

        public TASceneConnection(TAScene inScene, TAScene outScene, string inDes, string outDes, params TADir[] inDirs)
        {
            this.inScene = inScene;
            this.outScene = outScene;
            inDescription = inDes;
            outDescription = outDes;
            this.inDirs = inDirs;
        }

        public TAScene getConnectedScene(TAScene currentScene)
        {
            if (currentScene == inScene)
                return outScene;
            return inScene;
        }

        public TADir[] getDirs(TAScene currentScene)
        {
            if (currentScene == inScene)
                return inDirs;
            var oppDirs = new TADir[inDirs.Length];
            for(int i = 0; i < inDirs.Length; i++)
                oppDirs[i] = TACoordinateSystem.getOppositeDir(inDirs[i]);
            return oppDirs;            
        }

        public string getDescription(TAScene currentScene)
        {
            if (currentScene == inScene)
                return inDescription;
            return outDescription;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Server;
using TextAdventure.Game;

namespace TextAdventure.Game.Actor
{
    public class TACharacter : TAActor
    {
        public TACharacter(TAServer server, TAScene scene) : base(server, scene)
        {

        }

        public void moveScenes(TAScene newScene)
        {
            currentScene.actorLeft(this);
            currentScene = newScene;
            currentScene.actorEntered(this);
        }
    }
}

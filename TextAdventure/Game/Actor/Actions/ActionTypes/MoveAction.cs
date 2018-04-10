using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Game.Actor.Character;
using TextAdventure.Game.Actor.Actions;
using TextAdventure.Game.Scene;

namespace TextAdventure.Game.Actor.Actions.ActionTypes
{
    public class MoveAction : TAAction
    {
        public MoveAction(TACharacter character) : base(character)
        {
            actionName = "move";
            actionSpeed = 40;
        }

        public override bool checkArgs(params object[] args)
        {
            if (args.Length == 0)
                return false;
            TASceneConnection connection = sourceActor.currentScene.getMoveTarget((string)args[0]);
            if (connection != null)
            {
                target = connection.getConnectedScene(sourceActor.currentScene);
                return true;
            }
            return false;
        }

        public override void actionEnd()
        {
            var targetScene = target as TAScene;
            var character = sourceActor as TACharacter;
            if (character == null)
                return;
            sourceActor.currentScene.localBroadcast(sourceActor.actorName + " has left " + sourceActor.currentScene.sceneName);
            character.moveScenes(targetScene);
            var playerChar = character as TAPlayer;
            if (playerChar == null)
                return;
            playerChar.playerClient.locationUpdate();
        }
    }
}

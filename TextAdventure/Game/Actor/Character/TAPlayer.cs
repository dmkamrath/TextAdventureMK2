using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Game.Actor.Actions;
using TextAdventure.Server;
using TextAdventure.Game.Actor.Actions.ActionTypes;

namespace TextAdventure.Game.Actor.Character
{
    public class TAPlayer : TACharacter
    {
        public TAClient playerClient;

        public TAPlayer(TAServer server,TAScene scene, TAClient client) : base(server, scene)
        {
            playerClient = client;
            addDefaultActions();
            actorName = playerClient.clientName;
        }

        public void addDefaultActions()
        {
            availableActions.Add(new MoveAction(this));
        }

        public void tryActionVerb(string verb, params string[] nouns)
        {
            var action = actionFromVerb(verb);
            if (action == null)
            {
                getServer().sendStoryMessage("You can't do that", playerClient);
                return;
            }
            TALiveAction  liveAction = action.makeLive(nouns);
            if (liveAction == null)
            {
                getServer().sendStoryMessage("That's not a valid target", playerClient);
                return;
            }
            ongoingActions.Add(liveAction);
            string actionString = actorName + ": " + action.actionName + ": " + action.target;
            getServer().sendProgressBarEntry(actionString, action.actionSpeed, playerClient);
        }

        public TAAction actionFromVerb(string verb)
        {
            foreach(var a in availableActions)
            {
                Console.WriteLine("matching verb " + verb + " to action " + a.actionName);
                if (a.actionName == verb)
                    return a;
            }
            return null;
        }
    }
}

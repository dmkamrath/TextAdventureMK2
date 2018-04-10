using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Game.Actor.Character;
using TextAdventure.Game.Actor;
using TextAdventure.Game.Scene;

namespace TextAdventure.Game
{
    public class TAScene
    {
        public string sceneName;
        public string sceneDescription;
        public List<TASceneConnection> connections = new List<TASceneConnection>();
        private List<TAActor> actors = new List<TAActor>();
        private List<TAPlayer> players = new List<TAPlayer>();
        public TACoordinate location;

        public TAScene(string sceneName, TACoordinate location)
        {
            this.sceneName = sceneName;
            this.location = location;
        }

        public TAScene(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public string getSceneDescription(TACharacter requestor)
        {
            string sceneDes = sceneName + ". ";
            sceneDes += sceneDescription;
            
            sceneDes += " You can see ";
            int added = 0;
            foreach(var a in actors)
            {
                if (a != requestor)
                {
                    sceneDes += a.actorName;
                    added++;
                }
            }
            if (added == 0)
                sceneDes += " nothing of interest ";
            sceneDes += " here. ";
            foreach(var c in connections)
            {
                sceneDes += " The " + c.getDescription(this) + " leads to " + c.getConnectedScene(this).sceneName + ".";
            }
            return sceneDes;
        }

        public void connectScene(TAScene other, TASceneConnection connection)
        {
            connections.Add(connection);
            other.connections.Add(connection);
        }

        public void actorEntered(TAActor newActor)
        {
            actors.Add(newActor);
            var player = newActor as TAPlayer;
            if (player != null)
                players.Add(player);
            localBroadcast(newActor.actorName + " has entered the scene");
        }

        public void actorLeft(TAActor leaver)
        {
            actors.Remove(leaver);
            var player = leaver as TAPlayer;
            if (player != null)
                players.Remove(player);
            localBroadcast(leaver.actorName + " has left the scene");
        }

        public TASceneConnection getMoveTarget(string name)
        {
            foreach(var c in connections)
            {
                if(c.getDescription(this) ==  name)
                {
                    return c;
                }
            }
            return null;
        }

        public void localBroadcast(string message)
        {
            foreach(var p in players)
            {
                p.getServer().sendStoryMessage(message, p.playerClient);
            }
        }

        public override string ToString()
        {
            return sceneName;
        }
    }
}

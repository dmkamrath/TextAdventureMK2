using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Game.Scene;

namespace TextAdventure.Game
{
    public class TAWorld
    {
        public TACoordinateSystem coordSystem;
        public TAScene startScene;

        public TAWorld()
        {
            coordSystem = new TACoordinateSystem(1000, 1000, 4);

        }

        public TAScene createScenes()
        {
            startScene = new TAScene("Jail Cell",TACoordinateSystem.startlocation);
            startScene.sceneDescription = @"A cold, damp cell. A weak ray of pale light slips through a tiny window high on the back wall,
                and the cracks in the floor are caked with age old dust. An incessant dripping can be heard from somewhere down the hall.";
            var hallwayScene = new TAScene("Hallway");
            hallwayScene.sceneDescription = @"The hallway seems as though it has not been walked in years, as if just disturbing a speck of dust will anger sleeping ghosts.";
            startScene.connectScene(hallwayScene, new TASceneConnection(startScene,hallwayScene,"Hallway Door", "Jail Cell Door"));
            return startScene;
        }
    }
}

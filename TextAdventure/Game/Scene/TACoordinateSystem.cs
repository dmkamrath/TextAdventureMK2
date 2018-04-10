using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Game.Scene
{
    public struct TACoordinate
    {
        public int x;
        public int y;
        public int z;
        public TACoordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public enum TADir
    {
        north,
        south,
        east,
        west,
        up,
        down,
        none
    }

    public class TACoordinateSystem
    {
        public List<TAScene> scenes;
        public TAScene[][][] sceneArray;

        public static TACoordinate startlocation = new TACoordinate(500, 500, 0);

        public TACoordinateSystem(int xbound, int ybound, int zbound)
        {
            initSceneArray(xbound, ybound, zbound);
        }

        public void initSceneArray(int xbound, int ybound, int zbound)
        {
            sceneArray = new TAScene[xbound][][];
            for (int i = 0; i < sceneArray.Length; i++)
            {
                sceneArray[i] = new TAScene[ybound][];
                for (int j = 0; j < sceneArray[i].Length; j++)
                {
                    sceneArray[i][j] = new TAScene[zbound];
                }
            }
        }

        public void registerCoordinate(TACoordinate c, TAScene scene)
        {
            sceneArray[c.x][c.y][c.z] = scene;
        }

        public TACoordinate getCoordinateFromOffset(TACoordinate original, params TADir[] dirs)
        {
            TACoordinate ret = original;
            if (dirs.Length == 0)
                return original;
            foreach(var d in dirs)
            {
                ret = applyDirShift(ret, d);
            }
            return ret;
        }

        public static TACoordinate applyDirShift(TACoordinate coord, TADir dir)
        {
            TACoordinate ret = coord;
            switch (dir)
            {
                case TADir.north:
                    ret.y += 1;
                    break;
                case TADir.south:
                    ret.y -= 1;
                    break;
                case TADir.east:
                    ret.x += 1;
                    break;
                case TADir.west:
                    ret.x -= 1;
                    break;
                case TADir.up:
                    ret.z += 1;
                    break;
                case TADir.down:
                    ret.z -= 1;
                    break;
                case TADir.none:
                    break;
            }
            return ret;
        }

        public static TADir getOppositeDir(TADir d)
        {
            switch(d)
            {
                case TADir.north:
                    return TADir.south;
                case TADir.south:
                    return TADir.north;
                case TADir.east:
                    return TADir.east;
                case TADir.west:
                    return TADir.east;
                case TADir.up:
                    return TADir.down;
                case TADir.down:
                    return TADir.up;
            }
            return TADir.none;
        }
    }
}

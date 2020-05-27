using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    public static class Obstacle
    {
        public static terrainGraph addObstacletoGraph(terrainGraph graph, int maxWidth, int maxHeight)
        {
            Random prng = new Random();

            //Generate Max values for obstacle size
            int obstacleHeight = prng.Next(0, maxHeight);
            int obstacleWidth = prng.Next(0, maxWidth);

            //Generate random position for the obstacle
            // must be at least 50px away from all walls to ensure that a route 
            // through is always possible
            int obstacleXPos = prng.Next(50, 450 - obstacleWidth);
            int obstacleYPos = prng.Next(50, 450 - obstacleHeight);

            for (int i = obstacleXPos; i < obstacleXPos + obstacleWidth; i++)
            {
                for ( int j = obstacleYPos; j < obstacleYPos + obstacleHeight; j++)
                {
                    graph.terrainNodes[i,j].heightValue = -2;
                }
            }


            return graph;
        }


        public static terrainGraph addObstacletoGraphAtCoords(terrainGraph graph, int height, int width, int xPos, int yPos)
        {
            for (int i = xPos; i < xPos + width; i++)
            {
                for (int j = yPos; j < yPos + height; j++)
                {
                    graph.terrainNodes[i, j].heightValue = -2;
                }
            }

            return graph;
        }

        public static terrainGraph addObstacletoGraphAtCoords(terrainGraph graph, int TLX, int TLY, int BRX, int BRY, int flag)
        {
            for (int i = TLY; i < BRY; i++)
            {
                for (int j = TLX; j < BRX; j++)
                {
                    graph.terrainNodes[i, j].heightValue = -2;
                    graph.terrainNodes[i, j].paintValue = 150;
                }
            }

            return graph;
        }




    }
}

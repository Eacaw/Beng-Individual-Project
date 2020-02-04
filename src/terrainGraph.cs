using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    class terrainGraph
    {
        private DataNode[,] terrainNodes;
        private int height, width;

        /**
         * Terrain Graph Constructor
         */
        public terrainGraph(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        /**
         * Call the noise generator to construct the graph and input all
         * data into the nodes. 
         */
        private void constructTerrainFromNoiseMap()
        {

        }

        /**
         * Output a text visualisation of the graph 
         */
        public void printGraphToConsole()
        {

        }



    }
}

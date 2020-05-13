using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.lib;
using BEng_Individual_Project.src.Utilities;

namespace BEng_Individual_Project.src
{
    public class Agent
    {
        public Path agentPath { get; set; }

        public float pathCost { get; set; }
        public bool hitTarget { get; set; }
        public float riskValue { get; set; }
        public float distanceFromTarget { get; set; }

        public DataNode startingNode { get; set; }
        public DataNode targetNode { get; set; }

        // GA variables
        public float fitnessScore { get; set; }



        public Agent(DataNode startingNode, DataNode targetNode, DataNode edgeNode)
        {
            this.agentPath = new Path();
            this.startingNode = startingNode;
            this.targetNode = targetNode;

            this.pathCost = 0;

            // Add the edgeNode to the pathway at index 0;
            this.agentPath.addNodeToPath(edgeNode);
            // Add the starting Node to pathway at index 1;
            this.agentPath.addNodeToPath(this.startingNode);

            this.hitTarget = false;

        }

        // Overload method to create child agent from parent agent
        // avoids the need to pass references to special nodes
        public Agent(Agent parentAgent, Path newPath)
        {
            this.startingNode = parentAgent.startingNode;
            this.targetNode = parentAgent.targetNode;

            this.agentPath = newPath;

            this.hitTarget = false;
        }

        public void findDistanceToTarget()
        {
            this.distanceFromTarget = numericalUtilities.getDistanceBetweenNodes(this.agentPath.getFinalNode(), this.targetNode);
        }




    }
}

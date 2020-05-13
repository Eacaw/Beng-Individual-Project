using System;
using System.Collections.Generic;
namespace BEng_Individual_Project.src
{
    public class matingPartners
    {

        Agent parentA { get; set; }
        Agent parentB { get; set; }

        public List<int> crossoverIndeciesA { get; set; }
        public List<int> crossoverIndeciesB { get; set; }

        public matingPartners()
        {
            this.crossoverIndeciesA = new List<int>();
            this.crossoverIndeciesB = new List<int>();
        }

        public matingPartners(Agent ParentA, Agent ParentB)
        {
            // Sort the parents such that parent A has the longer path by node count
            if (ParentA.agentPath.getNodeCount() > ParentB.agentPath.getNodeCount())
            {
                this.parentA = ParentA;
                this.parentB = ParentB;
            }
            else
            {
                this.parentA = ParentB;
                this.parentB = ParentA;
            }
            this.crossoverIndeciesA = new List<int>();
            this.crossoverIndeciesB = new List<int>();
        }

        public Agent getParentA()
        {
            return this.parentA;
        }

        public Agent getParentB()
        {
            return this.parentB;
        }

    }
}

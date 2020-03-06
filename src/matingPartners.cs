using System;
namespace BEng_Individual_Project.src
{
    public class matingPartners
    {

        Agent parentA { get; set; }
        Agent parentB { get; set; }

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

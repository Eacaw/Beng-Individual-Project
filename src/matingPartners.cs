using System;
namespace BEng_Individual_Project.src
{
    public class matingPartners
    {

        Agent parentA { get; set; }
        Agent parentB { get; set; }

        public matingPartners(Agent ParentA, Agent ParentB)
        {
            this.parentA = ParentA;
            this.parentB = ParentB;
        }
    }
}

using System;
namespace BEng_Individual_Project.src
{
    public class Hostile
    {

        public int[] hostilePosition;
        public float hostileSeverity;
        public float hostileRange;


        public Hostile(int xPos, int yPos, float severity, float elevation, float range)
        {
            this.hostilePosition = new int[2];
            this.hostilePosition[0] = xPos;
            this.hostilePosition[1] = yPos;
            this.hostileSeverity = severity;
            this.hostileRange = range;
        }

        public Hostile(int[] position, float severity, float elevation, float range)
        {

            this.hostilePosition = position;
            this.hostileSeverity = severity;
            this.hostileRange = range;

        }

       



    }
}

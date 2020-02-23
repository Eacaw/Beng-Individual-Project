using System;
namespace BEng_Individual_Project.src
{
    public class Hostile
    {

        int[] hostilePosition;
        float hostileSeverity;
        float hostileRange;


        public Hostile(int xPos, int yPos, float severity, float elevation)
        {
            this.hostilePosition = new int[2];
            this.hostilePosition[0] = xPos;
            this.hostilePosition[1] = yPos;
            this.hostileSeverity = severity;
        }

        public Hostile(int[] position, float severity, float elevation)
        {

            this.hostilePosition = position;
            this.hostileSeverity = severity;

        }



    }
}

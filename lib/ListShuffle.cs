using System;
using System.Collections.Generic;
using BEng_Individual_Project.src;

namespace BEng_Individual_Project.lib
{
    public static class ListShuffle
    {
        private static Random rng = new Random();

        public static List<DataNode> Shuffle<DataNode>(List<DataNode> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                DataNode value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

    }
}

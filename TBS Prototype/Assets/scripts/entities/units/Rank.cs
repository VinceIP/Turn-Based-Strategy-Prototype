using UnityEngine;

namespace Busted
{
    /// <summary>
    /// A unit's combat rank. Increases when an enemy is killed.
    /// </summary>
    /// 
    [System.Serializable]
    public class Rank
    {
        public int rank;
        public static int rank0 = 0;
        public static int rank1 = 5;
        public static int rank2 = 15;
        public static int rank3 = 25;

        public Rank()
        {
            rank = 0;
        }
    }
}

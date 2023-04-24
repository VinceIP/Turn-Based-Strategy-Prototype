using UnityEngine;
using System.Collections;

namespace Busted
{
    public class TileLoader : MonoBehaviour
    //LOAD TILES TO MEMORY
    {
        public static TileLoader instance = null;

        public Sprite[] grassToWater;
        //Load sprite angles
        //[x,0] = Center
        //[x,1] = TL
        //[x,2] = T
        //[x,3] = TR
        //[x,4] = L
        //[x,5] = R
        //[x,6] = BL
        //[x,7] = B
        //[x,8] = BR
        //[0,x] = water to grass


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }
            //spriteAngles[0, 0] = Resources.Load<Sprite>("sprites/sprWater_C");
            //spriteAngles[0, 1] = Resources.Load<Sprite>("sprites/sprWater_TL");
            //Debug.Log(spriteAngles);
        }



    }

}

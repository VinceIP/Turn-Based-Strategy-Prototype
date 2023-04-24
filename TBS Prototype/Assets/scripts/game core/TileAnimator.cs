using UnityEngine;
using System.Collections;

namespace Busted
{
    public class TileAnimator : MonoBehaviour
    {
        public GameObject animator;
        void Start()
        {
            Tile me = GetComponent<Tile>();
            Transform trans = GetComponent<Transform>();
            //SpriteRenderer spr = GetComponent<SpriteRenderer>();
            if (me.tileType == Tile.TileType.WATER)
            {
                //Debug.Log("trying to animate");
                GameObject myAnimator = Instantiate(animator);
                Transform t = myAnimator.GetComponent<Transform>();
                t.SetParent(trans);
                t.position = trans.position - (Vector3.forward * 2);
            }
        }



    }

}

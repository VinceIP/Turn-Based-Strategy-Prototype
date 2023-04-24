using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class SpriteContainer : MonoBehaviour
    {
        public static SpriteContainer instance;

        [Header("Sprites")]
        public Sprite target;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
    }

}

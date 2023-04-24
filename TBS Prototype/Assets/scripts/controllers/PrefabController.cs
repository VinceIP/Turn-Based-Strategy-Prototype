using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class PrefabController : MonoBehaviour
    {
        public static PrefabController instance;
        public GameObject empty;
        public GameObject HumanPlayer;
        public GameObject AIPlayer;
        public GameObject curs;
        public GameObject map;
        public GameObject tile;

        [Header("Units")]
        public GameObject infantry;
        public GameObject tank;

        [Header("Buildings")]
        public GameObject city;
        public GameObject hq;
        public GameObject factory;
        public GameObject airfield;
        public GameObject seaport;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(transform.gameObject);
        }

    }

}

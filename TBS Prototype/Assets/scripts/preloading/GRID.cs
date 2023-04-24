using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Busted
{
    public static class GRID
    {
        public static Prefabs Prefabs;
        public static TileSprites TileSprites;
        public static AutotileSpriteContainer Autotiles;
        public static AudioCache AudioCache;
        public static GameController GameController;
        public static InputController InputController;
        public static AudioController AudioController;

        public static Map Map;

        static GRID()
        {
            Load();

        }

        static void Load()
        {
            GameObject g;
            g = GameObject.FindGameObjectWithTag("Preload");
            Prefabs = (Prefabs)SafeComponent(g, "Prefabs");
            TileSprites = (TileSprites)SafeComponent(g, "TileSprites");
            Autotiles = (AutotileSpriteContainer)SafeComponent(g, "AutotileSpriteContainer");
            AudioCache = (AudioCache)SafeComponent(g, "AudioCache");
            GameController = (GameController)SafeComponent(g, "GameController");
            InputController = (InputController)SafeComponent(g, "InputController");
            AudioController = (AudioController)SafeComponent(g, "AudioController");
        }

        static Component SafeComponent(GameObject g, string s)
        {
            Component c = g.GetComponent(s);
            return c;
        }
        
    }
}

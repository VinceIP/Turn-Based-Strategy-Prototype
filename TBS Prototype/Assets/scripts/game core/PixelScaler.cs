using UnityEngine;
using System.Collections;

namespace Busted
{
    public class PixelScaler : MonoBehaviour
    {

        public static float unitSize = 32f;

        public static float unitsPerPixel;

        void Start()
        {
            PixelScale();
        }

        void Update()
        {

        }

        public static void PixelScale()
        {
            unitsPerPixel = 1f / unitSize;
            //Camera.main.orthographicSize = (Screen.height / 8f) * unitsPerPixel + (EditorCamScroll.zoomLevel);
        }

    }

}

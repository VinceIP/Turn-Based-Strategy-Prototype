using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Busted
{
    public class DebugController : MonoBehaviour
    {
        public Text debugText;
        public float screenX;
        public float screenY;
        public Vector3 screenSize;
        public Vector3 mousePos;


        void Start()
        {
            debugText = GetComponentInChildren<Text>();

        }

        void Update()
        {
            screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            screenX = screenSize.x;
            screenY = screenSize.y;
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));



            debugText.text = "Mouse Position: " + mousePos.x + ", " + mousePos.y + "/n" +
                             "Screen size: " + screenX + "x" + screenY
                    ;


        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Busted.UI
{
    public class InputIcon : MonoBehaviour
    {
        public Sprite btn_Controller;
        public Sprite btn_Keyboard;

        private Image _rend;

        void Start()
        {
            _rend = GetComponent<Image>();
        }
        void Update()
        {
            if (InputController.IsKeyboardActive())
            {
                _rend.sprite = btn_Keyboard;
                _rend.color = Color.white;
            }
            else if (InputController.IsJoystickActive())
            {
                _rend.sprite = btn_Controller;
                _rend.color = Color.white;
            }
            else
            {
                _rend.sprite = null;
                _rend.color = Color.clear;
            }
        }

    }

}

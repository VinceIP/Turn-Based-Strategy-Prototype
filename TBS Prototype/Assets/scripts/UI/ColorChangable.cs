using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Busted.UI
{
    //[ExecuteInEditMode]
    public class ColorChangable : MonoBehaviour
    {
        public PlayerData.TeamColor teamColor;
        public Sprite[] sprites;
        public EditorControllerUI editorControllerUI;

        private Image _img;


        void Start()
        {
            _img = gameObject.GetComponent<Image>();
        }
        void Update()
        {
            _img = gameObject.GetComponent<Image>();
            teamColor = editorControllerUI.currentTeam;

            switch (teamColor)
            {
                case PlayerData.TeamColor.BLUE:
                    if (_img.sprite != sprites[0]) _img.sprite = sprites[0];
                    break;

                case PlayerData.TeamColor.RED:
                    if (_img.sprite != sprites[1]) _img.sprite = sprites[1];
                    break;
                case PlayerData.TeamColor.YELLOW:
                    if (_img.sprite != sprites[2]) _img.sprite = sprites[2];
                    break;
                case PlayerData.TeamColor.GREEN:
                    if (_img.sprite != sprites[3]) _img.sprite = sprites[3];
                    break;
                case PlayerData.TeamColor.NEUTRAL:
                    if (gameObject.name != "btn_HQ")
                    {
                        if (_img.sprite != sprites[4]) _img.sprite = sprites[4];
                    }
                    else teamColor = 0;

                    break;


            }
        }
    }
}


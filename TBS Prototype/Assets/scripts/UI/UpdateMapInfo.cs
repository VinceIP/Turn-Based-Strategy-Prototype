using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Busted.UI
{
    public class UpdateMapInfo : MonoBehaviour
    {
        public enum InputValue
        {
            Name,
            Description
        }
        public InputValue inputValueType;
        public InputField inputValue;

        public void UpdateInfo()
        {
            switch (inputValueType)
            {
                case InputValue.Name:
                    GRID.Map.mapName = inputValue.text;
                    break;
                case InputValue.Description:
                    GRID.Map.mapDescription = inputValue.text;
                    break;
            }
        }
    }

}

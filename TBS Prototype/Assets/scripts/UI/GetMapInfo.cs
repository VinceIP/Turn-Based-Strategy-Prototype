using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Busted.UI
{
    public class GetMapInfo : MonoBehaviour
    {
        public Text targetOutput; //Needs inspector assignment
        public enum OutputValue
        {
            Name,
            Description
        }
        public OutputValue outputValue;
        private bool updated;

        void Update()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            switch (outputValue)
            {
                case OutputValue.Name:
                    targetOutput.text = GRID.Map.mapName;
                    break;
                case OutputValue.Description:
                    targetOutput.text = GRID.Map.mapDescription;
                    break;
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class InputControllerState_Game : InputControllerState
    {
        //private EditorController editorController = EditorController.instance;

        public InputControllerState_Game() : base()
        {
            stateName = "InputControllerState_Game";
        }

        public override void OnStateEnter()
        {

        }

        public override void OnStateExit()
        {

        }
        public override void OnPressedConfirm()
        {
            //gameController.GameControllerState.On
        }
        /*
        public override void OnPressedConfirm()
        {
            if (EditorController.instance != null)
            {
                if (EditorController.instance.paintType == EditorController.PaintType.Terrain)
                {
                    if (Curs.instance.onTile.tileType != EditorController.instance.selectedTile)
                    {
                        EditorController.instance.Paint();
                    }
                }
                else if (EditorController.instance.paintType == EditorController.PaintType.Unit)
                {
                    if (Curs.instance.onTile.containsUnit == null || Curs.instance.onTile.containsUnit.unitType != EditorController.instance.selectedUnit)
                    {
                        EditorController.instance.Paint();
                    }
                }
                else if(EditorController.instance.paintType == EditorController.PaintType.Building)
                {
                    if (Curs.instance.onTile.containsBuilding == null || Curs.instance.onTile.containsBuilding.buildingType != EditorController.instance.selectedBuilding)
                    {
                        EditorController.instance.Paint();
                    }
                }
                else EditorController.instance.Paint();
            }
        }
        */

    }

}

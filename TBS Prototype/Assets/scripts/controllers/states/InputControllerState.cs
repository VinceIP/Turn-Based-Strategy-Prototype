using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Busted
{
    public class InputControllerState
    {
        public string stateName;
        public InputController inputController = GRID.InputController;
        public EditorController editorController;
        public GameController gameController;
        public InputControllerState()
        {
            stateName = "InputControllerState";
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {

        }

        public virtual void OnPressedConfirm()
        {

        }

    }

}

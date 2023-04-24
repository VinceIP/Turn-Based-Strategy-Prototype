using UnityEngine;

namespace Busted
{
    public class GameControllerState_SettingUp : GameControllerState
    {
        public GameControllerState_SettingUp() : base()
        {
            stateName = "GameControllerState_SettingUp";
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            gameController.SetupPlayers();
            gameController.StartGame();
        }
    }
}

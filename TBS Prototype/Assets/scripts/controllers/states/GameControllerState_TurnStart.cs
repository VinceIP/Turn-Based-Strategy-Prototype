using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busted
{
    public class GameControllerState_TurnStart : GameControllerState
    {
        List<Building> playerBuildings;
        public GameControllerState_TurnStart() : base()
        {
            stateName = "GameControllerState_TurnStart";
            playerBuildings = new List<Building>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            playerBuildings = gameController.playerTurn.playerData.ownedBuildings;
            foreach (Building b in playerBuildings)
            {
                gameController.playerTurn.playerData.funds += b.fundsPerTurn; //Give funds
                if (b.containsUnit && b.containsUnit.teamColor == gameController.playerTurn.playerData.teamColor)
                {
                    b.containsUnit.Health += b.recovery; //Restore health for units on building
                }

            }
            if (gameController.playerTurn.playerData.isCPU == false) gameController.GameControllerState = new GameControllerState_HumanTurn();
            else gameController.GameControllerState = new GameControllerState_AITurn();
        }
    }

}

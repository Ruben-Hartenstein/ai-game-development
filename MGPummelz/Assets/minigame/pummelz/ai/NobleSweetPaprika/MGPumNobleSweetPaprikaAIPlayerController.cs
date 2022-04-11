using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumNobleSweetPaprikaAIPlayerController : MGPumStudentAIPlayerController
    {

        public const string type = "NobleSweetPaprika";
        protected override int[] getTeamMartikels()
        {
            return new int[]{2746235, 8366074};
        }

        public MGPumNobleSweetPaprikaAIPlayerController(int playerID) : base(playerID){}

        internal override MGPumCommand calculateCommand()
        {
            MGPumMoveCommand moveCommand = null;
            List<MGPumUnit> availableUnits = state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID);
            foreach (MGPumUnit unit in availableUnits)
            {
                if (!stateOracle.canMove(unit) || unit.currentSpeed < 1)
                    continue;
                MGPumField targetField = state.getField(new Vector2Int(unit.field.x, unit.field.y) + Vector2Int.up);
                if (targetField == null || !targetField.isEmpty())
                    continue;
                
                MGPumFieldChain chain = new MGPumFieldChain(this.playerID, unit.getMoveMatcher());
                if(chain.canAdd(unit.field))
                    chain.add(unit.field);
                if(chain.canAdd(targetField))
                    chain.add(targetField);
                moveCommand = new MGPumMoveCommand(this.playerID, chain, unit);
            }
            return moveCommand;
        }
    }
}

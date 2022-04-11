using RelegatiaCCG.rccg.i18n;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumStartOfSpecificTurnCondition : MGPumEventCondition, MGPumDuration, MGPumConditionWithPST
    {
        private int turnNumber;

        private MGPumPST playerSelection = MGPumPST.Any;

        public MGPumStartOfSpecificTurnCondition() : base(typeof(MGPumStartTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumStartOfSpecificTurnCondition(MGPumPST playerSelection, int turnNumber) : base(typeof(MGPumStartTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
            this.turnNumber = turnNumber;
        }

        public MGPumStartOfSpecificTurnCondition(MGPumPST playerSelection, MGPumStateCondition stateCondition, int turnNumber) : base(typeof(MGPumStartTurnSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
            this.turnNumber = turnNumber;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumStartTurnSEvent stse = (MGPumStartTurnSEvent)gameEvent;

            //NOTE: turn will be increased in new turn event (after this) so we extrapolate what turn is starting
            int playerTurnNumber = state.playerTurnNumber[stse.playerID];
            
            playerTurnNumber++;
            

            bool result = playerSelection.isSelected(stse.playerID, source) && this.turnNumber == playerTurnNumber;

            //Debug.LogError("TunrNumber=" + playerTurnNumber + " result=" + result + " stse.playerID=" + stse.playerID);
            return result;


        }

        public MGPumPST getPST()
        {
            return this.playerSelection;
        }

        public void setPST(MGPumPST pst)
        {
            this.playerSelection = pst;
        }
    }
}

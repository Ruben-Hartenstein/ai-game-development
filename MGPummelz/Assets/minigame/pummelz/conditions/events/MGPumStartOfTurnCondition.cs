using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumStartOfTurnCondition : MGPumEventCondition, MGPumDuration, MGPumConditionWithPST
    {
        private MGPumPST playerSelection = MGPumPST.Any;

        public MGPumStartOfTurnCondition() : base(typeof(MGPumStartTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumStartOfTurnCondition(MGPumPST playerSelection) : base(typeof(MGPumStartTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
        }

        public MGPumStartOfTurnCondition(MGPumStateCondition stateCondition) : base(typeof(MGPumStartTurnSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumStartOfTurnCondition(MGPumPST playerSelection, MGPumStateCondition stateCondition) : base(typeof(MGPumStartTurnSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
            
        }

        public MGPumPST getPST()
        {
            return this.playerSelection;
        }

        public void setPST(MGPumPST pst)
        {
            this.playerSelection = pst;
        }

      
        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumStartTurnSEvent stse = (MGPumStartTurnSEvent)gameEvent;
            return playerSelection.isSelected(stse.playerID, source);
            
        }
        
    }
}

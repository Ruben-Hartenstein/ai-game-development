namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEndOfTurnCondition : MGPumEventCondition, MGPumDuration, MGPumConditionWithPST
    {
        private MGPumPST playerSelection = MGPumPST.Any;

        public MGPumEndOfTurnCondition() : base(typeof(MGPumEndTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumEndOfTurnCondition(MGPumPST playerSelection) : base(typeof(MGPumEndTurnSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
        }

        public MGPumEndOfTurnCondition(MGPumStateCondition stateCondition) : base(typeof(MGPumEndTurnSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumEndOfTurnCondition(MGPumPST playerSelection, MGPumStateCondition stateCondition) : base(typeof(MGPumEndTurnSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.playerSelection = playerSelection;
            
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumEndTurnSEvent etse = (MGPumEndTurnSEvent)gameEvent;

            return playerSelection.isSelected(etse.playerID, source);
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

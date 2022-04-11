namespace mg.pummelz
{
    [System.Serializable]
    class MGPumNextTurnSEvent : MGPumSuperEvent
    {

        public MGPumNextTurnSEvent() : base()
        {
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumNextTurnSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

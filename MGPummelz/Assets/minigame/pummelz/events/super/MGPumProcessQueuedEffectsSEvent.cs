namespace mg.pummelz
{
    [System.Serializable]
    class MGPumProcessQueuedEffectsSEvent : MGPumSuperEvent
    {

        public MGPumProcessQueuedEffectsSEvent() : base()
        {

        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumProcessQueuedEffectsSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

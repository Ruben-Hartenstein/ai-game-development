namespace mg.pummelz
{
    [System.Serializable]
    class MGPumInitialSpawnSEvent : MGPumSuperEvent
    {

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumInitialSpawnSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

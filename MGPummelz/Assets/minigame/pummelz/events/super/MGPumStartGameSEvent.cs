namespace mg.pummelz
{
    [System.Serializable]
    class MGPumStartGameSEvent : MGPumSuperEvent
    {

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumStartGameSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

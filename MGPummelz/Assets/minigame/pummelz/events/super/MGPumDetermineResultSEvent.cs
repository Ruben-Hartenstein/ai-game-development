namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDetermineResultSEvent : MGPumSuperEvent
    {
        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumDetermineResultSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

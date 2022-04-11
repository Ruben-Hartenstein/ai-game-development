namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGrantMultiBonusSEvent : MGPumSuperEvent
    {
       

        public MGPumGrantMultiBonusSEvent() : base()
        {
        }


        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumGrantMultiBonusSEvent();
            this.copyToGameEvent(result);
            return result;
        }
    }
}

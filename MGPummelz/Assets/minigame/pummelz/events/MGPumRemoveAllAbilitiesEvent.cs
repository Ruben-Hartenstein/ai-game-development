namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRemoveAllAbilitiesEvent : MGPumGameEvent
    {
        public MGPumUnit unit { get; set; }

        public MGPumRemoveAllAbilitiesEvent(MGPumUnit unit) : base()
        {
            this.unit = unit;
        }

        public override void apply(MGPumGameState state)
        {
            unit.abilityBase = null;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit p = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumRemoveAllAbilitiesEvent(p);
            this.copyToGameEvent(result);
            return result;

        }
    }


}

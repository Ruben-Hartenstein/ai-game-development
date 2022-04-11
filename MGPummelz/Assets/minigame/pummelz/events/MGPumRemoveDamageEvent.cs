namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRemoveDamageEvent : MGPumGameEvent
    {
        public MGPumEntity source { get; set; }
        public MGPumUnit unit { get; set; }


        public MGPumRemoveDamageEvent(MGPumEntity source, MGPumUnit unit) : base()
        {
            this.source = source;
            this.unit = unit;
        }

        public override void apply(MGPumGameState state)
        {
            this.unit.damage = 0;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumEntity s = state.lookupEntity(this.source);
            MGPumUnit hc = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumRemoveDamageEvent(s, hc);
            this.copyToGameEvent(result);
            return result;
        }
    }
}

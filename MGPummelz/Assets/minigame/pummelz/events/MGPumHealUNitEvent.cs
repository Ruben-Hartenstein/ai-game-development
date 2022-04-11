namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHealUnitEvent : MGPumGameEvent
    {
        public MGPumEntity source { get; set; }
        public MGPumUnit healedUnit { get; set; }
        public int amount { get; set; }


        public MGPumHealUnitEvent(MGPumEntity source, MGPumUnit healedUnit, int amount) : base()
        {
            this.source = source;
            this.healedUnit = healedUnit;
            this.amount = amount;
        }

        public override void apply(MGPumGameState state)
        {
            this.healedUnit.damage -= this.amount;
            if (this.healedUnit.damage < 0)
            {
                this.healedUnit.damage = 0;
            }
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumEntity s = state.lookupEntity(this.source);
            MGPumUnit hc = (MGPumUnit)state.lookupOrCreate(this.healedUnit);

            MGPumGameEvent result = new MGPumHealUnitEvent(s, hc, amount);
            this.copyToGameEvent(result);
            return result;
        }
    }
}

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSetHealthEvent : MGPumGameEvent
    {
        public MGPumUnit unit { get; set; }
        public int health { get; set; }


        public MGPumSetHealthEvent(MGPumUnit unit, int health) : base()
        {
            this.unit = unit;
            this.health = health;
        }

        public override void apply(MGPumGameState state)
        {
            this.unit.baseMaxHealth = this.health;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit c = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumSetHealthEvent(c, health);
            this.copyToGameEvent(result);
            return result;
        }
    }
}

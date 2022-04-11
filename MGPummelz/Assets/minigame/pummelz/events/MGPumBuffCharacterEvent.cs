namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumBuffUnitEvent : MGPumGameEvent
    {
        public MGPumUnit unit { get; set; }
        public int healthBuff { get; set; }

        public MGPumBuffUnitEvent(MGPumUnit unit, int healthBuff) : base()
        {
            this.unit = unit;
            this.healthBuff = healthBuff;
        }

        public override void apply(MGPumGameState state)
        {
            if(unit.zone == MGPumZoneType.Battlegrounds)
            {
                unit.baseMaxHealth += healthBuff;
            }
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit c = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumBuffUnitEvent(c, healthBuff);
            this.copyToGameEvent(result);
            return result;

        }
    }


}

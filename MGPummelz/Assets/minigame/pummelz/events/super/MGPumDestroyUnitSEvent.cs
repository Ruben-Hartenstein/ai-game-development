namespace mg.pummelz
{
    [System.Serializable]
    class MGPumDestroyUnitSEvent : MGPumSuperEvent, MGPumUnitReferencable
    {
        public MGPumUnit unit { get; set; }

        public MGPumUnit unitBefore { get; set; }


        public MGPumDestroyUnitSEvent(MGPumUnit unit) : base()
        {
            this.unit = unit;
            this.unitBefore = (MGPumUnit)(unit.deepCopy());
            this.unitBefore.field = this.unit.field;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit t = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumDestroyUnitSEvent(t);
            this.copyToGameEvent(result);
            return result;
        }

        public MGPumUnit getReferencableUnit()
        {
            return this.unit;
        }
    }
}

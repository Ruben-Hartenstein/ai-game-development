namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumCreateUnitCEvent : MGPumComplexGameEvent
    {
        internal MGPumInstantiateUnitEvent ice;
        internal MGPumField destination;

        public MGPumCreateUnitCEvent(MGPumInstantiateUnitEvent ice, MGPumField field, MGPumEntity source) : base(source)
        {
            this.ice = ice;
            this.destination = field;
        }

        public override void apply(MGPumGameState state)
        {
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumInstantiateUnitEvent copiedICE = (MGPumInstantiateUnitEvent)state.lookupEvent(this.ice);
            MGPumField f = state.getField((MGPumField)(this.destination));

            MGPumCreateUnitCEvent result = new MGPumCreateUnitCEvent(copiedICE, f, state.lookupEntity(source));
            this.copyToComplexGameEvent(result);
            return result;
        }

        public override void execute(MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection)
        {
            gc.spawnCreatedUnit(this, handler);
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return new MGPumDummyFilter();
        }

    }
}

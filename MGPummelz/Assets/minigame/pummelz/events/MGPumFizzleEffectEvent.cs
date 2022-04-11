namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumFizzleEffectEvent : MGPumGameEvent
    {
        public MGPumOneTimeEffect effect { get; private set; }
        public MGPumEntity source { get; private set; }
        public MGPumGameEvent trigger { get; private set; }

        public MGPumFizzleEffectEvent(MGPumOneTimeEffect effect, MGPumGameEvent trigger, MGPumEntity source) : base()
        {
            this.effect = effect;
            this.trigger = trigger;
            this.source = source;
        }

        

        public override void apply(MGPumGameState state)
        {
            //nothing happens. this is just to indicate that nothing happened when an effect should have happened.
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent t = state.lookupEvent(trigger);
            MGPumEntity s = state.lookupEntity(source);
            MGPumOneTimeEffect e = (MGPumOneTimeEffect)state.lookupEffect(effect, source);

            MGPumGameEvent result = new MGPumFizzleEffectEvent(e, t, s);
            this.copyToGameEvent(result);
            return result;

        }
    }
}

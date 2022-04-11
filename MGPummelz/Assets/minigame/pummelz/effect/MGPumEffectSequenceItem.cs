namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEffectSequenceItem
    {
        public MGPumOneTimeEffect effect;

        public bool applyStateBasedBefore;


        private MGPumEffectSequenceItem(bool applyStateBasedBefore)
        {
            this.applyStateBasedBefore = applyStateBasedBefore;
        }

        public MGPumEffectSequenceItem(MGPumOneTimeEffect effect, bool applyStateBasedBefore)
        {
            this.effect = effect;
            this.applyStateBasedBefore = applyStateBasedBefore;
        }

        internal MGPumEffectSequenceItem deepCopy(MGPumGameState state)
        {
            MGPumEffectSequenceItem other = new MGPumEffectSequenceItem(applyStateBasedBefore);
            other.effect = (MGPumOneTimeEffect)this.effect.deepCopy(state);
            return other;
        }
    }
}

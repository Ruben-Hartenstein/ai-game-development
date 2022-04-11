using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumCount
    {
        public int divider = 1;

        public abstract int count(MGPumGameState state, MGPumEntity source);

        public abstract int count(MGPumGameState state, int playerID, MGPumEntity source);

        public static MGPumCount f(MGPumFilter f)
        {
            return new MGPumFilterCount(f);
        }

        public MGPumCount withDivider(int divider)
        {
            this.divider = divider;
            return this;
        }
    }
}

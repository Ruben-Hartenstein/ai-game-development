using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumSubCondition
    {
        public abstract bool check(MGPumGameState state, MGPumEntity source);

    }
}

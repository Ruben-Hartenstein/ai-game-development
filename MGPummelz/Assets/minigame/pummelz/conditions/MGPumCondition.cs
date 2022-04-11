using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumCondition
    {
        public static MGPumStateCondition sc()
        {
            return new MGPumStateCondition();
        }
    }
}

using RelegatiaCCG.rccg.engine.state;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumReplacementEffect : MGPumEffect
    {
        public abstract MGPumGameEvent replace(MGPumGameEvent eventRoReplace, MGPumGameState state, MGPumEntity source);

        internal override void setEffectIDs(IDManager idm)
        {
            this.id = idm.getNextID();
        }
    }
}

using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumBuffOEffect : MGPumOngoingEffect
    {
        public MGPumSTAT stat;
        public int statBuff { get; set; }


        protected MGPumBuffOEffect()
        {
        }


        public MGPumBuffOEffect(MGPumSTAT stat, int statBuff, MGPumFilter affectedEntities) : this()
        {
            this.stat = stat;
            this.statBuff = statBuff;
            this.affectedEntityFilter = affectedEntities;
        }

        protected override void applyInternal(MGPumGameState state, MGPumSelection affectedEntities, MGPumEntity source)
        {
            foreach (MGPumEntity affectedEntity in affectedEntities.getSelection(state, source))
            {
                if (affectedEntity is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)affectedEntity;
                    c.currentStats[(int)stat] += statBuff;
                }
            }

        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumBuffOEffect(this.stat, this.statBuff, this.affectedEntityFilter);
        }
    }
}

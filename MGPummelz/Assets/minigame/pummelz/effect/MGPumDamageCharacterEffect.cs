using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDamageUnitEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        private int damage;
        internal bool poisonDamage;


        public MGPumDamageUnitEffect(MGPumSelector selector, int amount) : this(selector, amount, false)
        {

        }

        public MGPumDamageUnitEffect(MGPumSelector selector, int amount, bool isPoison)
        {
            this.selector = selector;
            this.damage = amount;
            this.poisonDamage = isPoison;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)e;

                    gc.damageUnit(parent, source, c, damage, false);

                }
                else
                {
                    throw new GameException("Entity " + e + " is not a unit.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumDamageUnitEffect(this.selector.deepCopySelector(state), damage, poisonDamage);
        }
    }
}

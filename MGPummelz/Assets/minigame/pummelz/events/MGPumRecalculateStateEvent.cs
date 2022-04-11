using RelegatiaCCG.rccg.engine;
using System;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRecalculateStateEvent : MGPumGameEvent
    {

        public MGPumRecalculateStateEvent() : base()
        {

        }

        public override void apply(MGPumGameState state)
        {

            //recalculate current power/health

            foreach (MGPumPlayer p in state.players)
            {
                int unitCount = state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, p.playerID).Count;

                if (unitCount == 0)
                {
                    p.losing = true;
                }
            }

            foreach (MGPumField f in state.fields)
            {
                if (f.unit != null)
                {
                    recalculateUnitState(f.unit);
                }
            }

            //apply abilities and boni AFTER reset
            state.registerOngoingAbilities();

            foreach (MGPumOngoingAbility a in state.ongoingAbilities)
            {
                if (a.isEnabled(state, a.owner) && a.effect.priority <= MGPumOngoingEffect.PRIORITY_NORMAL)
                {
                    a.effect.apply(state, a.owner);
                }
            }

            foreach (MGPumOngoingAbility a in state.ongoingAbilities)
            {
                if (a.isEnabled(state, a.owner) && a.effect.priority == MGPumOngoingEffect.PRIORITY_LATE_BEFORE_BONI)
                {
                    a.effect.apply(state, a.owner);
                }
            }

            foreach (MGPumOngoingAbility a in state.ongoingAbilities)
            {
                if (a.isEnabled(state, a.owner) && a.effect.priority >= MGPumOngoingEffect.PRIORITY_LATE_AFTER_BONI)
                {
                    a.effect.apply(state, a.owner);
                }
            }

        }

     

        private void recalculateUnitState(MGPumUnit c)
        {
            c.setCurrentToBase();
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumRecalculateStateEvent();

            this.copyToGameEvent(result);
            return result;
        }
    }
}

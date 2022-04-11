using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSelectedUnitHealthCount : MGPumCount
    {
        public MGPumSelectedUnitHealthCount()
        {
        }

        public override int count(MGPumGameState state, int playerID, MGPumEntity source)
        {
            return count(state, source);
        }

        public override int count(MGPumGameState state, MGPumEntity source)
        {
            int count = 0;
            foreach (MGPumGameEvent e in state.log.getEventsOfThisEffect())
            {
                if (e is MGPumEffectSEvent)
                {
                    MGPumEffectSEvent ese = (MGPumEffectSEvent)e;

                    if(ese.selection != null)
                    {
                        foreach(MGPumEntity entity in ese.selection.getSelection(state, source))
                        {
                            if(entity is MGPumUnit)
                            {
                                count += ((MGPumUnit)entity).currentHealth;
                            }
                        }
                    }
                }
            }
            return count / divider;
        }
    }
}

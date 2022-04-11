using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumUnitDiedCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        private int minCount;
        private bool lastTurn;
        private bool opponentsTurn;

        public MGPumUnitDiedCondition(MGPumFilter filterCondition, int minCount, bool lastTurn, bool opponentsTurn)
        {
            this.filter = filterCondition;
            this.minCount = minCount;
            this.lastTurn = lastTurn;
            this.opponentsTurn = opponentsTurn;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            int count = 0;

            LinkedList<MGPumGameEvent> eventsToCheck;
            if (lastTurn)
            {
                if (opponentsTurn)
                {
                    eventsToCheck = state.log.getEventsOfYourLastTurn(state.getOpponent(source.ownerID).playerID);
                }
                else
                {
                    eventsToCheck = state.log.getEventsOfYourLastTurn(source.ownerID);
                }
                
            }
            else
            {
                eventsToCheck = state.log.getEventsOfThisTurn();
            }

            foreach (MGPumGameEvent e in eventsToCheck)
            {
                if (e is MGPumDestroyUnitSEvent)
                {

                    MGPumDestroyUnitSEvent dcse = (MGPumDestroyUnitSEvent)e;
                    if (filter.test(state, dcse.unit, source))
                    {
                        count++;
                        if(count >= this.minCount)
                        {
                            break;
                        }
                    }
                }
            }

            return count >= this.minCount;
        }

    }
}

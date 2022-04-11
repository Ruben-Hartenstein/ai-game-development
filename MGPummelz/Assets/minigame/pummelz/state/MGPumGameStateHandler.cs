using UnityEngine;

namespace mg.pummelz
{
    public class MGPumGameStateHandler : MGPumGameEventHandler
    {
        public MGPumGameState state;
        public MGPumGameLog gameLog;


        public MGPumGameStateHandler(MGPumGameConfig gc)
        {
            state = new MGPumGameState(gc);
            gameLog = state.log;
        }

        public MGPumGameStateHandler(MGPumGameState gs)
        {
            state = gs;
            gameLog = state.log;
        }

        public void applyEvent(MGPumGameEvent e)
        {
            //Debug.Log("Applying event:" + e);
            applyEventCommon(e);

            e.apply(state);
        }

        private void applyEventCommon(MGPumGameEvent e)
        {
            e.turnNumber = state.turnNumber;
            gameLog.applyEvent(e);
        }

      



    }

}

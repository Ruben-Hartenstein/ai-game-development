using System.Collections.Generic;

namespace mg.pummelz
{
    public abstract class MGPumPlayerController
    {
        public MGPumPlayerController(int playerID)
        {
            this.playerID = playerID;
            this.logPosition = -1;
        }

        public int playerID { get; set; }

        public int logPosition { get; set; } 

        public MGPumGameController gameController { get; set; }

        public abstract void updateState(List<MGPumGameEvent> newEvents);
        public abstract void setInitialState(MGPumGameState state);

        public abstract void allowCommand();

        internal abstract void finishGame(MGPumGameResult result);

    }
}

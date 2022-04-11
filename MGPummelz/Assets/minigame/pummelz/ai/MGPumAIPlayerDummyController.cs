using System.Collections.Generic;

namespace mg.pummelz
{
    public class MGPumAIPlayerDummyController : MGPumPlayerController
    {

        public MGPumAIPlayerDummyController(int playerID) : base(playerID)
        {
        }

        public bool commandAllowed;

        public override void allowCommand()
        {
            commandAllowed = true;
        }

        public override void setInitialState(MGPumGameState state)
        {
            
        }

        public override void updateState(List<MGPumGameEvent> newEvents)
        {
            
        }

        internal override void finishGame(MGPumGameResult result)
        {

        }

        
    }
}

using System;
using System.Collections.Generic;

namespace mg.pummelz
{
    public class MGPumGUIPlayerController : MGPumPlayerController
    {
        public const string type = "HUMAN";

        internal bool reportToGUI = true;

        internal bool commandsAllowed;
        internal MGPummelz guiController;

        public MGPumGUIPlayerController(MGPummelz guiController, int playerID) : base(playerID)
        {
            this.playerID = playerID;
            this.guiController = guiController;
        }

        public MGPumGUIPlayerController nonReporting()
        {
            reportToGUI = false;
            return this;
        }

        public override void updateState(List<MGPumGameEvent> newEvents)
        {
            if (reportToGUI)
            {
                guiController.visualizeEvents(newEvents, playerID);
            }
        }

        public override void setInitialState(MGPumGameState state)
        {
            if(reportToGUI)
            {
                guiController.setPlayerState(state);
            }
            

        }

        public override void allowCommand()
        {
            this.commandsAllowed = true;
            guiController.promptHumanCommand(playerID);
        }

        public virtual bool checkCommandFromGUI(MGPumCommand c, bool checkChain)
        {
            return gameController.checkCommand(c, checkChain);

        }

        internal override void finishGame(MGPumGameResult result)
        {
            //guiController.gameFinished(result);
        }

        public void concedeGame()
        {
            this.gameController.concede(this.playerID);
        }
    }
}

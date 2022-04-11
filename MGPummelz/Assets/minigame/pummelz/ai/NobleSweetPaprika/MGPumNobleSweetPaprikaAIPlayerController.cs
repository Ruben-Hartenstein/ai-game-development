using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumNobleSweetPaprikaAIPlayerController : MGPumStudentAIPlayerController
    {

        public const string type = "NobleSweetPaprika";
        protected override int[] getTeamMartikels()
        {
            return new int[]{2746235, 8366074};
        }

        public MGPumNobleSweetPaprikaAIPlayerController(int playerID) : base(playerID){}

        internal override MGPumCommand calculateCommand()
        {
            return new MGPumEndTurnCommand(this.playerID);
        }
    }
}

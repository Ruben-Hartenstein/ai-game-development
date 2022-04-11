namespace mg.pummelz
{
    public class MGPumSkipTurnAIPlayerController : MGPumAIPlayerController
    {
        public const string type = "SKIP_TURN";

        public MGPumSkipTurnAIPlayerController(int playerID, bool threaded) : base(playerID, threaded)
        {
        }

        internal override MGPumCommand calculateCommand()
        {
            return new MGPumEndTurnCommand(this.playerID);
        }
    }
}
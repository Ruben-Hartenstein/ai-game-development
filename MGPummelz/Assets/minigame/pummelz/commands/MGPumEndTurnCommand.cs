namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumEndTurnCommand : MGPumCommand
    {
   

        public MGPumEndTurnCommand(int issuingPlayer)
        {
            this.issuingPlayer = issuingPlayer;
        }

      
        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumCommand result = new MGPumEndTurnCommand(this.issuingPlayer);
            this.copyToCommand(result);
            return result;

        }
      
    }
}

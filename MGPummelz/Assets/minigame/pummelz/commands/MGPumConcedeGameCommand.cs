namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumConcedeGameCommand : MGPumCommand
    {
   

        public MGPumConcedeGameCommand(int issuingPlayer)
        {
            this.issuingPlayer = issuingPlayer;
        }

      
        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumCommand result = new MGPumConcedeGameCommand(this.issuingPlayer);
            this.copyToCommand(result);
            return result;

        }
      
    }
}

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGameResult
    {
        public MGPumGameResultType resultType;
        public int finalTurn;

        public bool conceded = false;

        public MGPumGameConfig config;

        public MGPumGameResult(MGPumGameResultType result, MGPumGameConfig config)
        {
            this.resultType = result;
            this.config = config;
        }

    }
}

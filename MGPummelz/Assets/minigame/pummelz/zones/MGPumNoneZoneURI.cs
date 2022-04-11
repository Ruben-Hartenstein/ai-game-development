namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumNoneZoneURI : MGPumZoneURI
    {
        private int playerID;

        public MGPumNoneZoneURI(int playerID)
        {
            this.playerID = playerID;
        }

        public MGPumZoneType getZoneType()
        {
            return MGPumZoneType.Destroyed;
        }

        public MGPumZoneURI deepCopy(MGPumGameState state)
        {
            return new MGPumNoneZoneURI(playerID);
        }

        public void anonymize()
        {
            //nothing to be done as unit is not explicitly mentioned
        }

        public int getPlayerID()
        {
            return playerID;
        }

        public MGPumUnit getUnit(MGPumGameState state)
        {
            return null;
        }

        public void setUnit(MGPumUnit unit)
        {
            //nothing to be done as unit is not explicitly mentioned
        }
    }
}

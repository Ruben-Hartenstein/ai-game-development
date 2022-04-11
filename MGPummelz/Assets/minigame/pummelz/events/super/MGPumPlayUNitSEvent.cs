namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumPlayUnitSEvent : MGPumSuperEvent, MGPumPlayerReferencable, MGPumUnitReferencable
    {

        public abstract MGPumUnit getUnit();

        public MGPumUnit getReferencableUnit()
        {
            return getUnit();
        }

        public abstract int getReferencablePlayerID();
    }
}

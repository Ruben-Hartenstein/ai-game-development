namespace mg.pummelz
{

    [System.Serializable]
    public abstract class MGPumEntity
    {
        public const int NEUTRAL_OWNER = 2;

        public int id { get; set; }
        public int ownerID { get; set; }


    


        public abstract MGPumZoneType getZone();

        public void copyToGameEntity(MGPumEntity t)
        {
            t.id = this.id;
            t.ownerID = this.ownerID;
        }

    }
       
}

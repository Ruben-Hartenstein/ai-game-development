namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumSpawnSetup : MGPumSetup
    {
        public MGPumField field;

        public string unitID;

        public MGPumSpawnSetup(string unitID, int playerID, int x, int y)
        {
            this.unitID = unitID;
            this.field = new MGPumField(x, y);
        }

        

        public override void apply(MGPumGameState state)
        {
            MGPumUnit p = (MGPumUnit)state.createUnit(this.unitID, field.getPlayerID());
            p.resetToPrinted();
            p.zone = MGPumZoneType.Battlegrounds;
            MGPumField f = state.getField(field);
            f.unit = p;
        }

        public override string toI18nedString()
        {
            return "TODO";
        }
    }
}

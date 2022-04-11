namespace mg.pummelz
{
    public interface MGPumZoneURI
    {
        MGPumZoneType getZoneType();
        int getPlayerID();

        MGPumZoneURI deepCopy(MGPumGameState state);
        MGPumUnit getUnit(MGPumGameState state);
    }
}

namespace mg.pummelz
{
    //a class for events that end up in the command queue and that might need to do additional stuff depending on the current state
    public interface MGPumUnitReferencable
    {
        MGPumUnit getReferencableUnit();

    }
}

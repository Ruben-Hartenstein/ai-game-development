namespace RelegatiaCCG.rccg.engine.state
{
    [System.Serializable]
    public class IDManager
    {
        private int id = 0;

        public int getNextID()
        {
            return id++;
        }

        public IDManager deepCopy()
        {
            IDManager copy = new IDManager();
            copy.id = this.id;
            return copy;
        }

    }
}

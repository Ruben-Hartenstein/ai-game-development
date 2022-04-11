using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{

    [System.Serializable]
    public abstract class MGPumGameEvent : I18nizable
    {
        public const int UNINITIALIZED = -1000;

        public int turnNumber { get; set; }
        public int number { get; set; }
        public MGPumGameEvent parent { get; set; }

        public string getEventType()
        {
            return this.GetType().Name;
        }

        public MGPumGameEvent()
        {
            turnNumber = UNINITIALIZED;
            number = UNINITIALIZED;
            this.parent = null;
        }

        /*
        Visibility handling
        */

        public virtual MGPumGameEvent anonymize(MGPumGameState state, int playerID)
        {
            return this;
        }


        /*
        End Visibility handling
        */

        public abstract void apply(MGPumGameState state);

        public void copyToGameEvent(MGPumGameEvent e)
        {
            e.turnNumber = this.turnNumber;
            e.number = this.number;
            e.parent = parent;
        }

        public abstract MGPumGameEvent deepCopy(MGPumGameState state);

        public virtual string toI18nedString()
        {
            return getEventType();
        }
    }
}

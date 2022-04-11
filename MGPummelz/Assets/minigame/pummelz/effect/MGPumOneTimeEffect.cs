using System;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumOneTimeEffect : MGPumEffect
    {
        public void execute(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent)
        {
            MGPumEffectSEvent ese = new MGPumEffectSEvent(this, selection, source, triggeringEvent);
            ese.parent = parent;
            handler.applyEvent(ese);
        }

        //return true if execution is finished
        public bool execute(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            
            if(this is MGPumSingleEffect)
            {
                ((MGPumSingleEffect)this).executeInternal(parent, gc, handler, selection, source, triggeringEvent, eestate);
                return true;
            }
            else if (this is MGPumMultiEffect)
            {
                return ((MGPumMultiEffect)this).executeInternal(parent, gc, handler, selection, source, triggeringEvent, eestate);
            }
            else
            {
                throw new NotImplementedException("Effect type not known.");
            }
            
        }

        public abstract MGPumSelector getSelector(MGPumGameState state, MGPumEntity source);

        public abstract MGPumSelector getSelector(MGPumGameState state, MGPumEntity source, MGPumEffectExecutionState eestate);


    }
}

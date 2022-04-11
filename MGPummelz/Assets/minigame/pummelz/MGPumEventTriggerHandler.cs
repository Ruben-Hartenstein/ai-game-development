using System.Collections.Generic;

namespace mg.pummelz
{
    public class MGPumEventTriggerHandler : MGPumGameEventHandler
    {
        private MGPumGameState state;
        private MGPumGameEventHandler childHandler;


        private List<MGPumAbility> durationAbilities;
        private List<MGPumTriggeredAbility> triggeredAbilities;
        private Dictionary<int, MGPumUnit> deadlySources;


        public void registerAbilities(MGPumGameState state)
        {
            this.durationAbilities = new List<MGPumAbility>();
            this.triggeredAbilities = new List<MGPumTriggeredAbility>();
            this.deadlySources = new Dictionary<int, MGPumUnit>();


            foreach (MGPumUnit unit in state.getAllUnitsInZone(MGPumZoneType.Battlegrounds))
            {
                MGPumAbility a = unit.abilityCurrent;
                if(a != null)
                {
                    if (a is MGPumTriggeredAbility)
                    {
                        this.registerAbility((MGPumTriggeredAbility)a);
                    }
                    if (a.durationCondition != null && a.durationCondition is MGPumEventCondition)
                    {
                        this.registerDurationAbility(a);
                    }
                }
            }
        }

        public void registerAbility(MGPumTriggeredAbility ability)
        {
            triggeredAbilities.Add(ability);
        }

        public void registerDurationAbility(MGPumAbility ability)
        {
            durationAbilities.Add(ability);

        }

        public MGPumEventTriggerHandler(MGPumGameState state, MGPumGameEventHandler childHandler)
        {
            this.state = state;
            this.childHandler = childHandler;
            this.triggeredAbilities = new List<MGPumTriggeredAbility>();
        }


        public MGPumGameEvent triggerReplacementAbilities(MGPumGameEvent e)
        {

            MGPumGameEvent result = e;
            bool replaced = false;
            Dictionary<MGPumTriggeredAbility, bool> alreadyReplaced = new Dictionary<MGPumTriggeredAbility, bool>();

            do
            {
                replaced = false;
                foreach (MGPumTriggeredAbility ta in triggeredAbilities)
                {
                    if (ta.isReplacement && !alreadyReplaced.ContainsKey(ta) && ta.isTriggeredBy(e, state, ta.owner))
                    {
                        //TODO: if wanted, an indicator event that a replacement triggered could be aplied here

                        {
                            result = ((MGPumReplacementEffect)ta.effect).replace(result, state, ta.owner);
                        }
                        alreadyReplaced.Add(ta, true);
                        replaced = true;
                        break;
                    }
                }
            }
            while (replaced);

            return result;
           
        }

        public void triggerAbilities(MGPumGameEvent e)
        {
            //Debug.LogError("Checking " + e.getEventType());


            foreach (MGPumTriggeredAbility ta in triggeredAbilities)
            {

                //Debug.LogError("Triggering check " + e + " " + ta.toI18nedString());
                //if(ta.isTriggeredBy(e, state, ta.owner))
                //{
                //    Debug.LogError("Ability is triggered by " + e.ToString() + " " + ta.toI18nedString());
                //}

                if (!ta.isReplacement && ta.isTriggeredBy(e, state, ta.owner))
                {
                    //Debug.LogError("Triggered an ability " + ta.toI18nedString());
                    //if (ta.unit is RuleUnit)
                    //{
                    //    Debug.LogError("Triggered a rule unit of player " + ta.unit.ownerID);
                    //}

                    MGPumQueueEffectEvent qee = null;
                    //this indicates a trap unit was triggered
                    //if (ta.owner is Trap && ta == ((Trap)ta.owner).trapAbility)
                    //{
                    //    Debug.Log("Triggered a trap unit");
                    //    qee = new QueueEffectEvent(new TriggerTrapEffect(((Trap)ta.owner)), e, ta.owner);
                    //}
                    //else
                    {
                        qee = new MGPumQueueEffectEvent((MGPumOneTimeEffect)ta.effect, e, ta.owner);
                    }


                    qee.parent = e;

                    childHandler.applyEvent(qee);
                }
            }

        }

       

        public void applyEvent(MGPumGameEvent e)
        {
            //TODO: dreadful performance
            registerAbilities(state);

            MGPumGameEvent replacedE = triggerReplacementAbilities(e);

            childHandler.applyEvent(replacedE);
            this.triggerAbilities(replacedE);
        }


    }
}

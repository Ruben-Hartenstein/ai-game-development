namespace mg.pummelz
{
    public static class MGPumAF
    {
        public static MGPumTriggeredAbility on(MGPumEventCondition condition, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(condition, effect);
        }

        public static MGPumTriggeredAbility onEnterBattlegrounds(MGPumFilter filter, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumEnterBattlegroundsCondition(filter), effect);
        }

        public static MGPumTriggeredAbility onEnterBattlegrounds(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumEnterBattlegroundsCondition(new MGPumSourceFilter()), effect).usuallyOneTime();
        }

        public static MGPumTriggeredAbility onDestroy(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDestroyCondition(MGPumFF.source()), effect).usuallyOneTime();
        }

        public static MGPumTriggeredAbility onDestroy(MGPumFilter filter, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDestroyCondition(filter), effect);
        }

        //public static MGPumTriggeredAbility onDestroyAlliedUnit(MGPumOneTimeEffect effect)
        //{
        //    return new MGPumTriggeredAbility(new MGPumDestroyCondition(MGPumFF.alliedUnit()), effect);
        //}

        //public static MGPumTriggeredAbility onDestroyAlliedUnitIf(MGPumStateCondition condition, MGPumOneTimeEffect effect)
        //{
        //    return new MGPumTriggeredAbility(new MGPumDestroyCondition(MGPumFF.alliedUnit()), condition, effect);
        //}

        //public static MGPumTriggeredAbility onDestroyEnemyUnit(MGPumOneTimeEffect effect)
        //{
        //    return new MGPumTriggeredAbility(new MGPumDestroyCondition(MGPumFF.enemyUnit()), effect);
        //}

        public static MGPumTriggeredAbility onDamage(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDamagedCondition(new MGPumSourceFilter()), effect);
        }

        public static MGPumTriggeredAbility onDamageIf(MGPumStateCondition condition, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDamagedCondition(new MGPumSourceFilter()), condition, effect);
        }

        public static MGPumTriggeredAbility onDamage(MGPumFilter filter, MGPumEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDamagedCondition(filter), effect);
        }

        public static MGPumTriggeredAbility onDamage(int mindamage, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumDamagedCondition(new MGPumSourceFilter(), mindamage), effect);
        }

      
        public static MGPumTriggeredAbility onHeal(MGPumFilter filter, MGPumEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumHealedCondition(filter), effect);
        }

      
 


        public static MGPumTriggeredAbility onStartOfTurn(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfTurnCondition(MGPumPST.Any), effect);
        }

        public static MGPumTriggeredAbility onStartOfTurn(int number, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfSpecificTurnCondition(MGPumPST.Any, number), effect);
        }

        public static MGPumTriggeredAbility onStartOfYourTurn(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfTurnCondition(MGPumPST.Source), effect);
        }

        public static MGPumTriggeredAbility onStartOfYourTurn(int number, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfSpecificTurnCondition(MGPumPST.Source, number), effect);
        }

        public static MGPumTriggeredAbility onStartOfYourTurnIf(MGPumStateCondition condition, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfTurnCondition(MGPumPST.Source), condition, effect);
        }

        public static MGPumTriggeredAbility onStartOfOpponentsTurn(MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfTurnCondition(MGPumPST.Opponent), effect);
        }

        public static MGPumTriggeredAbility onStartOfOpponentsTurnIf(MGPumStateCondition condition, MGPumOneTimeEffect effect)
        {
            return new MGPumTriggeredAbility(new MGPumStartOfTurnCondition(MGPumPST.Opponent), condition, effect);
        }


         public static MGPumOngoingAbility ongoing(MGPumOngoingEffect oEffect)
        {
            return new MGPumOngoingAbility(oEffect);
        }

        public static MGPumOngoingAbility ongoingBuff(MGPumSTAT stat, int statBuff, MGPumFilter affectedUnits)
        {
            return new MGPumOngoingAbility(new MGPumBuffOEffect(stat, statBuff, affectedUnits));
        }

    }
}

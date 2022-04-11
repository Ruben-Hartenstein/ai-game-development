using RelegatiaCCG.rccg.engine;

namespace mg.pummelz
{
    public static class MGPumCF
    {
        public static MGPumStateCondition sc()
        {
            return new MGPumStateCondition();
        }

        public static MGPumStartOfTurnCondition untilTurnStart()
        {
            return new MGPumStartOfTurnCondition();
        }

        public static MGPumStartOfTurnCondition untilYourNextTurn()
        {
            return new MGPumStartOfTurnCondition(MGPumPST.Source);
        }

        public static MGPumStartOfTurnCondition untilTurnStart(MGPumPST player)
        {
            return new MGPumStartOfTurnCondition(player);
        }

        public static MGPumEndOfTurnCondition untilTurnEnd()
        {
            return new MGPumEndOfTurnCondition();
        }

        public static MGPumEndOfTurnCondition untilTurnEnd(MGPumPST player)
        {
            return new MGPumEndOfTurnCondition(player);
        }

        public static MGPumDamagedCondition thisIsDamaged()
        {
            return new MGPumDamagedCondition(MGPumFF.source());
        }

        public static MGPumDamagedCondition damaged(MGPumFilter filter)
        {
            return new MGPumDamagedCondition(filter);
        }

        public static MGPumDamagedCondition damagedAtLeast(MGPumFilter filter, int minDamage)
        {
            return new MGPumDamagedCondition(filter, minDamage);
        }

       

    }
}

using System;

namespace mg.pummelz
{
    [System.Serializable]
    public static class MGPumFF
    {

        public static MGPumFilter any()
        {
            return new MGPumAnyFilter();
        }

        public static MGPumListFilter units(string i18nSingular, string i18nPlural, params string[] units)
        {
            return new MGPumListFilter(i18nSingular, i18nPlural, units);
        }


        public static MGPumFilter you()
        {
            return new MGPumYouFilter();
        }
    
        public static MGPumFilter opponent()
        {
            return new MGPumOpponentFilter();
        }

        public static MGPumFilter allPlayers()
        {
            return new MGPumAllPlayersFilter();
        }

        public static MGPumFilter source()
        {
            return new MGPumSourceFilter();
        }

        internal static MGPumFilter common()
        {
            return new MGPumAnyFilter().withRarity(MGPumCRT.Common);
        }

        internal static MGPumFilter uncommon()
        {
            return new MGPumAnyFilter().withRarity(MGPumCRT.Uncommon);
        }

        internal static MGPumFilter rare()
        {
            return new MGPumAnyFilter().withRarity(MGPumCRT.Rare);
        }

        internal static MGPumFilter exceptional()
        {
            return new MGPumAnyFilter().withRarity(MGPumCRT.Exceptional);
        }

        internal static MGPumFilter emptyField()
        {
            return new MGPumFieldFilter().emptyField();
        }

        internal static MGPumFilter emptyAlliedField()
        {
            return new MGPumFieldFilter().emptyField().allied();
        }

        internal static MGPumFilter emptyEnemyField()
        {
            return new MGPumFieldFilter().emptyField().enemy();
        }


        internal static MGPumFilter fromBaseSet()
        {
            return new MGPumUnitSetFilter(MGPumBaseSet.setID);
        }

        internal static MGPumFilter unitOnBG()
        {
            return new MGPumZoneFilter(MGPumZoneType.Battlegrounds);
        }

        internal static MGPumRandomSelect random(MGPumAutoSelect filter)
        {
            return new MGPumRandomSelect(1, filter);
        }

        internal static MGPumRandomSelect random(int amount, MGPumFilter filter)
        {
            return new MGPumRandomSelect(amount, filter);
        }

   
    }
}

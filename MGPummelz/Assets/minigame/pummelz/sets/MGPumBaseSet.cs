using RelegatiaCCG.rccg.engine;

namespace mg.pummelz
{
    public class MGPumBaseSet : MGPumSet
    {
        public const string setID = "PUM";



        public override string id { get { return setID; } }

        public override bool isDebug { get { return false; } }

        //readable references for units
        public const string BELLIE = "PUM001";
        public const string HOPPEL = "PUM002";
        public const string SNEIP = "PUM003";
        public const string WOLLI = "PUM004";
        public const string CHILLY = "PUM005";
        public const string KILLY = "PUM006";
        public const string BUMMZ = "PUM007";
        public const string CZAREMIR = "PUM008";
        public const string LINK = "PUM009";
        public const string MAMPFRED = "PUM010";
        public const string FROMMLI = "PUM011";
        public const string ANGLI = "PUM012";
        public const string BUFFY = "PUM013";
        public const string HALEY = "PUM014";

        public MGPumBaseSet() : base()
        {
            {
                MGPumUnit u = new MGPumUnit(id + "001", "Bellie", health: 2, speed: 1, power: 1, range: 1);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "002", "Hoppel", health: 3, speed: 3, power: 1, range: 2);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "003", "Sneip", health: 1, speed: 2, power: 3, range: 3);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "004", "Wolli", health: 6, speed: 1, power: 1, range: 1);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "005", "Chilly", health: 4, speed: 1, power: 1, range: 1);
                u.withAbility(MGPumAF.onDamage(MGPumEF.transformSelf(KILLY)).withText("Turns to Killy when damaged."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "006", "Killy", health: 4, speed: 4, power: 4, range: 4);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "007", "Bummz", health: 3, speed: 1, power: 1, range: 1);
                u.withAbility(MGPumAF.onDestroy(MGPumEF.damageUnit(MGPumFF.unitOnBG().aroundSource(), 2)).withText("Explodes for 2 damage."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "008", "Czaremir", health: 9, speed: 1, power: 3, range: 1);
                u.withAbility(MGPumAF.onDestroy(MGPumEF.loseGame()).withText("Without him you lose."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "009", "Link", health: 5, speed: 2, power: 1, range: 2);
                u.withAbility(MGPumAF.ongoingBuff(MGPumSTAT.Power, 1, MGPumFF.unitOnBG().aroundSource().withUnitID(u.unitID).allied()).withText("Powers up nearby Links."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "010", "Mampfred", health: 4, speed: 1, power: 2, range: 1);
                u.withAbility(MGPumAF.onStartOfYourTurn(MGPumEF.healSelf(1)).withText("Has a healthy appetite."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "011", "Frömmli", health: 3, speed: 2, power: 1, range: 1);
                u.withAbility(MGPumAF.onDestroy(MGPumEF.createUnitInPlace(ANGLI)).withText("Is reborn as Ängli."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "012", "Ängli", health: 3, speed: 3, power: 3, range: 3);
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "013", "Buffy", health: 5, speed: 2, power: 1, range: 1);
                u.withAbility(MGPumAF.ongoingBuff(MGPumSTAT.Power, 1, MGPumFF.unitOnBG().aroundSource().allied()).withText("Buffs power of neighbors."));
                addUnit(u);
            }

            {
                MGPumUnit u = new MGPumUnit(id + "014", "Haley", health: 5, speed: 2, 1, range: 1);
                u.withAbility(MGPumAF.onStartOfYourTurn(MGPumEF.heal(MGPumFF.unitOnBG().aroundSource().allied().damaged(), 1)).withText("Heals adjacent allies."));
                addUnit(u);
            }

            //unused Pummlis
            //{
            //    MGPumUnit u = new MGPumUnit(id + "015", "Spot", health: 3, speed: 1, power: 1, range: 3);
            //    u.withAbility(MGPumAF.ongoingBuff(MGPumSTAT.Range, 1, MGPumFF.unitOnBG().aroundSource().allied()).withText("Guides adjacent pummlis."));
            //    addUnit(u);
            //}

            //{
            //    MGPumUnit u = new MGPumUnit(id + "016", "Mumpitz", health: 3, speed: 3, power: 1, range: 1);
            //    u.withAbility(MGPumAF.ongoingBuff(MGPumSTAT.Speed, 1, MGPumFF.unitOnBG().aroundSource()).withText("His jokes chase all away."));
            //    addUnit(u);
            //}
        }




    }
}

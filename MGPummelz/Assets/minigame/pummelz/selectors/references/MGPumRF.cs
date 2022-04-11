using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    public class MGPumRF
    {

        public static MGPumSameReference sameUnit()
        {
            return new MGPumSameReference("SNUM_ONE");
        }

        public static MGPumSameReference sameUnits()
        {
            return new MGPumSameReference("SNUM_MANY");
        }

        public static MGPumReference triggeringUnit()
        {
            return new MGPumTriggerEventUnitReference();
        }

        public static MGPumReference triggeringUnitField()
        {
            return new MGPumTriggerEventUnitFieldReference();
        }

        public static MGPumReference triggeringPlayer()
        {
            return new MGPumTriggerEventPlayerReference();
        }


        public static MGPumOwnerReference itsOwner()
        {
            return new MGPumOwnerReference("SNUM_ONE");
        }

        public static MGPumOwnerReference theirOwners()
        {
            return new MGPumOwnerReference("SNUM_MANY");
        }

    }
}

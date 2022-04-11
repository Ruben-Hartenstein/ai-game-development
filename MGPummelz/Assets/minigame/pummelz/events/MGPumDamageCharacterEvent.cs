using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDamageUnitEvent : MGPumGameEvent, MGPumUnitReferencable
    {
        public MGPumEntity source { get; set; }
        public MGPumUnit damagedUnit { get; set; }
        public int damage { get; set; }
        public bool isAttackDamage { get; set; }


        public MGPumDamageUnitEvent(MGPumEntity source, MGPumUnit damagedUnit, int damage, bool isAttackDamage) : base()
        {
            this.source = source;
            this.damagedUnit = damagedUnit;
            this.damage = damage;
            this.isAttackDamage = isAttackDamage;
        }

        public override void apply(MGPumGameState state)
        {
            this.damagedUnit.damage += this.damage;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumEntity s = state.lookupEntity(this.source);
            MGPumUnit dc = (MGPumUnit)state.lookupOrCreate(this.damagedUnit);

            MGPumGameEvent result = new MGPumDamageUnitEvent(s, dc, damage, isAttackDamage);
            this.copyToGameEvent(result);
            return result;
        }

        public MGPumUnit getReferencableUnit()
        {
            return damagedUnit;
        }
    }
}

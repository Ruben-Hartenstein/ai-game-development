using RelegatiaCCG.rccg.engine;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumUnit : MGPumEntity, I18nizable
    {
        
        public MGPumZoneType zone { get; set; }

        /// <summary>
        /// The id of the "printed" unit, not the individual unit
        /// </summary>
        /// 
        private string _unitID { get; set; }
        public string unitID
        {
            get { return _unitID; }
            set
            {
                this._unitID = value;
                if (this.artID == null)
                {
                    this.artID = value;
                }
            }
        }

        public string artID { get; set; }

        public string setID { get; set; }

        public MGPumCRT rarity { get; set; }

        public string name { get; set; }


        public int[] printedStats { get; set; }
        public int[] baseStats { get; set; }
        public int[] currentStats { get; set; }

        public MGPumAbility abilityPrinted { get; internal set; }
        public MGPumAbility abilityBase { get; internal set; }
        public MGPumAbility abilityCurrent { get; internal set; }


        public int damage { get; set; }

        public int printedMaxHealth { get { return printedStats[(short)MGPumSTAT.Health]; } set { printedStats[(short)MGPumSTAT.Health] = value; } }
        public int printedSpeed { get { return printedStats[(short)MGPumSTAT.Speed]; } set { printedStats[(short)MGPumSTAT.Speed] = value; } }
        public int printedPower { get { return printedStats[(short)MGPumSTAT.Power]; } set { printedStats[(short)MGPumSTAT.Power] = value; } }
        public int printedRange { get { return printedStats[(short)MGPumSTAT.Range]; } set { printedStats[(short)MGPumSTAT.Range] = value; } }

        public int baseMaxHealth { get { return baseStats[(short)MGPumSTAT.Health]; } set { baseStats[(short)MGPumSTAT.Health] = value; } }
        public int baseSpeed { get { return baseStats[(short)MGPumSTAT.Speed]; } set { baseStats[(short)MGPumSTAT.Speed] = value; } }
        public int basePower { get { return baseStats[(short)MGPumSTAT.Power]; } set { baseStats[(short)MGPumSTAT.Power] = value; } }
        public int baseRange { get { return baseStats[(short)MGPumSTAT.Range]; } set { baseStats[(short)MGPumSTAT.Range] = value; } }

        public int currentMaxHealth { get { return currentStats[(short)MGPumSTAT.Health]; } set { currentStats[(short)MGPumSTAT.Health] = value; } }
        public int currentSpeed { get { return currentStats[(short)MGPumSTAT.Speed]; } set { currentStats[(short)MGPumSTAT.Speed] = value; } }
        public int currentPower { get { return currentStats[(short)MGPumSTAT.Power]; } set { currentStats[(short)MGPumSTAT.Power] = value; } }
        public int currentRange { get { return currentStats[(short)MGPumSTAT.Range]; } set { currentStats[(short)MGPumSTAT.Range] = value; } }

        public int currentHealth { get { return currentMaxHealth - damage; } }

        internal MGPumST subTypePrinted;
        internal MGPumST subTypeBase;
        internal MGPumST subTypeNow;

        public MGPumField lastKnownField;

        private MGPumField _field;
        public MGPumField field {
            get { return _field; }
            set { if (value != null) {
                    lastKnownField = value;
                };
                _field = value;
            }
        }

        internal Vector2Int coords { get { return field != null ? field.coords : new Vector2Int(-1, -1); } }
        internal int x { get { return field != null ? field.coords.x : -1; } }
        internal int y { get { return field != null ? field.coords.y : -1; } }

        public int attacksThisTurn;
        public int movesThisTurn;

        public MGPumUnit(string unitID, String name, int health, int speed, int power, int range) : base()
        {
            this.id = -1;
            this.unitID = unitID;
            this.name = name;
            this.ownerID = 0;
            this.rarity = MGPumCRT.Common;

            this.subTypePrinted = MGPumST.None;
            
            this.printedStats = new int[Enum.GetNames(typeof(MGPumSTAT)).Length];

            this.printedMaxHealth = health;
            this.printedSpeed = speed;
            this.printedPower = power;
            this.printedRange = range;

            this.resetToPrinted();
        }

        public override MGPumZoneType getZone()
        {
            return zone;
        }

        public virtual string getNameI18nKey()
        {
            return this.artID + "_NAME";
        }

        public virtual string getStoryTextI18nKey()
        {
            return this.artID + "_STORYTEXT"; 
        }

        public MGPumUnit withRarity(MGPumCRT rarity)
        {
            this.rarity = rarity;
            return this;
        }

        public MGPumUnit withSubtype(MGPumST subtype)
        {
            this.subTypePrinted = subtype;
            return this;
        }

        public MGPumUnit withAbility(MGPumAbility ability)
        {
            this.abilityPrinted = ability;
            this.abilityPrinted.owner = this;
            return this;
        }

        public bool hasSubtype(MGPumST subtype)
        {
            return this.subTypeNow == subtype;
        }

        public virtual void resetToPrinted()
        {
            setBaseToPrintedUnit();
            setCurrentToBaseUnit();
        }

        public virtual void setBaseToPrinted()
        {
            setBaseToPrintedUnit();
        }

        public virtual void setCurrentToBase()
        {
            setCurrentToBaseUnit();
        }

        public void setBaseToPrintedUnit()
        {
            subTypeBase = subTypePrinted;
            this.baseStats = (int[])this.printedStats.Clone();
            this.abilityBase = this.abilityPrinted;            
        }

        public void setCurrentToBaseUnit()
        {
            subTypeNow = subTypeBase;
            this.currentStats = (int[])this.baseStats.Clone();
            this.abilityCurrent = this.abilityBase;
        }

        public override String ToString()
        {
            return name;
        }

        public virtual void copyToUnit(MGPumUnit unit)
        {
            copyToGameEntity(unit);

            unit.name = this.name;
            unit._unitID = this._unitID;
            unit.artID = this.artID;
            unit.zone = this.zone;

            unit.setID = this.setID;

            unit.rarity = this.rarity;

            unit.subTypePrinted = this.subTypePrinted;
            unit.subTypeBase = this.subTypeBase;
            unit.subTypeNow = this.subTypeNow;

            unit.printedStats = (int[])this.printedStats.Clone();
            unit.baseStats = (int[])this.baseStats.Clone();
            unit.currentStats = (int[])this.currentStats.Clone();

            unit.damage = this.damage;

            if(this.abilityPrinted != null)
            {
                unit.abilityPrinted = this.abilityPrinted.deepCopy();
                unit.abilityPrinted.owner = unit;
            }
            if (this.abilityBase != null)
            {
                unit.abilityBase = this.abilityBase.deepCopy();
                unit.abilityBase.owner = unit;
            }
            if (this.abilityCurrent != null)
            {
                unit.abilityCurrent = this.abilityCurrent.deepCopy();
                unit.abilityCurrent.owner = unit;
            }

            unit.attacksThisTurn = this.attacksThisTurn;
            unit.movesThisTurn = this.movesThisTurn;
        }


        public string toI18nedString()
        {
            return name;
        }

        public string toI18nedSearchString()
        {
            return name;
        }


        public MGPumUnit deepCopy()
        {
            MGPumUnit copy = new MGPumUnit(unitID, name, printedMaxHealth, printedSpeed, printedPower, printedRange);
            this.copyToUnit(copy);
            return copy;
        }

        public MGPumMoveChainMatcher getMoveMatcher()
        {
            return new MGPumMoveChainMatcher(this);
        }

        public MGPumAttackChainMatcher getAttackMatcher()
        {
            return new MGPumAttackChainMatcher(this);
        }
    }


}

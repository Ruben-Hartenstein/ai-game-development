using RelegatiaCCG.rccg.engine;

namespace mg.pummelz
{
    //Effect Factory
    public static class MGPumEF
    {
        public static MGPumSequentialEffect seq()
        {
            return new MGPumSequentialEffect();
        }

        public static MGPumWinGameEffect winGame()
        {
            return new MGPumWinGameEffect(MGPumFF.you());
        }

        public static MGPumLoseGameEffect loseGame()
        {
            return new MGPumLoseGameEffect(MGPumFF.you());
        }

        public static MGPumTransformUnitEffect transformSelf(string formID)
        {
            return new MGPumTransformUnitEffect(MGPumFF.source(), formID);
        }

        public static MGPumTransformUnitSelectorEffect transformSelf(MGPumAutoSelect selector)
        {
            return new MGPumTransformUnitSelectorEffect(MGPumFF.source(), selector);
        }

        public static MGPumTransformUnitEffect transform(MGPumSelector s, string formID)
        {
            return new MGPumTransformUnitEffect(s, formID);
        }
     
        public static MGPumGrantExtraTurnEffect grantExtraTurn(MGPumSelector playerSelector)
        {
            return new MGPumGrantExtraTurnEffect(playerSelector);
        }

        public static MGPumCreateUnitEffect createUnitInPlace(string id)
        {
            return new MGPumCreateUnitEffect(id, MGPumRF.triggeringUnitField());
        }

        public static MGPumCreateUnitEffect createUnit(string id)
        {
            return new MGPumCreateUnitEffect(id, MGPumFF.random(MGPumFF.emptyAlliedField()));
        }

        public static MGPumCreateRandomUnitEffect createRandomUnit(MGPumRandomSelect unitSelect)
        {
            return new MGPumCreateRandomUnitEffect(unitSelect, MGPumFF.random(MGPumFF.emptyAlliedField()));
        }

        public static MGPumCreateRandomUnitEffect createRandomUnitsEverywhere(MGPumRandomSelect unitSelect)
        {
            return new MGPumCreateRandomUnitEffect(unitSelect, MGPumFF.emptyField());
        }

        public static MGPumCreateRandomUnitEffect createRandomUnitAnyPlayer(MGPumRandomSelect unitSelect)
        {
            return new MGPumCreateRandomUnitEffect(unitSelect, MGPumFF.random(MGPumFF.emptyField()));
        }

        public static MGPumCreateUnitEffect createUnits(string id)
        {
            return new MGPumCreateUnitEffect(id, MGPumFF.emptyAlliedField());
        }

        public static MGPumCreateUnitEffect createUnitsForOpponent(string id)
        {
            return new MGPumCreateUnitEffect(id, MGPumFF.emptyEnemyField());
        }

        public static MGPumBuffCountEffect buff(MGPumSelector s, MGPumCount count)
        {
            return new MGPumBuffCountEffect(1, count, s);
        }

        public static MGPumBuffCountEffect buff(MGPumSelector s, int healthBuffPerCount, MGPumCount count)
        {
            return new MGPumBuffCountEffect(healthBuffPerCount, count, s);
        }

        public static MGPumBuffCountEffect buffSelf(int healthBuffPerCount, MGPumCount count)
        {
            return buff(MGPumFF.source(), healthBuffPerCount, count);
        }

        public static MGPumBuffEffect buff(MGPumSelector s,  int health)
        {
            return new MGPumBuffEffect( health, s);
        }

     

     

        public static MGPumBuffEffect buffSelf(int health)
        {
            return new MGPumBuffEffect(health, MGPumFF.source());
        }


        
        public static MGPumDamageUnitEffect damageUnit(MGPumSelector selector, int damage)
        {
            return new MGPumDamageUnitEffect(selector, damage);
        }

   


        public static MGPumDamageUnitCountEffect damageSelf(MGPumCount count)
        {
            return new MGPumDamageUnitCountEffect(MGPumFF.source(), count, 1);
        }

        public static MGPumDamageUnitEffect damageSelf(int damage)
        {
            return new MGPumDamageUnitEffect(MGPumFF.source(), damage);
        }

       

        public static MGPumSetHealthEffect setHealth(MGPumSelector s, int newHealth)
        {
            return new MGPumSetHealthEffect(s, newHealth);
        }

        public static MGPumHealUnitEffect healSelf(int amount)
        {
            return new MGPumHealUnitEffect(MGPumFF.source().damaged(), amount);
        }

        public static MGPumHealCountEffect healSelf(MGPumCount count)
        {
            return new MGPumHealCountEffect(count, MGPumFF.source());
        }

        public static MGPumHealUnitEffect heal(MGPumSelector s, int amount)
        {
            return new MGPumHealUnitEffect(s, amount);
        }

        public static MGPumHealCountEffect heal(MGPumSelector s, MGPumCount count)
        {
            return new MGPumHealCountEffect(count, s);
        }

     

        public static MGPumFullHealEffect fullHeal(MGPumSelector selector)
        {
            return new MGPumFullHealEffect(selector);
        }

     
                 

        public static MGPumRemoveAllAbilitiesEffect removeAllAbilitiesFromUnit(MGPumSelector selector)
        {
            return new MGPumRemoveAllAbilitiesEffect(selector, true);

        }

        public static MGPumDestroyUnitEffect destroy(MGPumSelector unitSelector)
        {
            return new MGPumDestroyUnitEffect(unitSelector);
        }

        public static MGPumConditionalEffect conditionalIf(MGPumStateCondition condition, MGPumOneTimeEffect effect)
        {
            return new MGPumConditionalEffect(condition, effect);
        }

        public static MGPumConditionalEffect conditionalIfElse(MGPumStateCondition condition, MGPumOneTimeEffect ifEffect, MGPumOneTimeEffect elseEffect)
        {
            return new MGPumConditionalEffect(condition, ifEffect, elseEffect);
        }

        public static MGPumPutUnitOnBattlegroundsEffect putUnitOnBattlegrounds(MGPumSelector unitSelector, MGPumSelector fieldSelector)
        {
            return new MGPumPutUnitOnBattlegroundsEffect(unitSelector, fieldSelector);
        }


    }
}

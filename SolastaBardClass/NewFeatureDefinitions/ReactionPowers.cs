﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.NewFeatureDefinitions
{
    public interface IReactionPowerOnAttackAttempt
    {
        bool canBeUsed(RulesetCharacter caster, RulesetCharacter attacker, RulesetCharacter defender, RulesetAttackMode attack_mode);
    }


    public interface IReactionPowerOnDamage
    {
        bool canBeUsed(RulesetCharacter caster, RulesetCharacter attacker, RulesetCharacter defender, RulesetAttackMode attack_mode, bool is_magic);
    }


    class FeatureDefinitionReactionPowerOnDamage: FeatureDefinitionPower, IReactionPowerOnDamage
    {
        public bool worksOnMelee;
        public bool worksOnRanged;
        public bool worksOnMagic;

        bool IReactionPowerOnDamage.canBeUsed(RulesetCharacter caster, RulesetCharacter attacker, RulesetCharacter defender, RulesetAttackMode attack_mode, bool is_magic)
        {
            var effect = this.EffectDescription;
            if (effect == null)
            {
                return false;
            }

            int max_distance = this.EffectDescription.TargetProximityDistance;
            bool works_on_caster = effect.TargetFilteringTag != (RuleDefinitions.TargetFilteringTag)ExtraTargetFilteringTag.NonCaster;

            if (!is_magic)
            {
                if (attack_mode.Ranged && !worksOnRanged)
                {
                    return false;
                }

                if (!attack_mode.Ranged && !worksOnMelee)
                {
                    return false;
                }
            }

            if (is_magic && !worksOnMagic)
            {
                return false;
            }

            if (attacker.Side != effect.TargetSide && effect.TargetSide != RuleDefinitions.Side.All)
            {
                return false;
            }

            if (!works_on_caster && defender == caster)
            {
                return false;
            }



            return true;
        }
    }


    class FeatureDefinitionReactionPowerOnAttackAttempt : FeatureDefinitionPower, IReactionPowerOnAttackAttempt
    {
        public bool worksOnMelee;
        public bool worksOnRanged;

        bool IReactionPowerOnAttackAttempt.canBeUsed(RulesetCharacter caster, RulesetCharacter attacker, RulesetCharacter defender, RulesetAttackMode attack_mode)
        {
            var effect = this.EffectDescription;
            if (effect == null)
            {
                return false;
            }

            int max_distance = this.EffectDescription.TargetProximityDistance;
            bool works_on_caster = effect.TargetFilteringTag != (RuleDefinitions.TargetFilteringTag)ExtraTargetFilteringTag.NonCaster;

            if (attack_mode.Ranged && !worksOnRanged)
            {
                return false;
            }

            if (!attack_mode.Ranged && !worksOnMelee)
            {
                return false;
            }

            if (attacker.Side != effect.TargetSide && effect.TargetSide != RuleDefinitions.Side.All)
            {
                return false;
            }

            if (!works_on_caster && defender == caster)
            {
                return false;
            }

            return true;
        }
    }
}

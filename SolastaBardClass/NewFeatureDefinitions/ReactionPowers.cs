using System;
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

    class FeatureDefinitionReactionPowerOnAllyAttackAttempt : FeatureDefinitionPower, IReactionPowerOnAttackAttempt
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
                return true;
            }

            return true;
        }
    }
}

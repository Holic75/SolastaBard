using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.NewFeatureDefinitions
{
    //power that will use resource from another power
    class LinkedPower : FeatureDefinitionPower
    {
        public FeatureDefinition linkedPower;

        public RulesetUsablePower getBasePower(RulesetCharacter character)
        {
            if (linkedPower == null)
            {
                return null;
            }
            return character?.usablePowers?.FirstOrDefault(p => p.PowerDefinition == linkedPower);
        }
    }
}

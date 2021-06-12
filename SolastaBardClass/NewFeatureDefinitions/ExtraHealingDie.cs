using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.NewFeatureDefinitions
{
    class FeatureDefinitionExtraHealingDieOnShortRest : FeatureDefinition
    {
        public RuleDefinitions.DieType DieType = RuleDefinitions.DieType.D1;
        public bool ApplyToParty = false;
        public string tag = "";
    }
}

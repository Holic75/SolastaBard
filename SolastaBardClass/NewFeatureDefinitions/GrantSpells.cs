using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.NewFeatureDefinitions
{
    public class GrantSpells: FeatureDefinition
    {
        public List<FeatureDefinitionAutoPreparedSpells.AutoPreparedSpellsGroup> spellGroups = new List<FeatureDefinitionAutoPreparedSpells.AutoPreparedSpellsGroup>();
        public CharacterClassDefinition spellcastingClass;
        public FeatureDefinitionCastSpell spellcastingFeature;
    }
}

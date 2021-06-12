using SolastaModApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RuleDefinitions;

namespace SolastaBardClass
{
    class Common
    {
        public static Dictionary<RuleDefinitions.RitualCasting, FeatureDefinitionMagicAffinity> ritual_spellcastings_map = new Dictionary<RitualCasting, FeatureDefinitionMagicAffinity>();

        static public void initialize()
        {
            fillRitualSpellcastingMap();
        }


        static void fillRitualSpellcastingMap()
        {
            ritual_spellcastings_map[RuleDefinitions.RitualCasting.Prepared] = DatabaseHelper.FeatureDefinitionMagicAffinitys.MagicAffinityClericRitualCasting;
            ritual_spellcastings_map[RuleDefinitions.RitualCasting.Spellbook] = DatabaseHelper.FeatureDefinitionMagicAffinitys.MagicAffinityWizardRitualCasting;

            var spontaneous_ritual_spellcsting = Helpers.CopyFeatureBuilder<FeatureDefinitionMagicAffinity>.createFeatureCopy("MagicAffinitySpontaneousRitualCasting",
                                                                                                                              "efd3d247-d74f-47ac-b575-159fcad3608f",
                                                                                                                              "",
                                                                                                                              "",
                                                                                                                              null,
                                                                                                                              DatabaseHelper.FeatureDefinitionMagicAffinitys.MagicAffinityClericRitualCasting
                                                                                                                              );
            Helpers.Accessors.SetField(spontaneous_ritual_spellcsting, "ritualCasting", (RuleDefinitions.RitualCasting)ExtraRitualCasting.Spontaneous);
            ritual_spellcastings_map[(RuleDefinitions.RitualCasting)ExtraRitualCasting.Spontaneous] = spontaneous_ritual_spellcsting;
        }
    }
}

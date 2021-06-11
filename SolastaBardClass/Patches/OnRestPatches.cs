using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.Patches
{
    class RestModuleHitDicePatcher
    {
        [HarmonyPatch(typeof(RestModuleHitDice), "Bind")]
        internal static class RestModuleHitDice_Bind_Patch
        {
            static Dictionary<RulesetCharacterHero, Dictionary<string, RuleDefinitions.DieType>> extra_healing_dice_per_hero;
         
            internal static void Postfix(RestModuleHitDice __instance, RuleDefinitions.RestType restType,
                                         RestDefinitions.RestStage restStage,
                                         RestModule.RestModuleRefreshedHandler restModuleRefreshed)
            {
                extra_healing_dice_per_hero = new Dictionary<RulesetCharacterHero, Dictionary<string, RuleDefinitions.DieType>>();
                foreach (var h in __instance.Heroes)
                {
                    extra_healing_dice_per_hero[h] = new Dictionary<string, RuleDefinitions.DieType>();
                }
                foreach (var h in __instance.Heroes)
                {
                    h.EnumerateFeaturesToBrowse<FeatureDefinitionExtraHealingDieOnShortRest>(h.FeaturesToBrowse);

                    foreach (FeatureDefinitionExtraHealingDieOnShortRest f in h.FeaturesToBrowse)
                    {
                        Main.Logger.Log("Found: " + f.name + " on " + h.Name);
                        if (f.ApplyToParty)
                        {
                            foreach (var hh in __instance.Heroes)
                            {
                                if (!extra_healing_dice_per_hero[hh].ContainsKey(f.tag) || extra_healing_dice_per_hero[hh][f.tag] < f.DieType)
                                {
                                    extra_healing_dice_per_hero[hh][f.tag] = f.DieType;
                                }
                            }
                        }
                        else
                        {
                            if (!extra_healing_dice_per_hero[h].ContainsKey(f.tag) || extra_healing_dice_per_hero[h][f.tag] < f.DieType)
                            {
                                extra_healing_dice_per_hero[h][f.tag] = f.DieType;
                            }
                        }
                    }
                }
            }
        }
    }
}

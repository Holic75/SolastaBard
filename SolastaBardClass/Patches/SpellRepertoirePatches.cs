using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.Patches
{
    class SpellRepertoirePatches
    {
        class SpellsByLevelGroupBindLearningPatcher
        {
            [HarmonyPatch(typeof(SpellsByLevelGroup), "BindLearning")]
            internal static class SpellsByLevelGroup_BindLearning_Patch
            {
                internal static bool Prefix(ICharacterBuildingService characterBuildingService,
                                            ref SpellListDefinition spellListDefinition,
                                            List<string> restrictedSchools,
                                            int spellLevel,
                                            SpellBox.SpellBoxChangedHandler spellBoxChanged,
                                            List<SpellDefinition> knownSpells,
                                            string spellTag,
                                            bool canAcquireSpells)
                {
                    var hero = characterBuildingService.HeroCharacter;
                    if (hero == null)
                    {
                        return true;
                    }

                    var extra_spell_list = hero.ActiveFeatures.Values.Aggregate(new List<NewFeatureDefinitions.IReplaceSpellList>(),
                                                                               (old, next) =>
                                                                               {
                                                                                   old.AddRange(next.OfType<NewFeatureDefinitions.IReplaceSpellList>());
                                                                                   return old;
                                                                               }).Select(rs => rs.getSpelllist(characterBuildingService)).FirstOrDefault(s => s != null);

                    if (extra_spell_list == null)
                    {
                        return true;
                    }

                    spellListDefinition = extra_spell_list;
                    return true;
                }
            }
        }


        class CharacterBuildingManagerSetPointPoolPatcher
        {
            [HarmonyPatch(typeof(CharacterBuildingManager), "SetPointPool")]
            internal static class CharacterBuildingManager_SetPointPool_Patch
            {
                internal static bool Prefix(CharacterBuildingManager __instance, HeroDefinitions.PointsPoolType pointPoolType, string tag, ref int maxNumber)
                {
                    if (pointPoolType != HeroDefinitions.PointsPoolType.Spell)
                    {
                        return true;
                    }
                    var hero = __instance.HeroCharacter;
                    if (hero == null)
                    {
                        return true;
                    }

                    int bonus_known_spells = hero.ActiveFeatures.Values.Aggregate(new List<NewFeatureDefinitions.IKnownSpellNumberIncrease>(),
                                                                                (old, next) =>
                                                                                {
                                                                                    old.AddRange(next.OfType<NewFeatureDefinitions.IKnownSpellNumberIncrease>());
                                                                                    return old;
                                                                                }).Aggregate(0, (old, next) => old += next.getKnownSpellsBonus(hero));

                    maxNumber = maxNumber + bonus_known_spells;
                    return true;
                }
            }
        }
    }
}

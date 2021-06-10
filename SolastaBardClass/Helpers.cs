using SolastaModApi;
using SolastaModApi.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.AddressableAssets;
using static FeatureDefinitionAbilityCheckAffinity;
using static FeatureDefinitionSavingThrowAffinity;

namespace SolastaBardClass.Helpers
{
    public static class Stats
    {
        public static string Strength = "Strength";
        public static string Dexterity = "Dexterity";
        public static string Constitution = "Constitution";
        public static string Wisdom = "Wisdom";
        public static string Intelligence = "Intelligence";
        public static string Charisma = "Charisma";

        public static string[] getAllStats()
        {
            return typeof(Stats).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Cast<string>().ToArray();
        }

        public static HashSet<string> getAllStatsSet()
        {
            return getAllStats().ToHashSet();
        }

        public static void assertAllStats(IEnumerable<string> stats)
        {
            var all_stats = getAllStatsSet();
            foreach (var s in stats)
            {
                if (!all_stats.Contains(s))
                {
                    throw new System.Exception(s + "is not an Ability");
                }
            }
        }
    }

    public static class Skills
    {
        public static string Acrobatics = "Acrobatics";
        public static string Arcana = "Arcana";
        public static string AnimalHandling = "AnimalHandling";
        public static string Athletics = "Athletics";
        public static string Deception = "Deception";
        public static string History = "History";
        public static string Insight = "Insight";
        public static string Intimidation = "Intimidation";
        public static string Investigation = "Investigation";
        public static string Medicine = "Medecine";
        public static string Nature = "Nature";
        public static string Perception = "Perception";
        public static string Perfromance = "Performance";
        public static string Persuasion = "Persuasion";
        public static string Religion = "Religion";
        public static string SleightOfHand = "SleightOfHand";
        public static string Stealth = "Stealth";
        public static string Survival = "Survival";

        public static Dictionary<string, string> skill_stat_map = new Dictionary<string, string>
        {
            {Acrobatics, Stats.Dexterity },
            {Arcana, Stats.Intelligence },
            {AnimalHandling, Stats.Wisdom },
            {Athletics, Stats.Strength },
            {Deception, Stats.Charisma },
            {History, Stats.Intelligence },
            {Insight, Stats.Wisdom },
            {Intimidation, Stats.Charisma },
            {Investigation, Stats.Intelligence },
            {Medicine, Stats.Wisdom },
            {Nature, Stats.Intelligence },
            {Perception, Stats.Wisdom },
            {Perfromance, Stats.Charisma },
            {Persuasion, Stats.Charisma },
            {Religion, Stats.Wisdom },
            {SleightOfHand, Stats.Dexterity },
            {Stealth, Stats.Dexterity },
            {Survival, Stats.Wisdom }
        };

        public static string[] getAllSkills()
        {
            return typeof(Skills).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Where(f => f is string).Cast<string>().ToArray();
        }

        public static HashSet<string> getAllSkillsSet()
        {
            return getAllSkills().ToHashSet();
        }

        public static void assertAllSkills(IEnumerable<string> skills)
        {
            var all_stats = getAllSkillsSet();
            foreach (var s in skills)
            {
                if (!all_stats.Contains(s))
                {
                    throw new System.Exception(s + "is not a Skill");
                }
            }
        }
    }

    public static class Tools
    {
        public static string ScrollKit = "ScrollKitType";
        public static string EnchantingTool = "EnchantingToolType";
        public static string SmithTool = "ArtisanToolSmithToolsType";
        public static string ThievesTool = "ThievesToolsType";
        public static string HerbalismKit = "HerbalismKitType";
        public static string PoisonerKit = "PoisonersKitType";

        public static string[] getAllTools()
        {
            return typeof(Tools).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Cast<string>().ToArray();
        }

        public static HashSet<string> getAllToolsSet()
        {
            return getAllTools().ToHashSet();
        }

        public static void assertAllTools(IEnumerable<string> tools)
        {
            var all_stats = getAllTools();
            foreach (var s in tools)
            {
                if (!all_stats.Contains(s))
                {
                    throw new System.Exception(s + "is not a Tool");
                }
            }
        }
    }



    public class ProficiencyBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionProficiency>
    {
        protected ProficiencyBuilder(string name, string guid, string title_string, string description_string, FeatureDefinitionProficiency base_feature, params string[] proficiencies) 
                : base(base_feature, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = description_string;
            }
            Definition.Proficiencies.Clear();
            Definition.Proficiencies.AddRange(proficiencies);
        }


        public static FeatureDefinitionProficiency CreateProficiency(string name, string guid, string title_string, string description_string,
                                                                        FeatureDefinitionProficiency base_feature, params string[] proficiencies)
        {
            return new ProficiencyBuilder(name, guid, title_string, description_string, base_feature, proficiencies).AddToDB();
        }


        public static FeatureDefinitionProficiency CreateSavingthrowProficiency(string name, string guid, params string[] stats)
        {
            Stats.assertAllStats(stats);
            return new ProficiencyBuilder(name, guid, "", "", DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueSavingThrow, stats).AddToDB();
        }


        public static FeatureDefinitionProficiency CreateToolsProficiency(string name, string guid, string title_string, params string[] tools)
        {
            Tools.assertAllTools(tools);
            return new ProficiencyBuilder(name, guid, title_string, "", DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueTools, tools).AddToDB();
        }


        public static FeatureDefinitionProficiency createCopy(string name, string guid, string new_title_string, string new_description_string, FeatureDefinitionProficiency base_feature)
        {
            return new ProficiencyBuilder(name, guid, new_title_string, new_description_string, base_feature, base_feature.Proficiencies.ToArray()).AddToDB();
        }
    }


    public class PoolBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionPointPool>
    {
        protected PoolBuilder(string name, string guid, string title_string, string description_string, 
                                        FeatureDefinitionPointPool base_feature, HeroDefinitions.PointsPoolType pool_type,
                                        int num_choices, params string[] choices)
                : base(base_feature, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = description_string;
            }
            Definition.SetPoolAmount(num_choices);
            Definition.SetPoolType(pool_type);
            Definition.RestrictedChoices.Clear();
            Definition.RestrictedChoices.AddRange(choices);
            Definition.RestrictedChoices.Sort();
        }


        public static FeatureDefinitionPointPool createSkillProficiency(string name, string guid, string new_title_string, string new_description_string, int num_skills, params string[] skills)
        {
            Skills.assertAllSkills(skills);
            return new PoolBuilder(name, guid, new_title_string, new_description_string, DatabaseHelper.FeatureDefinitionPointPools.PointPoolRogueSkillPoints, 
                                      HeroDefinitions.PointsPoolType.Skill, num_skills, skills).AddToDB();
        }
    }


    public class RitualSpellcastingBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionFeatureSet>
    {
        protected RitualSpellcastingBuilder(string name, string guid, string description_string) : base(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetWizardRitualCasting, name, guid)
        {
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = description_string;
            }
        }

        public static FeatureDefinitionFeatureSet createRitualSpellcasting(string name, string guid, string description_string)
        {
            return new RitualSpellcastingBuilder(name, guid, description_string).AddToDB();
        }
    }


    public class SpelllistBuilder : BaseDefinitionBuilderWithGuidStorage<SpellListDefinition>
    {
        protected SpelllistBuilder(string name, string guid, string title_string, SpellListDefinition base_list, params List<SpellDefinition>[] spells_by_level) : base(DatabaseHelper.SpellListDefinitions.SpellListWizard, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }

            for (int i = 0; i < Definition.SpellsByLevel.Count; i++)
            {
                Definition.SpellsByLevel[i].Spells.Clear();
                if (spells_by_level.Length > i)
                {
                    Definition.SpellsByLevel[i].Spells.AddRange(spells_by_level[i]);
                }
            }
        }

        public static SpellListDefinition create9LevelSpelllist(string name, string guid, string title_string, params List<SpellDefinition>[] spells_by_level)
        {
            return new SpelllistBuilder(name, guid, title_string, DatabaseHelper.SpellListDefinitions.SpellListWizard, spells_by_level).AddToDB();
        }
    }


    class SpellcastingBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionCastSpell>
    {
        
        protected SpellcastingBuilder(string name, string guid, string title_string, string description_string, SpellListDefinition spelllist,
                                      string spell_stat, RuleDefinitions.SpellKnowledge spell_knowledge, RuleDefinitions.SpellReadyness spell_readyness,
                                      List<int> scribed_spells, List<int> cantrips_per_level, List<int> known_spells,
                                      FeatureDefinitionCastSpell base_feature) : base(base_feature, name, guid)
        {
            Definition.GuiPresentation.Title = title_string;
            Definition.GuiPresentation.Description = description_string;

            Definition.SetSpellcastingAbility(spell_stat);
            Definition.SetSpellKnowledge(spell_knowledge);
            Definition.SetSpellReadyness(spell_readyness);
            Definition.ScribedSpells.Clear();
            Definition.ScribedSpells.AddRange(scribed_spells);
            Definition.KnownSpells.Clear();
            Definition.KnownSpells.AddRange(known_spells);
            Definition.SetSpellListDefinition(spelllist);
        }

        public static FeatureDefinitionCastSpell create9LevelSpontSpellcasting(string name, string guid, string title_string, string description_string,
                                                                               SpellListDefinition spelllist, string spell_stat,
                                                                               List<int> known_spells)
        {
            Stats.assertAllStats(new string[] { spell_stat });
            return new SpellcastingBuilder(name, guid, title_string, description_string, spelllist, spell_stat,
                                           RuleDefinitions.SpellKnowledge.Selection, RuleDefinitions.SpellReadyness.AllKnown,
                                           Enumerable.Repeat(0, 20).ToList(),
                                           DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard.KnownCantrips,
                                           known_spells, DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard).AddToDB();
        }
    }


    public class SavingThrowAffinityBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionSavingThrowAffinity>
    {


        protected SavingThrowAffinityBuilder(string name, string guid, 
                                             string title_string, string description_string, 
                                             AssetReferenceSprite sprite,
                                             RuleDefinitions.CharacterSavingThrowAffinity affinity,
                                             int dice_number,
                                             RuleDefinitions.DieType die_type,
                                             params string[] stats) : base(DatabaseHelper.FeatureDefinitionSavingThrowAffinitys.SavingThrowAffinityCreedOfSolasta, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = title_string;
            }
            if (sprite != null)
            {
                Definition.GuiPresentation.SetSpriteReference(sprite);
            }

            Definition.AffinityGroups.Clear();
            foreach (var s in stats)
            {
                var group = new SavingThrowAffinityGroup();
                group.savingThrowModifierDieType = die_type;
                group.savingThrowModifierDiceNumber = dice_number;
                group.affinity = affinity;
                group.abilityScoreName = s;
                Definition.AffinityGroups.Add(group);
            }          
        }

        public static FeatureDefinitionSavingThrowAffinity createSavingthrowAffinity(string name, string guid,
                                                                                     string title_string, string description_string,
                                                                                     AssetReferenceSprite sprite,
                                                                                     RuleDefinitions.CharacterSavingThrowAffinity affinity,
                                                                                     int dice_number,
                                                                                     RuleDefinitions.DieType die_type,
                                                                                     params string[] stats)
        {
            Stats.assertAllStats(stats);
            return new SavingThrowAffinityBuilder(name, guid, title_string, description_string, sprite, affinity, dice_number, die_type, stats).AddToDB();
        }
    }



    public class AbilityCheckAffinityBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionAbilityCheckAffinity>
    {
        protected AbilityCheckAffinityBuilder(string name, string guid,
                                             string title_string, string description_string,
                                             AssetReferenceSprite sprite,
                                             RuleDefinitions.CharacterAbilityCheckAffinity affinity,
                                             int dice_number,
                                             RuleDefinitions.DieType die_type,
                                             List<string> stats,
                                             List<string> proficiencies) : base(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityGuided, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = title_string;
            }
            if (sprite != null)
            {
                Definition.GuiPresentation.SetSpriteReference(sprite);
            }


            Definition.AffinityGroups.Clear();
            for (int i = 0; i < stats.Count; i++)
            {
                var group = new AbilityCheckAffinityGroup();
                group.abilityScoreName = stats[i];
                group.abilityCheckModifierDiceNumber = dice_number;
                group.proficiencyName = proficiencies[i];
                group.abilityCheckModifierDieType = die_type;
                group.affinity = affinity;
                Definition.AffinityGroups.Add(group);
            }
        }

        public static FeatureDefinitionAbilityCheckAffinity createAbilityCheckAffinity(string name, string guid,
                                                                                         string title_string, string description_string,
                                                                                         AssetReferenceSprite sprite,
                                                                                         RuleDefinitions.CharacterAbilityCheckAffinity affinity,
                                                                                         int dice_number,
                                                                                         RuleDefinitions.DieType die_type,
                                                                                         params string[] stats)
        {
            Stats.assertAllStats(stats);
            return new AbilityCheckAffinityBuilder(name, guid, title_string, description_string, sprite, affinity, dice_number, die_type, 
                                                    stats.ToList(), Enumerable.Repeat("", stats.Length).ToList()).AddToDB();
        }


        public static FeatureDefinitionAbilityCheckAffinity createSkillCheckAffinity(string name, string guid,
                                                                             string title_string, string description_string,
                                                                             AssetReferenceSprite sprite,
                                                                             RuleDefinitions.CharacterAbilityCheckAffinity affinity,
                                                                             int dice_number,
                                                                             RuleDefinitions.DieType die_type,
                                                                             params string[] skills)
        {
            Skills.assertAllSkills(skills);
            return new AbilityCheckAffinityBuilder(name, guid, title_string, description_string, sprite, affinity, dice_number, die_type,
                                                    skills.Select(s => Skills.skill_stat_map[s]).ToList(), skills.ToList()).AddToDB();
        }
    }


    public class AttackBonusBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionCombatAffinity>
    {
        protected AttackBonusBuilder(string name, string guid,
                                             string title_string, string description_string,
                                             AssetReferenceSprite sprite,
                                             int dice_number,
                                             RuleDefinitions.DieType die_type,
                                             bool substract = false) : base(DatabaseHelper.FeatureDefinitionCombatAffinitys.CombatAffinityBlessed, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = title_string;
            }
            if (sprite != null)
            {
                Definition.GuiPresentation.SetSpriteReference(sprite);
            }
            Definition.SetMyAttackModifierDiceNumber(dice_number);
            Definition.SetMyAttackModifierDieType(die_type);
            if (substract)
            {
                Definition.SetMyAttackModifierSign(RuleDefinitions.AttackModifierSign.Substract);
            }
           
        }


        public static FeatureDefinitionCombatAffinity createAttackBonus(string name, string guid,
                                                                                 string title_string, string description_string,
                                                                                 AssetReferenceSprite sprite,
                                                                                 int dice_number,
                                                                                 RuleDefinitions.DieType die_type,
                                                                                 bool substract = false)
        {
            return new AttackBonusBuilder(name, guid, title_string, description_string, sprite, dice_number, die_type, substract).AddToDB();
        }
    }


    public class ConditionBuilder : BaseDefinitionBuilderWithGuidStorage<ConditionDefinition>
    {
        protected ConditionBuilder(string name, string guid,
                                   string title_string, string description_string,
                                   AssetReferenceSprite sprite,
                                   ConditionDefinition base_condititon,
                                   RuleDefinitions.ConditionInterruption[] interruptions,
                                   FeatureDefinition[] features) : base(base_condititon, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = title_string;
            }
            if (sprite != null)
            {
                Definition.GuiPresentation.SetSpriteReference(sprite);
            }
            Definition.SpecialInterruptions.Clear();
            Definition.SpecialInterruptions.AddRange(interruptions);
            Definition.Features.Clear();
            Definition.Features.AddRange(features);
        }


        public static ConditionDefinition createCondition(string name, string guid,
                                                            string title_string, string description_string,
                                                            AssetReferenceSprite sprite,
                                                            ConditionDefinition base_condititon,
                                                            params FeatureDefinition[] features)
        {
            return new ConditionBuilder(name, guid, title_string, description_string, sprite,base_condititon, new RuleDefinitions.ConditionInterruption[0], features).AddToDB();
        }


        public static ConditionDefinition createConditionWithInterruptions(string name, string guid,
                                                                          string title_string, string description_string,
                                                                          AssetReferenceSprite sprite,
                                                                          ConditionDefinition base_condititon,
                                                                          RuleDefinitions.ConditionInterruption[] interruptions,
                                                                          params FeatureDefinition[] features)
        {
            return new ConditionBuilder(name, guid, title_string, description_string, sprite, base_condititon, interruptions, features).AddToDB();
        }
    }



    public class PowerBuilder : BaseDefinitionBuilderWithGuidStorage<FeatureDefinitionPower>
    {
        protected PowerBuilder(string name, string guid, string title_string, string description_string, AssetReferenceSprite sprite,
                               FeatureDefinitionPower base_power,
                               EffectDescription effect_description,
                               RuleDefinitions.ActivationTime activation_time,
                               int fixed_uses, 
                               RuleDefinitions.UsesDetermination uses_determination,
                               RuleDefinitions.RechargeRate recharge_rate,
                               string uses_ability,
                               string ability,
                               int cost_per_use = 1,
                               bool show_casting = true) : base(base_power, name, guid)
        {
            if (title_string != "")
            {
                Definition.GuiPresentation.Title = title_string;
            }
            if (description_string != "")
            {
                Definition.GuiPresentation.Description = description_string;
            }
            if (sprite != null)
            {
                Definition.GuiPresentation.SetSpriteReference(sprite);
            }

            Definition.SetRechargeRate(recharge_rate);
            Definition.SetCostPerUse(cost_per_use);
            Definition.SetFixedUsesPerRecharge(fixed_uses);
            Definition.SetActivationTime(activation_time);
            Definition.SetUsesDetermination(uses_determination);
            Definition.SetShowCasting(show_casting);
            Definition.SetAbilityScore(ability);
            Definition.SetUsesAbilityScoreName(uses_ability);
            Definition.SetEffectDescription(effect_description);
        }

        public static FeatureDefinitionPower createPower(string name, string guid,
                                                         string title_string, string description_string, AssetReferenceSprite sprite,
                                                         FeatureDefinitionPower base_power,
                                                         EffectDescription effect_description,
                                                         RuleDefinitions.ActivationTime activation_time,
                                                         int fixed_uses,
                                                         RuleDefinitions.UsesDetermination uses_determination,
                                                         RuleDefinitions.RechargeRate recharge_rate,
                                                         string uses_ability = "Strength",
                                                         string ability = "Strength",
                                                         int cost_per_use = 1,
                                                         bool show_casting = true)
        {
            return new PowerBuilder(name, guid, title_string, description_string, sprite, base_power, effect_description, 
                                    activation_time, fixed_uses, uses_determination, recharge_rate,
                                    uses_ability,ability, cost_per_use, show_casting).AddToDB();
        }
    }
}

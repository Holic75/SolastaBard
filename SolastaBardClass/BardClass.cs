using SolastaModApi;
using SolastaModApi.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using static FeatureDefinitionSavingThrowAffinity;

namespace SolastaBardClass
{
    internal class BardClassBuilder : CharacterClassDefinitionBuilder
    {
        const string BardClassName = "BardClass";
        const string BardClassNameGuid = "274106b8-0376-4bcd-bd1b-440633a394ae";
        const string BardClassSubclassesGuid = "be865126-d7c3-45f3-b891-e77bd8b00cb1";

        static public CharacterClassDefinition bard_class;
        static public Dictionary<RuleDefinitions.DieType, FeatureDefinitionPower> inspiration_powers = new Dictionary<RuleDefinitions.DieType, FeatureDefinitionPower>();
        static public FeatureDefinition font_of_inspiration;
        static public FeatureDefinitionPointPool expertise;
        static public FeatureDefinitionAbilityCheckAffinity jack_of_all_trades;
        static public Dictionary<RuleDefinitions.DieType, NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest> song_of_rest = new Dictionary<RuleDefinitions.DieType, NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest>();
        static public SpellListDefinition bard_spelllist;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection magical_secrets;
        static public FeatureDefinitionPower countercharm;
        //TODO
        //colleges: lore, virtue, wyrdsingers ?, ..


        protected BardClassBuilder(string name, string guid) : base(name, guid)
        {
            var rogue = DatabaseHelper.CharacterClassDefinitions.Rogue;
            bard_class = Definition;
            Definition.GuiPresentation.Title = "Class/&BardClassTitle";
            Definition.GuiPresentation.Description = "Class/&BardClassDescription";
            Definition.GuiPresentation.SetSpriteReference(rogue.GuiPresentation.SpriteReference);

            Definition.SetClassAnimationId(AnimationDefinitions.ClassAnimationId.Fighter);
            Definition.SetClassPictogramReference(rogue.ClassPictogramReference);
            Definition.SetDefaultBattleDecisions(rogue.DefaultBattleDecisions);
            Definition.SetHitDice(RuleDefinitions.DieType.D8);
            Definition.SetIngredientGatheringOdds(rogue.IngredientGatheringOdds);
            Definition.SetRequiresDeity(false);

            Definition.AbilityScoresPriority.Clear();
            Definition.AbilityScoresPriority.AddRange(new List<string> {Helpers.Stats.Charisma,
                                                                        Helpers.Stats.Dexterity,
                                                                        Helpers.Stats.Constitution,
                                                                        Helpers.Stats.Intelligence,
                                                                        Helpers.Stats.Strength,
                                                                        Helpers.Stats.Wisdom});

            Definition.FeatAutolearnPreference.AddRange(rogue.FeatAutolearnPreference);
            Definition.PersonalityFlagOccurences.AddRange(rogue.PersonalityFlagOccurences);

            Definition.SkillAutolearnPreference.Clear();
            Definition.SkillAutolearnPreference.AddRange(new List<string> { Helpers.Skills.Persuasion,
                                                                            Helpers.Skills.Deception,
                                                                            Helpers.Skills.Acrobatics,
                                                                            Helpers.Skills.Stealth,
                                                                            Helpers.Skills.Intimidation,
                                                                            Helpers.Skills.Arcana,
                                                                            Helpers.Skills.History,
                                                                            Helpers.Skills.Insight });

            Definition.ToolAutolearnPreference.Clear();
            Definition.ToolAutolearnPreference.AddRange(new List<string> { Helpers.Tools.ThievesTool, Helpers.Tools.EnchantingTool, Helpers.Tools.Lyre });


            Definition.EquipmentRows.AddRange(rogue.EquipmentRows);
            Definition.EquipmentRows.Clear();

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Rapier, EquipmentDefinitions.OptionWeapon, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Longsword, EquipmentDefinitions.OptionWeapon, 1),
                                    }
            );
            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ExplorerPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.BurglarPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    }
            );
            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.EnchantingTool, EquipmentDefinitions.OptionTool, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                         EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ThievesTool, EquipmentDefinitions.OptionTool, 1),
                                    }
            );

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
            {
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Dagger, EquipmentDefinitions.OptionWeapon, 1),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Leather, EquipmentDefinitions.OptionArmor, 1),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ComponentPouch, EquipmentDefinitions.OptionFocus, 1)
            });

            var saving_throws = Helpers.ProficiencyBuilder.CreateSavingthrowProficiency("BardSavingthrowProficiency",
                                                                                        "88d8752b-4956-4daf-91fc-84e6196c3985",
                                                                                        Helpers.Stats.Charisma, Helpers.Stats.Dexterity);

            var armor_proficiency = Helpers.ProficiencyBuilder.createCopy("BardArmorProficiency",
                                                                          "06d31b31-69db-40d7-8701-a8547c4dd063",
                                                                          "Feature/&BardArmorProficiencyTitle",
                                                                          "",
                                                                          DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueArmor
                                                                          );

            var weapon_proficiency = Helpers.ProficiencyBuilder.createCopy("BardWeaponProficiency",
                                                                          "9a0ef52f-052a-4838-b3d4-2096ab67453e",
                                                                          "Feature/&BardWeaponProficiencyTitle",
                                                                          "",
                                                                          DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueWeapon
                                                                          );

            var tools_proficiency = Helpers.ProficiencyBuilder.CreateToolsProficiency("BardToolsProficiency",
                                                                                      "96d8987b-e682-44a6-afdb-763cbe5361ad",
                                                                                      "Feature/&BardToolsProficiencyTitle",
                                                                                      Helpers.Tools.EnchantingTool, Helpers.Tools.ThievesTool, Helpers.Tools.Lyre
                                                                                      );

            var skills = Helpers.PoolBuilder.createSkillProficiency("BardSkillProficiency",
                                                                    "029f6c7e-f1fc-4030-9012-9c698c714f00",
                                                                    "Feature/&BardClassSkillPointPoolTitle",
                                                                    "Feature/&BardClassSkillPointPoolDescription",
                                                                    3,
                                                                    Helpers.Skills.getAllSkills());

            expertise = Helpers.CopyFeatureBuilder<FeatureDefinitionPointPool>.createFeatureCopy("BardExpertise",
                                                                                                 "",
                                                                                                 "Feature/&BardClassExpertisePointPoolTitle",
                                                                                                 "Feature/&BardClassExpertisePointPoolDescription",
                                                                                                 null,
                                                                                                 DatabaseHelper.FeatureDefinitionPointPools.PointPoolRogueExpertise);
            expertise.RestrictedChoices.Clear();
            expertise.RestrictedChoices.Add(Helpers.Tools.Lyre);


            var ritual_spellcasting = Helpers.RitualSpellcastingBuilder.createRitualSpellcasting("BardRitualSpellcasting", 
                                                                                                 "25c48b9b-e2e9-4ea7-8a80-e6c413275980",
                                                                                                 "Feature/&BardClassRitualCastingDescription",
                                                                                                 (RuleDefinitions.RitualCasting)ExtraRitualCasting.Spontaneous);

            bard_spelllist = Helpers.SpelllistBuilder.create9LevelSpelllist("BardClassSpelllist", "0f3d14a7-f9a1-41ec-a164-f3e0f3800104", "",
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.AnnoyingBee,
                                                                                    DatabaseHelper.SpellDefinitions.DancingLights,
                                                                                    DatabaseHelper.SpellDefinitions.Dazzle,
                                                                                    DatabaseHelper.SpellDefinitions.Light,
                                                                                    DatabaseHelper.SpellDefinitions.ShadowArmor,
                                                                                    DatabaseHelper.SpellDefinitions.ShadowDagger,
                                                                                    DatabaseHelper.SpellDefinitions.Shine,
                                                                                    DatabaseHelper.SpellDefinitions.Sparkle,
                                                                                    DatabaseHelper.SpellDefinitions.TrueStrike
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.AnimalFriendship,
                                                                                    DatabaseHelper.SpellDefinitions.Bane,
                                                                                    DatabaseHelper.SpellDefinitions.CharmPerson,
                                                                                    DatabaseHelper.SpellDefinitions.ColorSpray,
                                                                                    DatabaseHelper.SpellDefinitions.Command,
                                                                                    DatabaseHelper.SpellDefinitions.ComprehendLanguages,
                                                                                    DatabaseHelper.SpellDefinitions.CureWounds,
                                                                                    DatabaseHelper.SpellDefinitions.DetectMagic,
                                                                                    DatabaseHelper.SpellDefinitions.FaerieFire,
                                                                                    DatabaseHelper.SpellDefinitions.FeatherFall,
                                                                                    DatabaseHelper.SpellDefinitions.HealingWord,
                                                                                    DatabaseHelper.SpellDefinitions.Heroism,
                                                                                    DatabaseHelper.SpellDefinitions.Identify,
                                                                                    DatabaseHelper.SpellDefinitions.Longstrider,
                                                                                    DatabaseHelper.SpellDefinitions.Sleep,
                                                                                    DatabaseHelper.SpellDefinitions.HideousLaughter,
                                                                                    DatabaseHelper.SpellDefinitions.Thunderwave
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Aid,
                                                                                    DatabaseHelper.SpellDefinitions.Blindness,
                                                                                    DatabaseHelper.SpellDefinitions.CalmEmotions,
                                                                                    DatabaseHelper.SpellDefinitions.EnhanceAbility,
                                                                                    DatabaseHelper.SpellDefinitions.HoldPerson,
                                                                                    DatabaseHelper.SpellDefinitions.Invisibility,
                                                                                    DatabaseHelper.SpellDefinitions.Knock,
                                                                                    DatabaseHelper.SpellDefinitions.LesserRestoration,
                                                                                    //DatabaseHelper.SpellDefinitions.MirrorImage,
                                                                                    DatabaseHelper.SpellDefinitions.SeeInvisibility,
                                                                                    DatabaseHelper.SpellDefinitions.Shatter,
                                                                                    DatabaseHelper.SpellDefinitions.Silence
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.BestowCurse,
                                                                                    DatabaseHelper.SpellDefinitions.DispelMagic,
                                                                                    DatabaseHelper.SpellDefinitions.Fear,
                                                                                    DatabaseHelper.SpellDefinitions.HypnoticPattern,
                                                                                    DatabaseHelper.SpellDefinitions.MassHealingWord,
                                                                                    DatabaseHelper.SpellDefinitions.Slow,
                                                                                    DatabaseHelper.SpellDefinitions.StinkingCloud,
                                                                                    DatabaseHelper.SpellDefinitions.Tongues
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Confusion,
                                                                                    DatabaseHelper.SpellDefinitions.DimensionDoor,
                                                                                    DatabaseHelper.SpellDefinitions.FreedomOfMovement,
                                                                                    DatabaseHelper.SpellDefinitions.GreaterInvisibility,
                                                                                    DatabaseHelper.SpellDefinitions.PhantasmalKiller
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.DominatePerson,
                                                                                    DatabaseHelper.SpellDefinitions.GreaterRestoration,
                                                                                    DatabaseHelper.SpellDefinitions.HoldMonster,
                                                                                    DatabaseHelper.SpellDefinitions.MassCureWounds,
                                                                                    DatabaseHelper.SpellDefinitions.RaiseDead
                                                                                }
                                                                                );

            var bard_spellcasting = Helpers.SpellcastingBuilder.createSpontaneousSpellcasting("BardClassSpellcasting",
                                                                                              "f720edaf-92c4-43e3-8228-c48c0b41b93b",
                                                                                              "Feature/&BardClassSpellcastingTitle",
                                                                                              "Feature/&BardClassSpellcastingDescription",
                                                                                              bard_spelllist,
                                                                                              Helpers.Stats.Charisma,
                                                                                              DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard.KnownCantrips,
                                                                                              new List<int> {4,  5,  6,  7,  8,  9,  10, 11, 12, 12,
                                                                                                                         13, 13, 14, 14, 15, 15, 16, 16, 16, 16},
                                                                                              DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard.SlotsPerLevels
                                                                                              );

            jack_of_all_trades = Helpers.AbilityCheckAffinityBuilder.createAbilityCheckAffinity("BardClassJackOfAllTradesFeature",
                                                                                                 "",
                                                                                                "Feature/&BardClassJackOfAllTradesFeatureTitle",
                                                                                                "Feature/&BardClassJackOfAllTradesFeatureDescription",
                                                                                                 null,
                                                                                                 RuleDefinitions.CharacterAbilityCheckAffinity.HalfProficiencyWhenNotProficient,
                                                                                                 0,
                                                                                                 RuleDefinitions.DieType.D1,
                                                                                                 Helpers.Stats.getAllStats().ToArray()
                                                                                                 );



            createInspiration();
            createSongOfRest();
            createMagicalSecrets();
            createCountercharm();
            Definition.FeatureUnlocks.Clear();
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(saving_throws, 1)); 
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(armor_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(weapon_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(skills, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(tools_proficiency, 1)); 
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(bard_spellcasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(ritual_spellcasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(inspiration_powers[RuleDefinitions.DieType.D6], 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(song_of_rest[RuleDefinitions.DieType.D6], 2));           
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(jack_of_all_trades, 3));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(expertise, 3));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 4));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(inspiration_powers[RuleDefinitions.DieType.D8], 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(font_of_inspiration, 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(countercharm, 6));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 8));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(song_of_rest[RuleDefinitions.DieType.D8], 9));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets, 10));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(inspiration_powers[RuleDefinitions.DieType.D10], 10));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(expertise, 10));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 12));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 19));

            var subclassChoicesGuiPresentation = new GuiPresentation();
            subclassChoicesGuiPresentation.Title = "Subclass/&BardSubclassPathTitle";
            subclassChoicesGuiPresentation.Description = "Subclass/&BardSubclassPathDescription";
            BardFeatureDefinitionSubclassChoice = this.BuildSubclassChoice(3, "Path", false, "SubclassChoiceBardSpecialistArchetypes", subclassChoicesGuiPresentation, BardClassSubclassesGuid);
        }


        static void createCountercharm()
        {
            string countercharm_title_string = "Feature/&BardClassCountercharmPowerTitle";
            string countercharm_description_string = "Feature/&BardClassCountercharmPowerDescription";
            string countercharm_effect_description_string = "Feature/&BardClassCountercharmEffectDescription";

            var affinity_frightened = Helpers.ConditionAffinityBuilder.createConditionAffinity("BardClassCountercharmFrightenedAffinity",
                                                                                            "",
                                                                                            "",
                                                                                            "",
                                                                                            null,
                                                                                            Helpers.Conditions.Frightened,
                                                                                            RuleDefinitions.ConditionAffinityType.None,
                                                                                            RuleDefinitions.AdvantageType.Advantage,
                                                                                            RuleDefinitions.AdvantageType.None
                                                                                            );
            var affinity_charmed = Helpers.ConditionAffinityBuilder.createConditionAffinity("BardClassCountercharmCharmedAffinity",
                                                                                            "",
                                                                                            "",
                                                                                            "",
                                                                                            null,
                                                                                            Helpers.Conditions.Charmed,
                                                                                            RuleDefinitions.ConditionAffinityType.None,
                                                                                            RuleDefinitions.AdvantageType.Advantage,
                                                                                            RuleDefinitions.AdvantageType.None
                                                                                            );

            var effect_condition = Helpers.ConditionBuilder.createCondition("BardClassCountercharmEffectCondition",
                                                                            "",
                                                                            countercharm_title_string,
                                                                            countercharm_effect_description_string,
                                                                            null,
                                                                            DatabaseHelper.ConditionDefinitions.ConditionResisting,
                                                                            affinity_charmed,
                                                                            affinity_frightened
                                                                            );

            var effect = new EffectDescription();
            effect.Copy(DatabaseHelper.SpellDefinitions.Resistance.EffectDescription);
            effect.SetRangeType(RuleDefinitions.RangeType.Self);
            effect.SetRangeParameter(0);
            effect.SetTargetProximityDistance(30);
            effect.DurationParameter = 1;
            effect.DurationType = RuleDefinitions.DurationType.Round;
            effect.SetEndOfEffect(RuleDefinitions.TurnOccurenceType.EndOfTurn);
            effect.EffectForms.Clear();
            effect.SetTargetType(RuleDefinitions.TargetType.Sphere);

            var effect_form = new EffectForm();
            effect_form.ConditionForm = new ConditionForm();
            effect_form.FormType = EffectForm.EffectFormType.Condition;
            effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            effect_form.ConditionForm.ConditionDefinition = effect_condition;
            effect.EffectForms.Add(effect_form);
            effect.SetRecurrentEffect(RuleDefinitions.RecurrentEffect.OnActivation | RuleDefinitions.RecurrentEffect.OnEnter | RuleDefinitions.RecurrentEffect.OnTurnStart);
            countercharm = Helpers.PowerBuilder.createPower("BardCountercharmPower",
                                                            "",
                                                            countercharm_title_string,
                                                            countercharm_description_string,
                                                            DatabaseHelper.SpellDefinitions.Resistance.GuiPresentation.SpriteReference,
                                                            DatabaseHelper.FeatureDefinitionPowers.PowerPaladinAuraOfProtection,
                                                            effect,
                                                            RuleDefinitions.ActivationTime.Action,
                                                            0,
                                                            RuleDefinitions.UsesDetermination.Fixed,
                                                            RuleDefinitions.RechargeRate.AtWill,
                                                            Helpers.Stats.Charisma,
                                                            Helpers.Stats.Charisma
                                                            );
        }


        static void createMagicalSecrets()
        {
            var spelllist = Helpers.SpelllistBuilder.createCombinedSpellList("BardClassMagicalSecretSpelllist", "", "",
                                                                             bard_spelllist,
                                                                             DatabaseHelper.SpellListDefinitions.SpellListWizard,
                                                                             DatabaseHelper.SpellListDefinitions.SpellListCleric,
                                                                             DatabaseHelper.SpellListDefinitions.SpellListPaladin,
                                                                             DatabaseHelper.SpellListDefinitions.SpellListRanger
                                                                             );
            spelllist.SpellsByLevel[0].Spells = bard_spelllist.SpellsByLevel[0].Spells; //do not affect cantrips for the time being

            magical_secrets = Helpers.ExtraSpellSelectionBuilder.createExtraSpellSelection("BardClassMagicalSecrets",
                                                                                            "",
                                                                                            "Feature/&BardClassMagicalSecretsTitle",
                                                                                            "Feature/&BardClassMagicalSecretsDescription",
                                                                                            bard_class,
                                                                                            10,
                                                                                            2,
                                                                                            spelllist
                                                                                            );
        }


        static void createSongOfRest()
        {
            string song_of_rest_title_string = "Feature/&BardClassSongOfRestTitle";
            string song_of_rest_description_string = "Feature/&BardClassSongOfRestDescription";

            var dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8};

            for (int i = 0; i < dice.Length; i++)
            {
                var feature = Helpers.FeatureBuilder<NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest>.createFeature("BardClassSongOfRestFeature" + dice[i].ToString(),
                                                                                                                 "",
                                                                                                                 song_of_rest_title_string + (i + 1).ToString(),
                                                                                                                 song_of_rest_description_string,
                                                                                                                 null);
                feature.ApplyToParty = true;
                feature.tag = "SongOfRest";
                feature.DieType = dice[i];
                song_of_rest[dice[i]] = feature;
            }
        }


        static void createInspiration()
        {
            string inspiration_title_string = "Feature/&BardClassInspirationPowerTitle";
            string inspiration_description_string = "Feature/&BardClassInspirationPowerDescription";

            FeatureDefinitionPower previous_power = null;
            var dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8, RuleDefinitions.DieType.D10 };
            for (int i = 0; i < dice.Length; i++)
            {
                var inspiration_saves = Helpers.SavingThrowAffinityBuilder.createSavingthrowAffinity("BardClassInspirationSavingthrowBonus" + dice[i].ToString(),
                                                                                                     "",
                                                                                                     "",
                                                                                                     "",
                                                                                                     null,
                                                                                                     RuleDefinitions.CharacterSavingThrowAffinity.None,
                                                                                                     1,
                                                                                                     dice[i],
                                                                                                     Helpers.Stats.getAllStats().ToArray()
                                                                                                     );

                var inspiration_skills = Helpers.AbilityCheckAffinityBuilder.createAbilityCheckAffinity("BardClassInspirationSkillsBonus" + dice[i].ToString(),
                                                                                                         "",
                                                                                                         "",
                                                                                                         "",
                                                                                                         null,
                                                                                                         RuleDefinitions.CharacterAbilityCheckAffinity.None,
                                                                                                         1,
                                                                                                         dice[i],
                                                                                                         Helpers.Stats.getAllStats().ToArray()
                                                                                                         );

                var inspiration_attack = Helpers.AttackBonusBuilder.createAttackBonus("BardClassInspirationAttackBonus" + dice[i].ToString(),
                                                                                                         "",
                                                                                                         "",
                                                                                                         "",
                                                                                                         null,
                                                                                                         1,
                                                                                                         dice[i]
                                                                                                         );
                var inspiration_condition = Helpers.ConditionBuilder.createConditionWithInterruptions("BardClassInspirationCondition" + dice[i].ToString(),
                                                                                                      "",
                                                                                                      inspiration_title_string,
                                                                                                      inspiration_description_string,
                                                                                                      null,
                                                                                                      DatabaseHelper.ConditionDefinitions.ConditionGuided,
                                                                                                      new RuleDefinitions.ConditionInterruption[] {RuleDefinitions.ConditionInterruption.AbilityCheck,
                                                                                                                                               RuleDefinitions.ConditionInterruption.Attacks,
                                                                                                                                               RuleDefinitions.ConditionInterruption.SavingThrow },
                                                                                                      inspiration_saves,
                                                                                                      inspiration_skills,
                                                                                                      inspiration_attack
                                                                                                      );

                var effect = new EffectDescription();
                effect.Copy(DatabaseHelper.SpellDefinitions.Guidance.EffectDescription);
                effect.SetRangeType(RuleDefinitions.RangeType.Distance);
                effect.SetRangeParameter(60);
                effect.DurationParameter = 10;
                effect.DurationType = RuleDefinitions.DurationType.Minute;
                effect.EffectForms.Clear();
                effect.SetTargetFilteringTag((RuleDefinitions.TargetFilteringTag)ExtraTargetFilteringTag.NonCaster);

                var effect_form = new EffectForm();
                effect_form.ConditionForm = new ConditionForm();
                effect_form.FormType = EffectForm.EffectFormType.Condition;
                effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
                effect_form.ConditionForm.ConditionDefinition = inspiration_condition;
                effect.EffectForms.Add(effect_form);

                var inspiration_power = Helpers.PowerBuilder.createPower("BardInspirationPower" + dice[i].ToString(),
                                                                     "",
                                                                     inspiration_title_string + (i + 1).ToString(),
                                                                     inspiration_description_string,
                                                                     DatabaseHelper.SpellDefinitions.Guidance.GuiPresentation.SpriteReference,
                                                                     DatabaseHelper.FeatureDefinitionPowers.PowerPaladinLayOnHands,
                                                                     effect,
                                                                     RuleDefinitions.ActivationTime.BonusAction,
                                                                     0,
                                                                     RuleDefinitions.UsesDetermination.AbilityBonusPlusFixed,
                                                                     previous_power == null ? RuleDefinitions.RechargeRate.LongRest : RuleDefinitions.RechargeRate.ShortRest,
                                                                     Helpers.Stats.Charisma,
                                                                     Helpers.Stats.Charisma
                                                                     );
                inspiration_power.SetShortTitleOverride(inspiration_title_string);

                if (previous_power != null)
                {
                    inspiration_power.SetOverriddenPower(previous_power);
                }
                previous_power = inspiration_power;
                inspiration_powers.Add(dice[i], inspiration_power);
            }

            string font_of_inspiration_title_string = "Feature/&BardClassFontOfInspirationFeatureTitle";
            string font_of_inspiration_description_string = "Feature/&BardClassFontOfInspirationFeatureDescription";
            font_of_inspiration = Helpers.OnlyDescriptionFeatureBuilder.createOnlyDescriptionFeature("BardClassFontOfInspirationFeature",
                                                                                                     "",
                                                                                                     font_of_inspiration_title_string,
                                                                                                     font_of_inspiration_description_string);
        }


        public static void BuildAndAddClassToDB()
        {
            var BardClass = new BardClassBuilder(BardClassName, BardClassNameGuid).AddToDB();
            //Might need to add subclasses after the class is in the DB?
            CharacterSubclassDefinition characterSubclassDefinition = BardSubClassPathOfTheBear.Build();
            BardFeatureDefinitionSubclassChoice.Subclasses.Add(characterSubclassDefinition.Name);
            CharacterSubclassDefinition characterSubclassDefinitionFrenzy = BardSubclassPathOfFrenzy.Build();
            BardFeatureDefinitionSubclassChoice.Subclasses.Add(characterSubclassDefinitionFrenzy.Name);
            CharacterSubclassDefinition characterSubclassDefinitionReaver = BardSubclassPathOfTheReaver.Build();
            BardFeatureDefinitionSubclassChoice.Subclasses.Add(characterSubclassDefinitionReaver.Name);
        }

        private static FeatureDefinitionSubclassChoice BardFeatureDefinitionSubclassChoice;
    }

    public static class BardSubClassPathOfTheBear
    {
        const string BardSubClassPathOfTheBearName = "BardSubclassPathOfTheBear";
        const string BardSubClassPathOfTheBearNameGuid = "88d8752b-4956-4daf-91fc-84e6196c3985";

        public static CharacterSubclassDefinition Build()
        {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                    "Subclass/&BardSubclassPathOfTheBearDescription",
                    "Subclass/&BardSubclassPathOfTheBearTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.MartialChampion.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder(BardSubClassPathOfTheBearName, BardSubClassPathOfTheBearNameGuid)
                    .SetGuiPresentation(subclassGuiPresentation)
                    .AddFeatureAtLevel(BardClassPathOfBearRageClassPowerBuilder.RageClassPower, 3) // Special rage and increase rage count to 3
                    //.AddFeatureAtLevel(BardClassPathOfBearRageClassPowerBuilder.RageClassPower, 3) // TODO something more?
                    .AddFeatureAtLevel(BardClassPathOfBearRageClassPowerLevel6Builder.RageClassPower, 6) //Up rage count to 4 - Do in subclass since BearRage has its own characteristics
                    .AddFeatureAtLevel(DatabaseHelper.FeatureDefinitionEquipmentAffinitys.EquipmentAffinityBullsStrength, 6) //Double carry cap
                    .AddFeatureAtLevel(BardClassPathOfBearRageClassPowerLevel9Builder.RageClassPower, 9) //Up damage on rage - Do in subclass since BearRage has its own characteristics
                    .AddFeatureAtLevel(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityDwarvenPlateResistShove, 10) //TODO Extra feature totem has commune with nature but not sure what to add here.
                    .AddToDB();

            return definition;
        }
    }


    internal class BardClassDangerSenseDexteritySavingThrowAffinityBuilder : BaseDefinitionBuilder<FeatureDefinitionSavingThrowAffinity>
    {
        const string BardClassDangerSenseDexteritySavingThrowAffinityName = "BardClassDangerSenseDexteritySavingThrowAffinity";
        const string BardClassDangerSenseDexteritySavingThrowAffinityNameGuid = "93269849-0d10-47d5-858a-aaaf047d801c";

        protected BardClassDangerSenseDexteritySavingThrowAffinityBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionSavingThrowAffinitys.SavingThrowAffinityCreedOfArun, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassDangerSenseDexteritySavingThrowAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassDangerSenseDexteritySavingThrowAffinityDescription";

            //Just always gives Dex save ADV since making it work contextually with the effect originating within 30ft would require too much work.
            //The condition restrictions might be easier to implement, but I won't bother for now at least.
            Definition.AffinityGroups.Clear();
            var dexSaveAffinityGroup = new SavingThrowAffinityGroup();
            dexSaveAffinityGroup.affinity = RuleDefinitions.CharacterSavingThrowAffinity.Advantage;
            dexSaveAffinityGroup.abilityScoreName = "Dexterity";
            dexSaveAffinityGroup.savingThrowContext = RuleDefinitions.SavingThrowContext.None;
            dexSaveAffinityGroup.savingThrowModifierType = ModifierType.AddDice;
            Definition.AffinityGroups.Add(dexSaveAffinityGroup);
        }

        public static FeatureDefinitionSavingThrowAffinity CreateAndAddToDB(string name, string guid)
            => new BardClassDangerSenseDexteritySavingThrowAffinityBuilder(name, guid).AddToDB();

        public static FeatureDefinitionSavingThrowAffinity BardClassDangerSenseDexteritySavingThrowAffinity = CreateAndAddToDB(BardClassDangerSenseDexteritySavingThrowAffinityName, BardClassDangerSenseDexteritySavingThrowAffinityNameGuid);
    }

    internal class BardClassUnarmoredDefenseBuilder : BaseDefinitionBuilder<FeatureDefinitionFightingStyleChoice>
    {
        const string BardClassUnarmoredDefenseName = "BardClassUnarmoredDefenseAttributeModifier";
        const string BardClassUnarmoredDefenseNameGuid = "801c6454-e904-4924-8344-0c6b0ececf20";

        protected BardClassUnarmoredDefenseBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionFightingStyleChoices.FightingStyleFighter, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassUnarmoredDefenseTitle";
            Definition.GuiPresentation.Description = SolastaUnarmoredDefense.FightingStyles.UnarmoredDefense_Constitution.GuiPresentation.Description;

            Definition.FightingStyles.Clear();
            Definition.FightingStyles.Add(SolastaUnarmoredDefense.FightingStyles.UnarmoredDefense_Constitution.Name);
        }

        public static FeatureDefinitionFightingStyleChoice CreateAndAddToDB(string name, string guid)
            => new BardClassUnarmoredDefenseBuilder(name, guid).AddToDB();

        public static FeatureDefinitionFightingStyleChoice BardClassUnarmoredDefense = CreateAndAddToDB(BardClassUnarmoredDefenseName, BardClassUnarmoredDefenseNameGuid);
    }



    internal class BardClassFastMovementMovementAffinityBuilder : BaseDefinitionBuilder<FeatureDefinitionMovementAffinity>
    {
        const string FastMovementMovementAffinityName = "BardClassFastMovementMovementAffinity";
        const string FastMovementMovementAffinityNameGuid = "9d39501c-8e77-49d9-9991-b8710586381f";

        protected BardClassFastMovementMovementAffinityBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionMovementAffinitys.MovementAffinityLongstrider, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassFastMovementMovementAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassFastMovementMovementAffinityDescription";
        }

        public static FeatureDefinitionMovementAffinity CreateAndAddToDB(string name, string guid)
            => new BardClassFastMovementMovementAffinityBuilder(name, guid).AddToDB();

        public static FeatureDefinitionMovementAffinity FastMovementMovementAffinity
            = CreateAndAddToDB(FastMovementMovementAffinityName, FastMovementMovementAffinityNameGuid);
    }

    /// <summary>
    /// 'Blank' power for the level up description
    /// </summary>
    internal class BardClassFastMovementMovementPowerForLevelUpDescriptionBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string FastMovementMovementAffinityName = "BardClassFastMovementMovementPowerForLevelUpDescription";
        const string FastMovementMovementAffinityNameGuid = "84f70554-c274-483d-9d4f-10a6ed4da905";

        protected BardClassFastMovementMovementPowerForLevelUpDescriptionBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerDomainElementalFireBurst, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassFastMovementMovementAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassFastMovementMovementAffinityDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.None); 
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.Permanent);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(0);
            Definition.SetShortTitleOverride("Feature/&BardClassFastMovementMovementAffinityTitle");
            Definition.SetEffectDescription(new EffectDescription());
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassFastMovementMovementPowerForLevelUpDescriptionBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower FastMovementMovementPowerForLevelUpDescription
            = CreateAndAddToDB(FastMovementMovementAffinityName, FastMovementMovementAffinityNameGuid);
    }


    /// <summary>
    /// 'Blank' power for the level up description
    /// </summary>
    internal class BardClassInitiativeAdvantagePowerForLevelUpDescriptionBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string FastMovementMovementAffinityName = "BardClassInitiativeAdvantagePowerForLevelUpDescription";
        const string FastMovementMovementAffinityNameGuid = "2a9230f7-e6a9-49fd-b722-b3ba28495dc7";

        protected BardClassInitiativeAdvantagePowerForLevelUpDescriptionBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerDomainElementalFireBurst, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassInitiativeAdvantageTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassInitiativeAdvantageDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.None);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.Permanent);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(0);
            Definition.SetShortTitleOverride("Feature/&BardClassInitiativeAdvantageTitle");
            Definition.SetEffectDescription(new EffectDescription());
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassInitiativeAdvantagePowerForLevelUpDescriptionBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower InitiativeAdvantagePowerForLevelUpDescription
            = CreateAndAddToDB(FastMovementMovementAffinityName, FastMovementMovementAffinityNameGuid);
    }

    internal class BardClassRageClassPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string RageClassPowerName = "BardClassRageClassPower";
        const string RageClassPowerNameGuid = "2179ad09-27ca-4d7c-b20e-a1705ef5978b";

        protected BardClassRageClassPowerBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerDomainElementalFireBurst, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassPowerDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(2);
            Definition.SetShortTitleOverride("Feature/&BardClassRageClassPowerTitle");

            //Create the power attack effect
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardClassRageClassConditionBuilder.RageClassCondition;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassRageClassPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(RageClassPowerName, RageClassPowerNameGuid);
    }

    internal class BardClassPathOfBearRageClassPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string RageClassPowerName = "BardClassPathOfBearRageClassPower";
        const string RageClassPowerNameGuid = "fe1812f0-1ba5-45d2-bc15-70ea99f0dd6d";

        protected BardClassPathOfBearRageClassPowerBuilder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfBearRageClassPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfBearRageClassPowerDescription";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassRageClassPowerBuilder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(3); //3 uses at level 3 when this is introduced
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfBearRageClassPowerTitle");

            //Create the power attack effect
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheBearRageClassConditionBuilder.PathOfBearRageCondition;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfBearRageClassPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(RageClassPowerName, RageClassPowerNameGuid);
    }

    internal class BardClassPathOfBearRageClassPowerLevel6Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string RageClassPowerName = "BardClassPathOfBearRageClassPowerLevel6";
        const string RageClassPowerNameGuid = "df1d9bed-5d00-4f26-88fa-7b3850b327b6";

        protected BardClassPathOfBearRageClassPowerLevel6Builder(string name, string guid) : base(BardClassPathOfBearRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfBearRageClassPowerLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfBearRageClassPowerLevel6Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfBearRageClassPowerBuilder.RageClassPower);
            Definition.SetFixedUsesPerRecharge(4); //Just increase the use count
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfBearRageClassPowerLevel6Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(RageClassPowerName, RageClassPowerNameGuid);
    }

    internal class BardClassPathOfBearRageClassPowerLevel9Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string RageClassPowerName = "BardClassPathOfBearRageClassPowerLevel9";
        const string RageClassPowerNameGuid = "9657531a-122e-4fb9-acf4-aac32c686a39";

        protected BardClassPathOfBearRageClassPowerLevel9Builder(string name, string guid) : base(BardClassPathOfBearRageClassPowerLevel6Builder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfBearRageClassPowerLevel9Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfBearRageClassPowerLevel9Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfBearRageClassPowerLevel6Builder.RageClassPower);
            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4); //4 uses at level 9
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfBearRageClassPowerTitle");

            //Create the power rage condition - this time Bear With level 9 extra damage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheBearRageClassConditionLevel9Builder.RageClassCondition;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfBearRageClassPowerLevel9Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(RageClassPowerName, RageClassPowerNameGuid);
    }


    internal class BardClassRageClassConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string RageClassConditionName = "BardClassRageClassCondition";
        const string RageClassConditionNameGuid = "f5a14f78-4755-4eac-ab6a-a1b11acf7d5f";

        protected BardClassRageClassConditionBuilder(string name, string guid) : base(DatabaseHelper.ConditionDefinitions.ConditionHeraldOfBattle, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassConditionDescription";

            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierBuilder.RageClassDamageBonusAttackModifier);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardClassRageClassConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition RageClassCondition
            = CreateAndAddToDB(RageClassConditionName, RageClassConditionNameGuid);
    }

    internal class BardPathOfTheBearRageClassConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string RageClassConditionName = "BardPathOfTheBearRageClassCondition";
        const string RageClassConditionNameGuid = "b6f1f1d4-5ca7-4e25-aba7-e55b6f44bc3f";

        protected BardPathOfTheBearRageClassConditionBuilder(string name, string guid) : base(DatabaseHelper.ConditionDefinitions.ConditionHeraldOfBattle, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassBearConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassBearConditionDescription";

            //Resitance to all damage so as not to straight copy Totem bear :)
            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityAcidResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityColdResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityFireResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityForceDamageResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityLightningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPoisonResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPsychicResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityRadiantResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityThunderResistance);

            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierBuilder.RageClassDamageBonusAttackModifier);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheBearRageClassConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition PathOfBearRageCondition
            = CreateAndAddToDB(RageClassConditionName, RageClassConditionNameGuid);
    }

    /// <summary>
    /// Up the rage damage at level 9 - otherwise this is the same.
    /// Maybe there's a better way to do this
    /// </summary>
    internal class BardPathOfTheBearRageClassConditionLevel9Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string RageClassConditionName = "BardPathOfTheBearRageClassConditionLevel9";
        const string RageClassConditionNameGuid = "0280af54-886a-4ead-9a05-b01760cd1533";

        protected BardPathOfTheBearRageClassConditionLevel9Builder(string name, string guid) : base(DatabaseHelper.ConditionDefinitions.ConditionHeraldOfBattle, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassBearConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassBearConditionDescription";

            //Resitance to all damage so as not to straight copy Totem bear :)
            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityAcidResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityColdResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityFireResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityForceDamageResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityLightningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPoisonResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPsychicResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityRadiantResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityThunderResistance);

            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierLevel9Builder.RageClassDamageBonusAttackLevel9Modifier);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheBearRageClassConditionLevel9Builder(name, guid).AddToDB();

        public static ConditionDefinition RageClassCondition
            = CreateAndAddToDB(RageClassConditionName, RageClassConditionNameGuid);
    }

    internal class BardClassRageClassStrengthSavingThrowAffinityBuilder : BaseDefinitionBuilder<FeatureDefinitionSavingThrowAffinity>
    {
        const string RageClassStrengthSavingThrowAffinityName = "BardClassRageClassStrengthSavingThrowAffinity";
        const string RageClassStrengthSavingThrowAffinityNameGuid = "0280af54-886a-4ead-9a05-b01760cd1533";

        protected BardClassRageClassStrengthSavingThrowAffinityBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionSavingThrowAffinitys.SavingThrowAffinityCreedOfArun, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassStrengthSavingThrowAffinityTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassStrengthSavingThrowAffinityDescription";

            Definition.AffinityGroups.Clear();
            var strengthSaveAffinityGroup = new SavingThrowAffinityGroup();
            strengthSaveAffinityGroup.affinity = RuleDefinitions.CharacterSavingThrowAffinity.Advantage;
            strengthSaveAffinityGroup.abilityScoreName = "Strength";
            Definition.AffinityGroups.Add(strengthSaveAffinityGroup);
        }

        public static FeatureDefinitionSavingThrowAffinity CreateAndAddToDB(string name, string guid)
            => new BardClassRageClassStrengthSavingThrowAffinityBuilder(name, guid).AddToDB();

        public static FeatureDefinitionSavingThrowAffinity RageClassStrengthSavingThrowAffinity
            = CreateAndAddToDB(RageClassStrengthSavingThrowAffinityName, RageClassStrengthSavingThrowAffinityNameGuid);
    }

    internal class BardClassRageClassDamageBonusAttackModifierBuilder : BaseDefinitionBuilder<FeatureDefinitionAttackModifier>
    {
        const string RageClassDamageBonusAttackModifierName = "BardClassRageClassDamageBonusAttackModifier";
        const string RageClassDamageBonusAttackModifierNameGuid = "9398a1da-9511-423f-984d-5680ac6c64c9";

        protected BardClassRageClassDamageBonusAttackModifierBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAttackModifiers.AttackModifierFightingStyleArchery, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassDamageBonusAttackModifierTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassDamageBonusAttackModifierDescription";

            Definition.SetAttackRollModifier(0);
            Definition.SetDamageRollModifier(2);//Could find a way to up this at level 9 to match barb but that seems like a lot of work right now :)
        }

        public static FeatureDefinitionAttackModifier CreateAndAddToDB(string name, string guid)
            => new BardClassRageClassDamageBonusAttackModifierBuilder(name, guid).AddToDB();

        public static FeatureDefinitionAttackModifier RageClassDamageBonusAttackModifier
            = CreateAndAddToDB(RageClassDamageBonusAttackModifierName, RageClassDamageBonusAttackModifierNameGuid);
    }

    internal class BardClassRageClassDamageBonusAttackModifierLevel9Builder : BaseDefinitionBuilder<FeatureDefinitionAttackModifier>
    {
        const string RageClassDamageBonusAttackModifierName = "BardClassRageClassDamageBonusAttackModifierLevel9";
        const string RageClassDamageBonusAttackModifierNameGuid = "34987bc9-0fa2-454e-a24b-ad4a44a50c0b";

        protected BardClassRageClassDamageBonusAttackModifierLevel9Builder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAttackModifiers.AttackModifierFightingStyleArchery, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassRageClassDamageBonusAttackModifierTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassRageClassDamageBonusAttackModifierDescription";

            Definition.SetAttackRollModifier(0);
            Definition.SetDamageRollModifier(3);
        }

        public static FeatureDefinitionAttackModifier CreateAndAddToDB(string name, string guid)
            => new BardClassRageClassDamageBonusAttackModifierLevel9Builder(name, guid).AddToDB();

        public static FeatureDefinitionAttackModifier RageClassDamageBonusAttackLevel9Modifier
            = CreateAndAddToDB(RageClassDamageBonusAttackModifierName, RageClassDamageBonusAttackModifierNameGuid);
    }
}

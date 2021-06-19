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

        static public RuleDefinitions.DieType[] inspiration_dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8, RuleDefinitions.DieType.D10, RuleDefinitions.DieType.D12 };
        static public CharacterClassDefinition bard_class;
        static public Dictionary<RuleDefinitions.DieType, FeatureDefinitionPower> inspiration_powers = new Dictionary<RuleDefinitions.DieType, FeatureDefinitionPower>();
        static public FeatureDefinition font_of_inspiration;
        static public FeatureDefinitionPointPool expertise;
        static public FeatureDefinitionAbilityCheckAffinity jack_of_all_trades;
        static public Dictionary<RuleDefinitions.DieType, NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest> song_of_rest = new Dictionary<RuleDefinitions.DieType, NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest>();
        static public SpellListDefinition bard_spelllist;
        static public SpellListDefinition magical_secrets_spelllist;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection magical_secrets;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection magical_secrets14;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection magical_secrets18;
        static public FeatureDefinitionPower countercharm;

        static public Dictionary<RuleDefinitions.DieType, FeatureDefinitionFeatureSet> cutting_words = new Dictionary<RuleDefinitions.DieType, FeatureDefinitionFeatureSet>();

        static public FeatureDefinitionPointPool lore_college_bonus_proficiencies;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection additional_magical_secrets;
        //TODO
        //colleges: virtue, wyrdsingers ?, ..


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
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets14, 14));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(inspiration_powers[RuleDefinitions.DieType.D12], 15));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets18, 18));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 19));

            var subclassChoicesGuiPresentation = new GuiPresentation();
            subclassChoicesGuiPresentation.Title = "Subclass/&BardSubclassCollegeTitle";
            subclassChoicesGuiPresentation.Description = "Subclass/&BardSubclassCollegeDescription";
            BardFeatureDefinitionSubclassChoice = this.BuildSubclassChoice(3, "College", false, "SubclassChoiceBardSpecialistArchetypes", subclassChoicesGuiPresentation, BardClassSubclassesGuid);
        }


        static CharacterSubclassDefinition createLoreCollege()
        {
            createCuttingWords();
            createLoreCollegeBonusProficiencies();
            createLoreCollegeMagicalSecrets();

            var gui_presentation = new GuiPresentationBuilder(
                    "Subclass/&BardSubclassCollegeOfLoreDescription",
                    "Subclass/&BardSubclassCollegeOfLoreTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.TraditionLoremaster.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder("BardSubclassCollegeOfLore", "3bcb4d6b-0a7a-470f-b172-002092b8f464")
                    .SetGuiPresentation(gui_presentation)
                    .AddFeatureAtLevel(lore_college_bonus_proficiencies, 3)
                    .AddFeatureAtLevel(cutting_words[RuleDefinitions.DieType.D6], 3)
                    .AddFeatureAtLevel(cutting_words[RuleDefinitions.DieType.D8], 5)
                    .AddFeatureAtLevel(additional_magical_secrets, 6)
                    .AddFeatureAtLevel(cutting_words[RuleDefinitions.DieType.D10], 10)
                    .AddFeatureAtLevel(cutting_words[RuleDefinitions.DieType.D12], 15)
                    .AddToDB();

            return definition;
        }


        static void createLoreCollegeBonusProficiencies()
        {
            lore_college_bonus_proficiencies = Helpers.PoolBuilder.createSkillProficiency("BardLoreSubclassSkillProficiency",
                                                        "",
                                                        "Feature/&BardLoreSublclassExtraSkillPointPoolTitle",
                                                        "Feature/&BardLoreSublclassExtraSkillPointPoolDescription",
                                                        3,
                                                        Helpers.Skills.getAllSkills());
        }


        static void createLoreCollegeMagicalSecrets()
        {
            additional_magical_secrets = Helpers.ExtraSpellSelectionBuilder.createExtraSpellSelection("BardLoreSubclassAdditionalMagicalSecrets",
                                                                                                        "",
                                                                                                        "Feature/&BardLoreSubclassAdditionalMagicalSecretsTitle",
                                                                                                        "Feature/&BardLoreSubclassAdditionalMagicalSecretsDescription",
                                                                                                        bard_class,
                                                                                                        6,
                                                                                                        2,
                                                                                                        magical_secrets_spelllist
                                                                                                        );
        }


        static void createCuttingWords()
        {
            //TODO: add enemies immune to charmed be unaffected
            //TODO: check distance to the attacker
            string cutting_words_title_string = "Feature/&BardClassCuttingWordsTitle";
            string cutting_words_description_string = "Feature/&BardClassCuttingWordsDescription";
            string cutting_words_attack_roll_title_string = "Feature/&BardClassCuttingWordsPowerAttackRollTitle";
            string cutting_words_damage_roll_title_string = "Feature/&BardClassCuttingWordsPowerDamageRollTitle";

            string use_cutting_words_attack_roll_react_description = "Reaction/&UseBardClassCuttingWordsAttackRollsPenaltyPowerReactDescription";
            string use_cutting_words_attack_roll_react_title = "Reaction/&CommonUsePowerReactTitle";
            string use_cutting_words_attack_roll_description = use_cutting_words_attack_roll_react_description;
            string use_cutting_words_attack_roll_title = cutting_words_attack_roll_title_string;

            string use_cutting_words_damage_roll_react_description = "Reaction/&UseBardClassCuttingWordsDamageRollsPenaltyPowerReactDescription";
            string use_cutting_words_damage_roll_react_title = "Reaction/&CommonUsePowerReactTitle";
            string use_cutting_words_damage_roll_description = use_cutting_words_damage_roll_react_description;
            string use_cutting_words_damage_roll_title = cutting_words_damage_roll_title_string;

            NewFeatureDefinitions.FeatureDefinitionReactionPowerOnAttackAttempt previous_attack_roll_penalty_power = null;
            NewFeatureDefinitions.FeatureDefinitionReactionPowerOnDamage previous_damage_roll_penalty_power = null;
            var dice = inspiration_dice;

            for (int i = 0; i < dice.Length; i++)
            {
                var penalty_attack = Helpers.AttackBonusBuilder.createAttackBonus("BardClassCuttingWordsAttackPenalty" + dice[i].ToString(),
                                                                                    "",
                                                                                    "",
                                                                                    "",
                                                                                    null,
                                                                                    1,
                                                                                    dice[i],
                                                                                    substract: true
                                                                                    );

                var attack_penalty_condition = Helpers.ConditionBuilder.createConditionWithInterruptions("BardClassCuttingWordsAttackPenaltyCondition" + dice[i].ToString(),
                                                                                                          "",
                                                                                                          Helpers.StringProcessing.concatenateStrings(Common.common_condition_prefix,
                                                                                                                                                      cutting_words_attack_roll_title_string,
                                                                                                                                                      "Rules/&BardClassCuttingWordsAttackPenaltyCondition" + dice[i].ToString()
                                                                                                                                                      ),
                                                                                                          cutting_words_description_string,
                                                                                                          null,
                                                                                                          DatabaseHelper.ConditionDefinitions.ConditionDazzled,
                                                                                                          new RuleDefinitions.ConditionInterruption[] { RuleDefinitions.ConditionInterruption.Attacks },
                                                                                                          penalty_attack
                                                                                                          );

                var effect = new EffectDescription();
                effect.Copy(DatabaseHelper.SpellDefinitions.Dazzle.EffectDescription);
                effect.SetRangeType(RuleDefinitions.RangeType.Distance);
                effect.SetRangeParameter(60);
                effect.DurationParameter = 1;
                effect.SetTargetSide(RuleDefinitions.Side.Enemy);
                effect.DurationType = RuleDefinitions.DurationType.Round;
                effect.EffectForms.Clear();

                var effect_form = new EffectForm();
                effect_form.ConditionForm = new ConditionForm();
                effect_form.FormType = EffectForm.EffectFormType.Condition;
                effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
                effect_form.ConditionForm.ConditionDefinition = attack_penalty_condition;
                effect.EffectForms.Add(effect_form);

                var attack_penalty_power = Helpers.GenericPowerBuilder<NewFeatureDefinitions.FeatureDefinitionReactionPowerOnAttackAttempt>
                                                    .createPower("BardClassCuttingWordsAttackRollsPenaltyPower" + dice[i].ToString(),
                                                                    "",
                                                                    cutting_words_attack_roll_title_string,
                                                                    cutting_words_description_string,
                                                                    DatabaseHelper.SpellDefinitions.Dazzle.GuiPresentation.SpriteReference,
                                                                    effect,
                                                                    RuleDefinitions.ActivationTime.Reaction,
                                                                    0,
                                                                    RuleDefinitions.UsesDetermination.AbilityBonusPlusFixed,
                                                                    previous_attack_roll_penalty_power == null ? RuleDefinitions.RechargeRate.LongRest : RuleDefinitions.RechargeRate.ShortRest,
                                                                    Helpers.Stats.Charisma,
                                                                    Helpers.Stats.Charisma
                                                                    );
                attack_penalty_power.linkedPower = inspiration_powers[dice[i]];
                attack_penalty_power.worksOnMelee = true;
                attack_penalty_power.worksOnRanged = true;
                attack_penalty_power.SetShortTitleOverride(cutting_words_attack_roll_title_string);
                if (previous_attack_roll_penalty_power != null)
                {
                    attack_penalty_power.SetOverriddenPower(previous_attack_roll_penalty_power);
                }
                previous_attack_roll_penalty_power = attack_penalty_power;
                Helpers.StringProcessing.addPowerReactStrings(attack_penalty_power, use_cutting_words_attack_roll_title, use_cutting_words_attack_roll_description,
                                                                                    use_cutting_words_attack_roll_react_title, use_cutting_words_attack_roll_react_description);


                var penalty_damage = Helpers.FeatureBuilder<NewFeatureDefinitions.ModifyDiceRollValue>.createFeature("BardClassCuttingWordsDamagePenalty" + dice[i].ToString(),
                                                                                                                     "",
                                                                                                                     "",
                                                                                                                     "",
                                                                                                                     null,
                                                                                                                     m =>
                                                                                                                     {
                                                                                                                         m.diceType = dice[i];
                                                                                                                         m.numDice = -1;
                                                                                                                         m.contexts = new List<RuleDefinitions.RollContext>() { RuleDefinitions.RollContext.DamageValueRoll };
                                                                                                                     }
                                                                                                                     );

                var damage_penalty_condition = Helpers.ConditionBuilder.createConditionWithInterruptions("BardClassCuttingWordsDamagePenaltyCondition" + dice[i].ToString(),
                                                                                                          "",
                                                                                                          Helpers.StringProcessing.concatenateStrings(Common.common_condition_prefix,
                                                                                                                                                      cutting_words_damage_roll_title_string,
                                                                                                                                                      "Rules/&BardClassCuttingWordsDamagePenaltyCondition" + dice[i].ToString()
                                                                                                                                                      ),
                                                                                                          cutting_words_description_string,
                                                                                                          null,
                                                                                                          DatabaseHelper.ConditionDefinitions.ConditionDazzled,
                                                                                                          new RuleDefinitions.ConditionInterruption[] { (RuleDefinitions.ConditionInterruption)ExtraConditionInterruption.RollsForDamage },
                                                                                                          penalty_damage
                                                                                                          );

                effect = new EffectDescription();
                effect.Copy(DatabaseHelper.SpellDefinitions.Dazzle.EffectDescription);
                effect.SetRangeType(RuleDefinitions.RangeType.Distance);
                effect.SetRangeParameter(60);
                effect.DurationParameter = 1;
                effect.SetTargetSide(RuleDefinitions.Side.Enemy);
                effect.DurationType = RuleDefinitions.DurationType.Round;
                effect.EffectForms.Clear();

                effect_form = new EffectForm();
                effect_form.ConditionForm = new ConditionForm();
                effect_form.FormType = EffectForm.EffectFormType.Condition;
                effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
                effect_form.ConditionForm.ConditionDefinition = damage_penalty_condition;
                effect.EffectForms.Add(effect_form);

                var damage_penalty_power = Helpers.GenericPowerBuilder<NewFeatureDefinitions.FeatureDefinitionReactionPowerOnDamage>
                                                    .createPower("BardClassCuttingWordsDamageRollsPenaltyPower" + dice[i].ToString(),
                                                                    "",
                                                                    cutting_words_damage_roll_title_string,
                                                                    cutting_words_description_string,
                                                                    DatabaseHelper.SpellDefinitions.Dazzle.GuiPresentation.SpriteReference,
                                                                    effect,
                                                                    RuleDefinitions.ActivationTime.Reaction,
                                                                    0,
                                                                    RuleDefinitions.UsesDetermination.AbilityBonusPlusFixed,
                                                                    previous_damage_roll_penalty_power == null ? RuleDefinitions.RechargeRate.LongRest : RuleDefinitions.RechargeRate.ShortRest,
                                                                    Helpers.Stats.Charisma,
                                                                    Helpers.Stats.Charisma
                                                                    );
                Helpers.StringProcessing.addPowerReactStrings(damage_penalty_power, use_cutting_words_damage_roll_title, use_cutting_words_damage_roll_description,
                                                                    use_cutting_words_damage_roll_react_title, use_cutting_words_damage_roll_react_description);
                damage_penalty_power.linkedPower = inspiration_powers[dice[i]];

                damage_penalty_power.worksOnMelee = true;
                damage_penalty_power.worksOnRanged = true;
                damage_penalty_power.worksOnMagic = true;
                damage_penalty_power.SetShortTitleOverride(cutting_words_damage_roll_title_string);
                if (previous_damage_roll_penalty_power != null)
                {
                    damage_penalty_power.SetOverriddenPower(previous_damage_roll_penalty_power);
                }
                previous_damage_roll_penalty_power = damage_penalty_power;

                var feature_set = Helpers.FeatureSetBuilder.createFeatureSet("BardClassCuttingWordsFeature" + dice[i].ToString(),
                                                                             "",
                                                                             Helpers.StringProcessing.appendToString(cutting_words_title_string,
                                                                                                                     cutting_words_title_string + dice[i].ToString(),
                                                                                                                     $" ({dice[i].ToString().ToString().ToLower()})"),
                                                                             cutting_words_description_string,
                                                                             false,
                                                                             FeatureDefinitionFeatureSet.FeatureSetMode.Union,
                                                                             false,
                                                                             attack_penalty_power,
                                                                             damage_penalty_power
                                                                             );


                cutting_words.Add(dice[i], feature_set);
            }
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
                                                                            Helpers.StringProcessing.concatenateStrings(Common.common_condition_prefix,
                                                                                                                        countercharm_title_string,
                                                                                                                        "Rules/&BardClassCountercharmEffectCondition"
                                                                                                                        ),
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
            magical_secrets14 = Helpers.ExtraSpellSelectionBuilder.createExtraSpellSelection("BardClassMagicalSecrets14",
                                                                                "",
                                                                                "Feature/&BardClassMagicalSecretsTitle",
                                                                                "Feature/&BardClassMagicalSecretsDescription",
                                                                                bard_class,
                                                                                14,
                                                                                2,
                                                                                spelllist
                                                                                );
            magical_secrets18 = Helpers.ExtraSpellSelectionBuilder.createExtraSpellSelection("BardClassMagicalSecrets18",
                                                                                "",
                                                                                "Feature/&BardClassMagicalSecretsTitle",
                                                                                "Feature/&BardClassMagicalSecretsDescription",
                                                                                bard_class,
                                                                                18,
                                                                                2,
                                                                                spelllist
                                                                                );
            magical_secrets_spelllist = spelllist;
        }


        static void createSongOfRest()
        {
            string song_of_rest_title_string = "Feature/&BardClassSongOfRestTitle";
            string song_of_rest_description_string = "Feature/&BardClassSongOfRestDescription";

            var dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8 };

            for (int i = 0; i < dice.Length; i++)
            {
                var feature = Helpers.FeatureBuilder<NewFeatureDefinitions.FeatureDefinitionExtraHealingDieOnShortRest>.createFeature("BardClassSongOfRestFeature" + dice[i].ToString(),
                                                                                                                 "",
                                                                                                                 Helpers.StringProcessing.appendToString(song_of_rest_title_string,
                                                                                                                                                         song_of_rest_title_string + dice[i].ToString(),
                                                                                                                                                         $" ({dice[i].ToString().ToString().ToLower()})"),
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
            var dice = inspiration_dice;
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
                                                                                                      Helpers.StringProcessing.concatenateStrings(Common.common_condition_prefix,
                                                                                                                                                  inspiration_title_string,
                                                                                                                                                  "Rules/&BardClassInspirationCondition" + dice[i].ToString()
                                                                                                                                                  ),
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
                                                                         Helpers.StringProcessing.appendToString(inspiration_title_string,
                                                                                                                 inspiration_title_string + dice[i].ToString(),
                                                                                                                 $" ({dice[i].ToString().ToString().ToLower()})"),
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

            BardFeatureDefinitionSubclassChoice.Subclasses.Add(createLoreCollege().Name);
            BardClass.FeatureUnlocks.Sort(
                delegate (FeatureUnlockByLevel a, FeatureUnlockByLevel b)
                {
                    return a.Level - b.Level;
                });
        }

        private static FeatureDefinitionSubclassChoice BardFeatureDefinitionSubclassChoice;
    }
}

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

        protected BardClassBuilder(string name, string guid) : base(name, guid)
        {
            var rogue = DatabaseHelper.CharacterClassDefinitions.Rogue;
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
            Definition.AbilityScoresPriority.AddRange(new List<string> {"Charisma", "Dexterity", "Constitution", "Intelligence", "Strength", "Wisdom" });

            Definition.FeatAutolearnPreference.AddRange(rogue.FeatAutolearnPreference);
            Definition.PersonalityFlagOccurences.AddRange(rogue.PersonalityFlagOccurences);

            Definition.SkillAutolearnPreference.Clear();
            Definition.SkillAutolearnPreference.AddRange(new List<string>{"Persuasion", "Deception", "Intimidation", "Stealth", "Acrobatics", "Investigation", "Arcana", "History", "Insight" });

            Definition.ToolAutolearnPreference.Clear();
            Definition.ToolAutolearnPreference.AddRange(new List<string> { "ThievesToolsType",  "EnchantingToolType" });


            Definition.EquipmentRows.AddRange(rogue.EquipmentRows);
            Definition.EquipmentRows.Clear();
            List<CharacterClassDefinition.HeroEquipmentOption> list = new List<CharacterClassDefinition.HeroEquipmentOption>();
            List<CharacterClassDefinition.HeroEquipmentOption> list2 = new List<CharacterClassDefinition.HeroEquipmentOption>();
            List<CharacterClassDefinition.HeroEquipmentOption> list22 = new List<CharacterClassDefinition.HeroEquipmentOption>();
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
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Leather, EquipmentDefinitions.OptionArmor, 1)
            });

            Definition.FeatureUnlocks.Clear();
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassSavingthrowProficiencyBuilder.BardClassSavingthrowProficiency, 1)); 
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassArmorProficiencyBuilder.BardClassArmorProficiency, 1)); //Same armor as rogue
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassWeaponProficiencyBuilder.BardClassWeaponProficiency, 1)); //Same weapons as rogue
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassSkillPointPoolBuilder.BardClassSkillPointPool, 1)); //any 3 skills
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassToolsProficiencyBuilder.BardClassToolsProficiency, 1)); //thieving tools and enchanting tools
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassSpellcastingBuilder.BardClassSpellCasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassRitualSpellcastingBuilder.BardClassRitualSpellcasting, 1)); //same as any other caster

            //Level 3 Additional rage use - Add additional uses through subclasses since most times the subclass alters the rage power anyways.
            //Subclass feature at level 3
            var subclassChoicesGuiPresentation = new GuiPresentation();
            subclassChoicesGuiPresentation.Title = "Subclass/&BardSubclassPathTitle";
            subclassChoicesGuiPresentation.Description = "Subclass/&BardSubclassPathDescription";
            BardFeatureDefinitionSubclassChoice = this.BuildSubclassChoice(3, "Path", false, "SubclassChoiceBardSpecialistArchetypes", subclassChoicesGuiPresentation, BardClassSubclassesGuid);

            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 4));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierFighterExtraAttack, 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassFastMovementMovementAffinityBuilder.FastMovementMovementAffinity, 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassFastMovementMovementPowerForLevelUpDescriptionBuilder.FastMovementMovementPowerForLevelUpDescription, 5));
            //Level 6 Additional rage use - Add additional uses through subclasses since most times the subclass alters the rage power anyways.
            //SubclassFeature at level 6
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionCombatAffinitys.CombatAffinityEagerForBattle, 7));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(BardClassInitiativeAdvantagePowerForLevelUpDescriptionBuilder.InitiativeAdvantagePowerForLevelUpDescription, 7));
            //Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionCampAffinitys.CampAffinityFeatFocusedSleeper, 7)); //Could use this to helps not be asleep in camp maybe?  Could add the full Oblivion domain thing? Not sure
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 8));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierMartialChampionImprovedCritical, 9)); //Ideally we could add extra damage on crit but I don't think that's possible
            //Subclass feature at level 10

            //Above level 10 features
            //Level 11 Relentless Rage
            //Level 12 Rage use increase
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 12));
            //Level 13 Brutal Critical (2 dice)
            //Level 14 Path feature
            //Level 15 Persistent Rage
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
            //Level 16 Rage damage increase
            //Level 16 Brutal Critical (3 dice)
            //Level 17 Rage use increase
            //Level 18 Indomitable Might
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 19));
            //Level 20 	Primal Champion
            //Level 20 Unlimited Rages

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

    internal class BardClassSavingthrowProficiencyBuilder : BaseDefinitionBuilder<FeatureDefinitionProficiency>
    {
        const string BardClassSavingthrowProficiencyName = "BardSavingthrowProficiency";
        const string BardClassSavingthrowProficiencyGuid = "4eeb641f-e5a5-46ca-902e-a504b50c0247";

        protected BardClassSavingthrowProficiencyBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueSavingThrow, name, guid)
        {
            Definition.Proficiencies.Clear();
            Definition.Proficiencies.AddRange(new List<string> { "Dexterity", "Charisma" });
        }

        public static FeatureDefinitionProficiency CreateAndAddToDB(string name, string guid)
            => new BardClassSavingthrowProficiencyBuilder(name, guid).AddToDB();

        public static FeatureDefinitionProficiency BardClassSavingthrowProficiency= CreateAndAddToDB(BardClassSavingthrowProficiencyName, BardClassSavingthrowProficiencyGuid);
    }


    internal class BardClassWeaponProficiencyBuilder : BaseDefinitionBuilder<FeatureDefinitionProficiency>
    {
        const string BardClassWeaponProficiencyName = "BardWeaponProficiency";
        const string BardClassWeaponProficiencyGuid = "9a0ef52f-052a-4838-b3d4-2096ab67453e";

        protected BardClassWeaponProficiencyBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueWeapon, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardWeaponProficiencyTitle";
            Definition.GuiPresentation.Description = "Feature/&WeaponTrainingShortDescription";
        }

        public static FeatureDefinitionProficiency CreateAndAddToDB(string name, string guid)
            => new BardClassWeaponProficiencyBuilder(name, guid).AddToDB();

        public static FeatureDefinitionProficiency BardClassWeaponProficiency = CreateAndAddToDB(BardClassWeaponProficiencyName, BardClassWeaponProficiencyGuid);
    }


    internal class BardClassArmorProficiencyBuilder : BaseDefinitionBuilder<FeatureDefinitionProficiency>
    {
        const string BardClassArmorProficiencyName = "BardArmorProficiency";
        const string BardClassArmorProficiencyGuid = "06d31b31-69db-40d7-8701-a8547c4dd063";

        protected BardClassArmorProficiencyBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueArmor, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardArmorProficiencyTitle";
            Definition.GuiPresentation.Description = "Feature/&ArmorTrainingShortDescription";
        }

        public static FeatureDefinitionProficiency CreateAndAddToDB(string name, string guid)
            => new BardClassArmorProficiencyBuilder(name, guid).AddToDB();

        public static FeatureDefinitionProficiency BardClassArmorProficiency = CreateAndAddToDB(BardClassArmorProficiencyName, BardClassArmorProficiencyGuid);
    }


    internal class BardClassToolsProficiencyBuilder : BaseDefinitionBuilder<FeatureDefinitionProficiency>
    {
        const string BardClassArmorProficiencyName = "BardToolsProficiency";
        const string BardClassArmorProficiencyGuid = "798b8e47-4205-4fce-ba90-0b3ea772e15e";

        protected BardClassToolsProficiencyBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueTools, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardToolsProficiencyTitle";
            Definition.GuiPresentation.Description = "Feature/&ToolProficiencyPluralShortDescription";

            Definition.Proficiencies.Clear();
            Definition.Proficiencies.AddRange(new List<string> { "ThievesToolsType", "EnchantingToolType" });
        }

        public static FeatureDefinitionProficiency CreateAndAddToDB(string name, string guid)
            => new BardClassToolsProficiencyBuilder(name, guid).AddToDB();

        public static FeatureDefinitionProficiency BardClassToolsProficiency = CreateAndAddToDB(BardClassArmorProficiencyName, BardClassArmorProficiencyGuid);
    }


    internal class BardClassSkillPointPoolBuilder : BaseDefinitionBuilder<FeatureDefinitionPointPool>
    {
        const string BardClassSkillPoolName = "BardClassSkillPointPool";
        const string BardClassSkillPoolNameGuid = "0bdd06ab-32a4-493d-9c0f-2b14b1357ee0";

        protected BardClassSkillPointPoolBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPointPools.PointPoolRogueSkillPoints, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassSkillPointPoolTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassSkillPointPoolDescription";

            Definition.SetPoolAmount(3);
            Definition.SetPoolType(HeroDefinitions.PointsPoolType.Skill);
            Definition.RestrictedChoices.AddRange(new string[] { "AnimalHandling", "Survival", "Medecine", "Religion" });
            Definition.RestrictedChoices.Sort();
        }

        public static FeatureDefinitionPointPool CreateAndAddToDB(string name, string guid)
            => new BardClassSkillPointPoolBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPointPool BardClassSkillPointPool = CreateAndAddToDB(BardClassSkillPoolName, BardClassSkillPoolNameGuid);
    }


    internal class BardClassRitualSpellcastingBuilder : BaseDefinitionBuilder<FeatureDefinitionFeatureSet>
    {
        const string BardClassRitualSpellcastingName = "BardClassRitualSpellcasting";
        const string BardClassRitualSpellcastingNameGuid = "25c48b9b-e2e9-4ea7-8a80-e6c413275980";

        protected BardClassRitualSpellcastingBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetWizardRitualCasting, name, guid)
        {
            Definition.GuiPresentation.Description = "Feature/&BardClassRitualCastingDescription";
        }

        public static FeatureDefinitionFeatureSet CreateAndAddToDB(string name, string guid)
            => new BardClassRitualSpellcastingBuilder(name, guid).AddToDB();

        public static FeatureDefinitionFeatureSet BardClassRitualSpellcasting = CreateAndAddToDB(BardClassRitualSpellcastingName, BardClassRitualSpellcastingNameGuid);
    }


    internal class BardClassSpelllistBuilder : BaseDefinitionBuilder<SpellListDefinition>
    {
        const string BardClassSpelllistName = "BardClassSpelllist";
        const string BardClassSpelllistNameGuid = "0f3d14a7-f9a1-41ec-a164-f3e0f3800104";

        protected BardClassSpelllistBuilder(string name, string guid) : base(DatabaseHelper.SpellListDefinitions.SpellListWizard, name, guid)
        {
            Definition.SpellsByLevel[0].Spells.Clear();
            Definition.SpellsByLevel[0].Spells.AddRange(
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
                }
            );

            Definition.SpellsByLevel[1].Spells.Clear();
            Definition.SpellsByLevel[1].Spells.AddRange(
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
                }
            );

            Definition.SpellsByLevel[2].Spells.Clear();
            Definition.SpellsByLevel[2].Spells.AddRange(
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
                    DatabaseHelper.SpellDefinitions.MirrorImage,
                    DatabaseHelper.SpellDefinitions.SeeInvisibility,
                    DatabaseHelper.SpellDefinitions.Shatter,
                    DatabaseHelper.SpellDefinitions.Silence
                }
            );

            Definition.SpellsByLevel[3].Spells.Clear();
            Definition.SpellsByLevel[3].Spells.AddRange(
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
                }
            );

            Definition.SpellsByLevel[4].Spells.Clear();
            Definition.SpellsByLevel[4].Spells.AddRange(
                new List<SpellDefinition>
                {
                    DatabaseHelper.SpellDefinitions.Confusion,
                    DatabaseHelper.SpellDefinitions.DimensionDoor,
                    DatabaseHelper.SpellDefinitions.FreedomOfMovement,
                    DatabaseHelper.SpellDefinitions.GreaterInvisibility,
                    DatabaseHelper.SpellDefinitions.PhantasmalKiller
                }
            );

            Definition.SpellsByLevel[5].Spells.Clear();
            Definition.SpellsByLevel[5].Spells.AddRange(
                new List<SpellDefinition>
                {
                    DatabaseHelper.SpellDefinitions.DominatePerson,
                    DatabaseHelper.SpellDefinitions.GreaterRestoration,
                    DatabaseHelper.SpellDefinitions.HoldMonster,
                    DatabaseHelper.SpellDefinitions.MassCureWounds,
                    DatabaseHelper.SpellDefinitions.RaiseDead
                }
            );
        }

        public static SpellListDefinition CreateAndAddToDB(string name, string guid)
            => new BardClassSpelllistBuilder(name, guid).AddToDB();

        public static SpellListDefinition BardClassSpelllist = CreateAndAddToDB(BardClassSpelllistName, BardClassSpelllistNameGuid);
    }


    internal class BardClassSpellcastingBuilder : BaseDefinitionBuilder<FeatureDefinitionCastSpell>
    {
        const string BardClassSpellcastingName = "BardClassSpellcasting";
        const string BardClassSpellcastingNameGuid = "f720edaf-92c4-43e3-8228-c48c0b41b93b";

        protected BardClassSpellcastingBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassSpellcastingTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassSpellcastingDescription";

            Definition.SetSpellcastingAbility("Charisma");
            Definition.SetSpellKnowledge(RuleDefinitions.SpellKnowledge.Selection);
            Definition.SetSpellReadyness(RuleDefinitions.SpellReadyness.AllKnown);
            Definition.ScribedSpells.Clear();
            Definition.ScribedSpells.AddRange(Enumerable.Repeat(0, 20));
            Definition.KnownSpells.Clear();
            Definition.KnownSpells.AddRange(new List<int> {4,  5,  6,  7,  8,  9,  10, 11, 12, 14,
                                                           15, 15, 16, 18, 19, 19, 20, 22, 22, 22});
            Definition.SetSpellListDefinition(BardClassSpelllistBuilder.BardClassSpelllist);
        }

        public static FeatureDefinitionCastSpell CreateAndAddToDB(string name, string guid)
            => new BardClassSpellcastingBuilder(name, guid).AddToDB();

        public static FeatureDefinitionCastSpell BardClassSpellCasting = CreateAndAddToDB(BardClassSpellcastingName, BardClassSpellcastingNameGuid);
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

using SolastaModApi;
using SolastaModApi.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using static FeatureDefinitionSavingThrowAffinity;

namespace SolastaBardClass
{
    public static class BardSubclassPathOfTheReaver
    {
        public static Guid BardSubclassPathOfTheReaverGuid = new Guid("e922e24c-16fb-44dc-9a67-6831ea1ad037");

        const string BardSubClassPathOfTheReaverName = "BardSubclassPathOfTheReaver";
        private static readonly string BardSubClassPathOfTheBearNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardSubClassPathOfTheReaverName).ToString();

        public static CharacterSubclassDefinition Build()
        {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                    "Subclass/&BardSubclassPathOfTheReaverDescription",
                    "Subclass/&BardSubclassPathOfTheReaverTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.RoguishDarkweaver.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder(BardSubClassPathOfTheReaverName, BardSubClassPathOfTheBearNameGuid)
                    .SetGuiPresentation(subclassGuiPresentation)
                    .AddFeatureAtLevel(BardClassPathOfTheReaverRageClassPowerBuilder.RageClassPower, 3) // Special rage and increase rage count to 3 - D4 necrotic damage per attack (very similar damage to the 5e Zealot if multiple attacks hit - This could be changed to once per turn instead like ColossusSlayer though, but this opens up for synergy with the level 6 feature of extra attack on kill!)
                    .AddFeatureAtLevel(BardClassPathOfTheReaverRageClassPowerLevel6Builder.RageClassPower, 6) //Up rage count to 4 - Increase Necrotic damage to d6
                    .AddFeatureAtLevel(BardPathOfTheReaverExtraAttackOnKillBuilder.PathOfTheReaverExtraAttackOnKill, 6)
                    .AddFeatureAtLevel(BardClassPathOfTheReaverRageClassPowerLevel9Builder.RageClassPower, 9) //Up base damage on rage
                    .AddFeatureAtLevel(BardClassPathOfTheReaverRageClassPowerLevel10Builder.RageClassPower, 10)//Increase Necrotic damage to d8
                    .AddFeatureAtLevel(BardPathOfTheReaverDeathsFearPowerBuilder.DeathsFear, 10) //Extra feature Deaths Fear
                    .AddToDB();

            return definition;
        }
    }

    internal class BardClassPathOfTheReaverRageClassPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfTheReaverRageClassPowerName = "BardClassPathOfTheReaverRageClassPower";
        private static readonly string BardClassPathOfTheReaverRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardClassPathOfTheReaverRageClassPowerName).ToString();

        protected BardClassPathOfTheReaverRageClassPowerBuilder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfTheReaverRageClassPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfTheReaverRageClassPowerDescription";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassRageClassPowerBuilder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(3); //3 uses at level 3 when this is introduced
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfTheReaverRageClassPowerTitle");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheReaverRageClassConditionBuilder.PathOfTheReaverRageCondition;

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
            => new BardClassPathOfTheReaverRageClassPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfTheReaverRageClassPowerName, BardClassPathOfTheReaverRageClassPowerNameGuid);
    }

    internal class BardClassPathOfTheReaverRageClassPowerLevel6Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfTheReaverRageClassPowerName = "BardClassPathOfTheReaverRageClassPowerLevel6";
        private static readonly string BardClassPathOfTheReaverRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardClassPathOfTheReaverRageClassPowerName).ToString();

        protected BardClassPathOfTheReaverRageClassPowerLevel6Builder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel6Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfTheReaverRageClassPowerBuilder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4);
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfTheReaverRageClassPowerLevel6Title");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheReaverRageClassConditionLevel6Builder.PathOfTheReaverRageConditionLevel6;

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
            => new BardClassPathOfTheReaverRageClassPowerLevel6Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfTheReaverRageClassPowerName, BardClassPathOfTheReaverRageClassPowerNameGuid);
    }

    internal class BardClassPathOfTheReaverRageClassPowerLevel9Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfTheReaverRageClassPowerName = "BardClassPathOfTheReaverRageClassPowerLevel9";
        private static readonly string BardClassPathOfTheReaverRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardClassPathOfTheReaverRageClassPowerName).ToString();

        protected BardClassPathOfTheReaverRageClassPowerLevel9Builder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel9Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel9Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfTheReaverRageClassPowerLevel6Builder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4);
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfTheReaverRageClassPowerLevel9Title");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheReaverRageClassConditionLevel9Builder.PathOfTheReaverRageConditionLevel9; //Add extra 1 rage damage from base class

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
            => new BardClassPathOfTheReaverRageClassPowerLevel9Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfTheReaverRageClassPowerName, BardClassPathOfTheReaverRageClassPowerNameGuid);
    }

    internal class BardClassPathOfTheReaverRageClassPowerLevel10Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfTheReaverRageClassPowerName = "BardClassPathOfTheReaverRageClassPowerLevel10";
        private static readonly string BardClassPathOfTheReaverRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardClassPathOfTheReaverRageClassPowerName).ToString();

        protected BardClassPathOfTheReaverRageClassPowerLevel10Builder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel10Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfTheReaverRageClassPowerLevel10Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfTheReaverRageClassPowerLevel9Builder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4);
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfTheReaverRageClassPowerLevel10Title");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfTheReaverRageClassConditionLevel10Builder.PathOfTheReaverRageConditionLevel10;//Condition doesn't change at level 10

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
            => new BardClassPathOfTheReaverRageClassPowerLevel10Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfTheReaverRageClassPowerName, BardClassPathOfTheReaverRageClassPowerNameGuid);
    }


    internal class BardPathOfTheReaverRageClassConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfTheReaverRageClassConditionName = "BardPathOfTheReaverRageClassCondition";
        private static readonly string BardPathOfTheReaverRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverRageClassConditionName).ToString();

        protected BardPathOfTheReaverRageClassConditionBuilder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverRageClassConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverRageClassConditionDescription";

            Definition.SetAllowMultipleInstances(false);
            //Already has the base damage resist & damage features
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(BardPathOfTheReaverExtraDamageBuilder.PathOfTheReaverExtraNecroticDamage);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);

            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverRageClassConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition PathOfTheReaverRageCondition
            = CreateAndAddToDB(BardPathOfTheReaverRageClassConditionName, BardPathOfTheReaverRageClassConditionNameGuid);
    }

    internal class BardPathOfTheReaverRageClassConditionLevel6Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfTheReaverRageClassConditionName = "BardPathOfTheReaverRageClassConditionLevel6";
        private static readonly string BardPathOfTheReaverRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverRageClassConditionName).ToString();

        protected BardPathOfTheReaverRageClassConditionLevel6Builder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverRageClassConditionLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverRageClassConditionLevel6Description";

            Definition.SetAllowMultipleInstances(false);
            //Already has the base damage resist & damage features
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(BardPathOfTheReaverExtraDamageLevel6Builder.PathOfTheReaverExtraNecroticDamageLevel6);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);

            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverRageClassConditionLevel6Builder(name, guid).AddToDB();

        public static ConditionDefinition PathOfTheReaverRageConditionLevel6
            = CreateAndAddToDB(BardPathOfTheReaverRageClassConditionName, BardPathOfTheReaverRageClassConditionNameGuid);
    }

    internal class BardPathOfTheReaverRageClassConditionLevel9Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfTheReaverRageClassConditionName = "BardPathOfTheReaverRageClassConditionLevel9";
        private static readonly string BardPathOfTheReaverRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverRageClassConditionName).ToString();

        protected BardPathOfTheReaverRageClassConditionLevel9Builder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverRageClassConditionLevel9Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverRageClassConditionLevel9Description";

            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierLevel9Builder.RageClassDamageBonusAttackLevel9Modifier); //Level 9 base rage damage
            Definition.Features.Add(BardPathOfTheReaverExtraDamageLevel6Builder.PathOfTheReaverExtraNecroticDamageLevel6);

            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverRageClassConditionLevel9Builder(name, guid).AddToDB();

        public static ConditionDefinition PathOfTheReaverRageConditionLevel9
            = CreateAndAddToDB(BardPathOfTheReaverRageClassConditionName, BardPathOfTheReaverRageClassConditionNameGuid);
    }

    internal class BardPathOfTheReaverRageClassConditionLevel10Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfTheReaverRageClassConditionName = "BardPathOfTheReaverRageClassConditionLevel10";
        private static readonly string BardPathOfTheReaverRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverRageClassConditionName).ToString();

        protected BardPathOfTheReaverRageClassConditionLevel10Builder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverRageClassConditionLevel10Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverRageClassConditionLevel10Description";

            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityNecroticResistance);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierLevel9Builder.RageClassDamageBonusAttackLevel9Modifier); //Level 9 base rage damage
            Definition.Features.Add(BardPathOfTheReaverExtraDamageLevel10Builder.PathOfTheReaverExtraNecroticDamageLevel10);

            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverRageClassConditionLevel10Builder(name, guid).AddToDB();

        public static ConditionDefinition PathOfTheReaverRageConditionLevel10
            = CreateAndAddToDB(BardPathOfTheReaverRageClassConditionName, BardPathOfTheReaverRageClassConditionNameGuid);
    }

    internal class BardPathOfTheReaverExtraAttackOnKillBuilder : BaseDefinitionBuilder<FeatureDefinitionAdditionalAction>
    {
        const string BardPathOfTheReaverExtraAttackConditionName = "BardPathOfTheReaverExtraAttackCondition";
        private static readonly string BardPathOfTheReaverExtraAttackConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverExtraAttackConditionName).ToString();

        protected BardPathOfTheReaverExtraAttackOnKillBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAdditionalActions.AdditionalActionHunterHordeBreaker, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverExtraAttackOnKillTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverExtraAttackOnKillDescription";

            Definition.RestrictedActions.Clear();
        }

        public static FeatureDefinitionAdditionalAction CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverExtraAttackOnKillBuilder(name, guid).AddToDB();

        public static FeatureDefinitionAdditionalAction PathOfTheReaverExtraAttackOnKill
            = CreateAndAddToDB(BardPathOfTheReaverExtraAttackConditionName, BardPathOfTheReaverExtraAttackConditionNameGuid);
    }

    internal class BardPathOfTheReaverDeathsFearPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string IntimidatePowerName = "BardPathOfTheReaverDeathsFearPower";
        private static readonly string IntimidatePowerNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, IntimidatePowerName).ToString();

        protected BardPathOfTheReaverDeathsFearPowerBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerFighterSecondWind, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverDeathsFearPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverDeathsFearPowerDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetFixedUsesPerRecharge(1);
            Definition.SetCostPerUse(1);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.Action);

            //Create the prone effect - Weirdly enough the motion form seems to also automatically apply the prone condition
            EffectForm fearEffect = new EffectForm();
            fearEffect.FormType = EffectForm.EffectFormType.Condition;
            fearEffect.ConditionForm = new ConditionForm();
            fearEffect.ConditionForm.ConditionDefinition = DatabaseHelper.ConditionDefinitions.ConditionFrightened;
            fearEffect.SavingThrowAffinity = RuleDefinitions.EffectSavingThrowType.Negates;
            fearEffect.SaveOccurence = RuleDefinitions.TurnOccurenceType.EndOfTurn;
            fearEffect.CanSaveToCancel = true;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(fearEffect);
            newEffectDescription.SetSavingThrowDifficultyAbility("Strength");
            newEffectDescription.SetDifficultyClassComputation(RuleDefinitions.EffectDifficultyClassComputation.AbilityScoreAndProficiency);
            newEffectDescription.SavingThrowAbility = "Wisdom";
            newEffectDescription.HasSavingThrow = true;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.PerceivingWithinDistance);
            newEffectDescription.SetTargetProximityDistance(30);
            newEffectDescription.SetTargetParameter(6);
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Enemy);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverDeathsFearPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower DeathsFear = CreateAndAddToDB(IntimidatePowerName, IntimidatePowerNameGuid);
    }

    internal class BardPathOfTheReaverExtraDamageBuilder : BaseDefinitionBuilder<FeatureDefinitionAdditionalDamage>
    {
        const string BardPathOfTheReaverExtraAttackConditionName = "BardPathOfTheReaverExtraDamage";
        private static readonly string BardPathOfTheReaverExtraAttackConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverExtraAttackConditionName).ToString();

        protected BardPathOfTheReaverExtraDamageBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAdditionalDamages.AdditionalDamageHunterColossusSlayer, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverExtraDamageTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverExtraDamageDescription";
            Definition.SetCachedName(Definition.GuiPresentation.Title);
            Definition.SetNotificationTag("DeathsTouch");

            Definition.SetTriggerCondition(RuleDefinitions.AdditionalDamageTriggerCondition.AlwaysActive);
            Definition.SetDamageDieType(RuleDefinitions.DieType.D4);
            Definition.SetAdditionalDamageType(RuleDefinitions.AdditionalDamageType.Specific);
            Definition.SetSpecificDamageType("DamageNecrotic");
            Definition.SetLimitedUsage(RuleDefinitions.FeatureLimitedUsage.None);
        }

        public static FeatureDefinitionAdditionalDamage CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverExtraDamageBuilder(name, guid).AddToDB();

        public static FeatureDefinitionAdditionalDamage PathOfTheReaverExtraNecroticDamage
            = CreateAndAddToDB(BardPathOfTheReaverExtraAttackConditionName, BardPathOfTheReaverExtraAttackConditionNameGuid);
    }

    internal class BardPathOfTheReaverExtraDamageLevel6Builder : BaseDefinitionBuilder<FeatureDefinitionAdditionalDamage>
    {
        const string BardPathOfTheReaverExtraAttackConditionName = "BardPathOfTheReaverExtraDamageLevel6";
        private static readonly string BardPathOfTheReaverExtraAttackConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverExtraAttackConditionName).ToString();

        protected BardPathOfTheReaverExtraDamageLevel6Builder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAdditionalDamages.AdditionalDamageHunterColossusSlayer, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverExtraDamageLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverExtraDamageLevel6Description";
            Definition.SetCachedName(Definition.GuiPresentation.Title);
            Definition.SetNotificationTag("DeathsTouch");

            Definition.SetTriggerCondition(RuleDefinitions.AdditionalDamageTriggerCondition.AlwaysActive);
            Definition.SetDamageDieType(RuleDefinitions.DieType.D6);
            Definition.SetAdditionalDamageType(RuleDefinitions.AdditionalDamageType.Specific);
            Definition.SetSpecificDamageType("DamageNecrotic");
            Definition.SetLimitedUsage(RuleDefinitions.FeatureLimitedUsage.None);
        }

        public static FeatureDefinitionAdditionalDamage CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverExtraDamageLevel6Builder(name, guid).AddToDB();

        public static FeatureDefinitionAdditionalDamage PathOfTheReaverExtraNecroticDamageLevel6
            = CreateAndAddToDB(BardPathOfTheReaverExtraAttackConditionName, BardPathOfTheReaverExtraAttackConditionNameGuid);
    }

    internal class BardPathOfTheReaverExtraDamageLevel10Builder : BaseDefinitionBuilder<FeatureDefinitionAdditionalDamage>
    {
        const string BardPathOfTheReaverExtraAttackConditionName = "BardPathOfTheReaverExtraDamageLevel10";
        private static readonly string BardPathOfTheReaverExtraAttackConditionNameGuid = GuidHelper.Create(BardSubclassPathOfTheReaver.BardSubclassPathOfTheReaverGuid, BardPathOfTheReaverExtraAttackConditionName).ToString();

        protected BardPathOfTheReaverExtraDamageLevel10Builder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAdditionalDamages.AdditionalDamageHunterColossusSlayer, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfTheReaverExtraDamageLevel10Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfTheReaverExtraDamageLevel10Description";
            Definition.SetCachedName(Definition.GuiPresentation.Title);
            Definition.SetNotificationTag("DeathsTouch");

            Definition.SetTriggerCondition(RuleDefinitions.AdditionalDamageTriggerCondition.AlwaysActive);
            Definition.SetDamageDieType(RuleDefinitions.DieType.D8);
            Definition.SetAdditionalDamageType(RuleDefinitions.AdditionalDamageType.Specific);
            Definition.SetSpecificDamageType("DamageNecrotic");
            Definition.SetLimitedUsage(RuleDefinitions.FeatureLimitedUsage.None);
        }

        public static FeatureDefinitionAdditionalDamage CreateAndAddToDB(string name, string guid)
            => new BardPathOfTheReaverExtraDamageLevel10Builder(name, guid).AddToDB();

        public static FeatureDefinitionAdditionalDamage PathOfTheReaverExtraNecroticDamageLevel10
            = CreateAndAddToDB(BardPathOfTheReaverExtraAttackConditionName, BardPathOfTheReaverExtraAttackConditionNameGuid);
    }
}

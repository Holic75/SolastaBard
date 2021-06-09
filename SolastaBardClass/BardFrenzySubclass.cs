using SolastaModApi;
using SolastaModApi.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using static FeatureDefinitionSavingThrowAffinity;

namespace SolastaBardClass
{
    public static class BardSubclassPathOfFrenzy
    {
        public static Guid BardSubclassPathOfFrenzyGuid = new Guid("ad24d8cd-76dd-460b-9f13-bc5abbb937b2");

        const string BardSubClassPathOfFrenzyName = "BardSubclassPathOfFrenzy";
        private static readonly string BardSubClassPathOfTheBearNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardSubClassPathOfFrenzyName).ToString();

        public static CharacterSubclassDefinition Build()
        {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                    "Subclass/&BardSubclassPathOfFrenzyDescription",
                    "Subclass/&BardSubclassPathOfFrenzyTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.MartialMountaineer.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder(BardSubClassPathOfFrenzyName, BardSubClassPathOfTheBearNameGuid)
                    .SetGuiPresentation(subclassGuiPresentation)
                    .AddFeatureAtLevel(BardClassPathOfFrenzyRageClassPowerBuilder.RageClassPower, 3) // Special rage and increase rage count to 3
                    .AddFeatureAtLevel(BardClassPathOfFrenzyRageClassPowerLevel6Builder.RageClassPower, 6) //Up rage count to 4 - Do in subclass since BearRage has its own characteristics
                    .AddFeatureAtLevel(BardClassPathOfFrenzyRageClassPowerLevel9Builder.RageClassPower, 9) //Up damage on rage - Do in subclass since BearRage has its own characteristics
                    .AddFeatureAtLevel(BardPathOfFrenzyIntimidatePowerBuilder.IntimidatePower, 10)
                    .AddToDB();

            return definition;
        }
    }

    internal class BardClassPathOfFrenzyRageClassPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfFrenzyRageClassPowerName = "BardClassPathOfFrenzyRageClassPower";
        private static readonly string BardClassPathOfFrenzyRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardClassPathOfFrenzyRageClassPowerName).ToString();

        protected BardClassPathOfFrenzyRageClassPowerBuilder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfFrenzyRageClassPowerTitle";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfFrenzyRageClassPowerDescription";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassRageClassPowerBuilder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(3); //3 uses at level 3 when this is introduced
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfFrenzyRageClassPowerTitle");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassConditionBuilder.PathOfFrenzyRageCondition;

            //Create the way to remove the hindered effect of ending rage
            EffectForm removeHindrancesEffect = new EffectForm();
            removeHindrancesEffect.ConditionForm = new ConditionForm();
            removeHindrancesEffect.FormType = EffectForm.EffectFormType.Condition;
            removeHindrancesEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.RemoveDetrimentalAll;
            removeHindrancesEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition;
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition);

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.EffectForms.Add(removeHindrancesEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfFrenzyRageClassPowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfFrenzyRageClassPowerName, BardClassPathOfFrenzyRageClassPowerNameGuid);
    }

    internal class BardClassPathOfFrenzyRageClassPowerLevel6Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfFrenzyRageClassPowerName = "BardClassPathOfFrenzyRageClassPowerLevel6";
        private static readonly string BardClassPathOfFrenzyRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardClassPathOfFrenzyRageClassPowerName).ToString();

        protected BardClassPathOfFrenzyRageClassPowerLevel6Builder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfFrenzyRageClassPowerLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfFrenzyRageClassPowerLevel6Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfFrenzyRageClassPowerBuilder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4);
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfFrenzyRageClassPowerLevel6Title");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassConditionLevel6Builder.PathOfFrenzyRageConditionLevel6;

            //Create the way to remove the hindered effect of ending rage
            EffectForm removeHindrancesEffect = new EffectForm();
            removeHindrancesEffect.ConditionForm = new ConditionForm();
            removeHindrancesEffect.FormType = EffectForm.EffectFormType.Condition;
            removeHindrancesEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.RemoveDetrimentalAll;
            removeHindrancesEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition;
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition);
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(DatabaseHelper.ConditionDefinitions.ConditionCharmed);
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(DatabaseHelper.ConditionDefinitions.ConditionFrightened);

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.EffectForms.Add(removeHindrancesEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfFrenzyRageClassPowerLevel6Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfFrenzyRageClassPowerName, BardClassPathOfFrenzyRageClassPowerNameGuid);
    }



    internal class BardClassPathOfFrenzyRageClassPowerLevel9Builder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string BardClassPathOfFrenzyRageClassPowerName = "BardClassPathOfFrenzyRageClassPowerLevel9";
        private static readonly string BardClassPathOfFrenzyRageClassPowerNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardClassPathOfFrenzyRageClassPowerName).ToString();

        protected BardClassPathOfFrenzyRageClassPowerLevel9Builder(string name, string guid) : base(BardClassRageClassPowerBuilder.RageClassPower, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardClassPathOfFrenzyRageClassPowerLevel9Title";
            Definition.GuiPresentation.Description = "Feature/&BardClassPathOfFrenzyRageClassPowerLevel9Description";

            FeatureDefinitionPowerExtensions.SetOverriddenPower(Definition, BardClassPathOfFrenzyRageClassPowerLevel6Builder.RageClassPower);

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.LongRest);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.BonusAction);
            Definition.SetCostPerUse(1);
            Definition.SetFixedUsesPerRecharge(4);
            Definition.SetShortTitleOverride("Feature/&BardClassPathOfFrenzyRageClassPowerLevel9Title");

            //Create the rage
            EffectForm rageEffect = new EffectForm();
            rageEffect.ConditionForm = new ConditionForm();
            rageEffect.FormType = EffectForm.EffectFormType.Condition;
            rageEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            rageEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassConditionLevel9Builder.PathOfFrenzyRageConditionLevel9;

            //Create the way to remove the hindered effect of ending rage
            EffectForm removeHindrancesEffect = new EffectForm();
            removeHindrancesEffect.ConditionForm = new ConditionForm();
            removeHindrancesEffect.FormType = EffectForm.EffectFormType.Condition;
            removeHindrancesEffect.ConditionForm.Operation = ConditionForm.ConditionOperation.RemoveDetrimentalAll;
            removeHindrancesEffect.ConditionForm.ConditionDefinition = BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition;
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition);
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(DatabaseHelper.ConditionDefinitions.ConditionCharmed);
            removeHindrancesEffect.ConditionForm.DetrimentalConditions.Add(DatabaseHelper.ConditionDefinitions.ConditionFrightened);

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(rageEffect);
            newEffectDescription.EffectForms.Add(removeHindrancesEffect);
            newEffectDescription.HasSavingThrow = false;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Minute;
            newEffectDescription.DurationParameter = 1;
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Self);
            newEffectDescription.SetCanBePlacedOnCharacter(true);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardClassPathOfFrenzyRageClassPowerLevel9Builder(name, guid).AddToDB();

        public static FeatureDefinitionPower RageClassPower
            = CreateAndAddToDB(BardClassPathOfFrenzyRageClassPowerName, BardClassPathOfFrenzyRageClassPowerNameGuid);
    }


    internal class BardPathOfFrenzyRageClassConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfFrenzyRageClassConditionName = "BardPathOfFrenzyRageClassCondition";
        private static readonly string BardPathOfFrenzyRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardPathOfFrenzyRageClassConditionName).ToString();

        protected BardPathOfFrenzyRageClassConditionBuilder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfFrenzyRageClassConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfFrenzyRageClassConditionDescription";

            Definition.SetAllowMultipleInstances(false);
            //Already has the damage resist & damage features
            Definition.Features.Add(BardPathOfFrenzyExtraAttackConditionBuilder.PathOfFrenzyExtraAttack);
            Definition.SetSubsequentOnRemoval(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition); //Consider a different detriment?
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);

            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyRageClassConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition PathOfFrenzyRageCondition
            = CreateAndAddToDB(BardPathOfFrenzyRageClassConditionName, BardPathOfFrenzyRageClassConditionNameGuid);
    }

    internal class BardPathOfFrenzyRageClassConditionLevel6Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfFrenzyRageClassConditionName = "BardPathOfFrenzyRageClassConditionLevel6";
        private static readonly string BardPathOfFrenzyRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardPathOfFrenzyRageClassConditionName).ToString();

        protected BardPathOfFrenzyRageClassConditionLevel6Builder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfFrenzyRageClassConditionLevel6Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfFrenzyRageClassConditionLevel6Description";

            Definition.SetAllowMultipleInstances(false);
            //Already has the damage resist & damage features
            Definition.Features.Add(BardPathOfFrenzyExtraAttackConditionBuilder.PathOfFrenzyExtraAttack);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionConditionAffinitys.ConditionAffinityCharmImmunity); //Add Charm/Frighten immunity
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionConditionAffinitys.ConditionAffinityFrightenedImmunity);
            Definition.SetSubsequentOnRemoval(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyRageClassConditionLevel6Builder(name, guid).AddToDB();

        public static ConditionDefinition PathOfFrenzyRageConditionLevel6
            = CreateAndAddToDB(BardPathOfFrenzyRageClassConditionName, BardPathOfFrenzyRageClassConditionNameGuid);
    }

    internal class BardPathOfFrenzyRageClassConditionLevel9Builder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfFrenzyRageClassConditionName = "BardPathOfFrenzyRageClassConditionLevel9";
        private static readonly string BardPathOfFrenzyRageClassConditionNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardPathOfFrenzyRageClassConditionName).ToString();

        protected BardPathOfFrenzyRageClassConditionLevel9Builder(string name, string guid) : base(BardClassRageClassConditionBuilder.RageClassCondition, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfFrenzyRageClassConditionLevel9Title";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfFrenzyRageClassConditionLevel9Description";

            Definition.SetAllowMultipleInstances(false);
            Definition.Features.Clear();
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionAbilityCheckAffinitys.AbilityCheckAffinityConditionBullsStrength);
            Definition.Features.Add(BardClassRageClassStrengthSavingThrowAffinityBuilder.RageClassStrengthSavingThrowAffinity);
            Definition.Features.Add(BardClassRageClassDamageBonusAttackModifierLevel9Builder.RageClassDamageBonusAttackLevel9Modifier); //Level 9 damage

            Definition.Features.Add(BardPathOfFrenzyExtraAttackConditionBuilder.PathOfFrenzyExtraAttack);
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionConditionAffinitys.ConditionAffinityCharmImmunity); //Add Charm/Frighten immunity
            Definition.Features.Add(DatabaseHelper.FeatureDefinitionConditionAffinitys.ConditionAffinityFrightenedImmunity);
            Definition.SetSubsequentOnRemoval(BardPathOfFrenzyRageClassHinderedConditionBuilder.PathOfFrenzyHinderedCondition);

            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(1);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyRageClassConditionLevel9Builder(name, guid).AddToDB();

        public static ConditionDefinition PathOfFrenzyRageConditionLevel9
            = CreateAndAddToDB(BardPathOfFrenzyRageClassConditionName, BardPathOfFrenzyRageClassConditionNameGuid);
    }



    internal class BardPathOfFrenzyRageClassHinderedConditionBuilder : BaseDefinitionBuilder<ConditionDefinition>
    {
        const string BardPathOfFrenzyRageClassHinderedConditionName = "BardPathOfFrenzyRageClassHinderedCondition";
        private static readonly string BardPathOfFrenzyRageClassHinderedConditionNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, BardPathOfFrenzyRageClassHinderedConditionName).ToString();

        protected BardPathOfFrenzyRageClassHinderedConditionBuilder(string name, string guid) : base(DatabaseHelper.ConditionDefinitions.ConditionHindered, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfFrenzyRageClassHinderedConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfFrenzyRageClassHinderedConditionDescription";

            Definition.SetAllowMultipleInstances(false);
            Definition.SetDurationType(RuleDefinitions.DurationType.Minute);
            Definition.SetDurationParameter(10);
            Definition.SetAdditionalCondition(DatabaseHelper.ConditionDefinitions.ConditionCursedByBestowCurseStrength);
        }

        public static ConditionDefinition CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyRageClassHinderedConditionBuilder(name, guid).AddToDB();

        public static ConditionDefinition PathOfFrenzyHinderedCondition
            = CreateAndAddToDB(BardPathOfFrenzyRageClassHinderedConditionName, BardPathOfFrenzyRageClassHinderedConditionNameGuid);
    }

    internal class BardPathOfFrenzyExtraAttackConditionBuilder : BaseDefinitionBuilder<FeatureDefinitionAdditionalAction>
    {
        const string BardPathOfFrenzyExtraAttackConditionName = "BardPathOfFrenzyExtraAttackCondition";
        private static readonly string BardPathOfFrenzyExtraAttackConditionNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, "BardPathOfFrenzyExtraAttackCondition").ToString();

        protected BardPathOfFrenzyExtraAttackConditionBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionAdditionalActions.AdditionalActionHasted, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&BardPathOfFrenzyExtraAttackConditionTitle";
            Definition.GuiPresentation.Description = "Feature/&BardPathOfFrenzyExtraAttackConditionDescription";

            Definition.RestrictedActions.Clear();
            Definition.RestrictedActions.Add(ActionDefinitions.Id.AttackMain);
        }

        public static FeatureDefinitionAdditionalAction CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyExtraAttackConditionBuilder(name, guid).AddToDB();

        public static FeatureDefinitionAdditionalAction PathOfFrenzyExtraAttack
            = CreateAndAddToDB(BardPathOfFrenzyExtraAttackConditionName, BardPathOfFrenzyExtraAttackConditionNameGuid);
    }

    internal class BardPathOfFrenzyIntimidatePowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        const string IntimidatePowerName = "BardIntimidatePower";
        private static readonly string IntimidatePowerNameGuid = GuidHelper.Create(BardSubclassPathOfFrenzy.BardSubclassPathOfFrenzyGuid, IntimidatePowerName).ToString();

        protected BardPathOfFrenzyIntimidatePowerBuilder(string name, string guid) : base(DatabaseHelper.FeatureDefinitionPowers.PowerFighterSecondWind, name, guid)
        {
            Definition.GuiPresentation.Title = "Feature/&IntimidatePowerTitle";
            Definition.GuiPresentation.Description = "Feature/&IntimidatePowerDescription";

            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.AtWill);
            Definition.SetCostPerUse(0);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.Action);

            //Create the prone effect - Weirdly enough the motion form seems to also automatically apply the prone condition
            EffectForm fearEffect = new EffectForm();
            fearEffect.FormType = EffectForm.EffectFormType.Condition;
            fearEffect.ConditionForm = new ConditionForm();
            fearEffect.ConditionForm.ConditionDefinition = DatabaseHelper.ConditionDefinitions.ConditionFrightened;
            fearEffect.SavingThrowAffinity = RuleDefinitions.EffectSavingThrowType.Negates;

            //Add to our new effect
            EffectDescription newEffectDescription = new EffectDescription();
            newEffectDescription.Copy(Definition.EffectDescription);
            newEffectDescription.EffectForms.Clear();
            newEffectDescription.EffectForms.Add(fearEffect);
            newEffectDescription.SetSavingThrowDifficultyAbility("Strength");
            newEffectDescription.SetDifficultyClassComputation(RuleDefinitions.EffectDifficultyClassComputation.AbilityScoreAndProficiency);
            newEffectDescription.SavingThrowAbility = "Wisdom";
            newEffectDescription.HasSavingThrow = true;
            newEffectDescription.DurationType = RuleDefinitions.DurationType.Round;
            newEffectDescription.DurationParameter = 2;
            newEffectDescription.SetRangeType(RuleDefinitions.RangeType.Distance);
            newEffectDescription.SetRangeParameter(6);
            newEffectDescription.SetTargetType(RuleDefinitions.TargetType.Individuals);
            newEffectDescription.SetTargetSide(RuleDefinitions.Side.Enemy);

            Definition.SetEffectDescription(newEffectDescription);
        }

        public static FeatureDefinitionPower CreateAndAddToDB(string name, string guid)
            => new BardPathOfFrenzyIntimidatePowerBuilder(name, guid).AddToDB();

        public static FeatureDefinitionPower IntimidatePower = CreateAndAddToDB(IntimidatePowerName, IntimidatePowerNameGuid);
    }
}

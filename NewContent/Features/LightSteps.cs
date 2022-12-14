using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Diagnostics.Tracing;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.UnitLogic.Mechanics.Components;
using LegacyOfShadows.NewComponents;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;
using LegacyOfShadows.Utilities;
using Kingmaker.UnitLogic.Abilities.Components;
using HlEX = LegacyOfShadows.Utilities.HelpersExtension;


namespace LegacyOfShadows.NewContent.Features
{

    // Just like in Holic75's version of the Ninja Light Steps is implemented as a weakened version of Monk's Abundant Step, which takes a full-round action to activate.
    internal class LightSteps
    {
        private static readonly string LightStepsFeatureName = "NinjaLightStepsFeature.Name";
        private static readonly string LightStepsFeatureDescription = "NinjaLightStepsFeature.Description";


        private static BlueprintCharacterClassReference[] RogueClassArray()
        {
            return new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
        }



        public static void ConfigureLightSteps() 
        {



            var LightStepsIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("f3c0b267dd17a2a45a40805e31fe3cd1").Icon; // LightSteps uses Feather Steps icon

            var MonkAbundantStepAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("f3c0b267dd17a2a45a40805e31fe3cd1"); // Abundant Step is used as reference.
            var MonkAbundantStepCasterDisappearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("b9a3f1855ab08bf42a8b119818bcc6dd");
            var MonkAbundantStepCasterAppearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("4125f30c999bddc4492bf91d73c4cf64");
            var MonkAbundantSideDisappearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("cdaff4fd8665656409ddffe42fbc07c1");
            var MonkAbundantSideAppearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("6c72207ef86803543b4b13352bcc5cf6");


            var LightStepsAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaLightStepsAbility", bp => {
                bp.SetName(LoSContext, LightStepsFeatureName);
                bp.SetDescription(LoSContext, LightStepsFeatureDescription);
                bp.m_Icon = LightStepsIcon;
                bp.AddComponent<AbilityCustomDimensionDoor>(c => {
                    c.Radius = 0.Feet();
                    c.PortalFromPrefab = HlEX.CreatePrefabLink("1886751171485164");
                    c.PortalToPrefab = HlEX.CreatePrefabLink("1886751171485164");
                    c.CasterDisappearFx = HlEX.CreatePrefabLink("ccd3a2dcada23c145b232501d105c55d"); // Uses the Invisibility Fx
                    c.SideDisappearFx = HlEX.CreatePrefabLink("ccd3a2dcada23c145b232501d105c55d"); // Uses the Invisibility Fx
                    c.m_CasterDisappearProjectile = MonkAbundantStepCasterDisappearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_CasterAppearProjectile = MonkAbundantStepCasterAppearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_SideDisappearProjectile = MonkAbundantSideDisappearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_SideAppearProjectile = MonkAbundantSideAppearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_CameraShouldFollow = false;
                });
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.IgnoreMinimalRangeLimit = false;
                bp.ShowNameForVariant = false;
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = false;
                bp.CanTargetFriends = false;
                bp.SpellResistance = false;
                bp.Hidden = false;
                bp.NeedEquipWeapons = false;
                bp.NotOffensive = false;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Move;
                bp.m_IsFullRoundAction = true;
                bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Quicken;
                bp.ResourceAssetIds = new string[0];


            });

           var LightStepsFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "NinjaLightStepsFeature", bp => {
                bp.SetName(LoSContext, LightStepsFeatureName);
                bp.SetDescription(LoSContext, LightStepsFeatureDescription);
                bp.m_Icon = LightStepsIcon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LightStepsAbility.ToReference<BlueprintUnitFactReference>(),
                    };


                });
            });

            LoSContext.Logger.LogPatch("Created Light Steps.", LightStepsFeature);


        }

    }

}

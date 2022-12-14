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
using System.Drawing;
using Kingmaker.UnitLogic.Buffs;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    // This has been called "Flurry of Stars" because I plan to re-introduce shurikens.


    internal class FlurryOfStars
    {
        private static readonly string FlurryOfStarsFeatureName = "NinjaTrickFlurryOfStarsFeature.Name";
        private static readonly string FlurryOfStarsFeatureDescription = "NinjaTrickFlurryOfStarsFeature.Description";



        public static void ConfigureFlurryOfStars()
        {

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var FlurryOfStarsIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "FlurryOfStars.png");

            var Flurry_Of_Stars_Buff = Helpers.CreateBlueprint<BlueprintBuff>(LoSContext, "FlurryOfStarsBuff", bp => {
                bp.SetName(LoSContext, FlurryOfStarsFeatureName);
                bp.SetDescription(LoSContext, FlurryOfStarsFeatureDescription);
                bp.m_Icon = FlurryOfStarsIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
                bp.AddComponent(Helpers.Create<BuffExtraAttackCategorySpecific>(c => {
                    c.Categories = new WeaponCategory[] { WeaponCategory.Dart, WeaponCategory.Shuriken };
                    c.Extra_Attacks = 2; 
                    c.Attack_Bonus = -2;
                }));

            });

            var Apply_Flurry_Of_Stars_Buff = HlEX.CreateContextActionApplyBuff(Flurry_Of_Stars_Buff.ToReference<BlueprintBuffReference>(),
                                             HlEX.CreateContextDuration(1), 
                                             false, false, false, false, false);

            var FlurryOfStarsAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaTrickFlurryOfStarsAbility", bp => {
                bp.SetName(LoSContext, FlurryOfStarsFeatureName);
                bp.SetDescription(LoSContext, FlurryOfStarsFeatureDescription);
                bp.m_Icon = FlurryOfStarsIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Extraordinary;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(LoSContext, "NinjaTrickFlurryOfStarsAbility.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Flurry_Of_Stars_Buff));
                bp.AddComponent(Helpers.Create<AbilityCasterMainWeaponCheck>(c => {
                    c.Category = new WeaponCategory[] { WeaponCategory.Dart, WeaponCategory.Shuriken };
                }));
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            FlurryOfStarsAbility.SetMiscAbilityParametersSelfOnly();

            var flurry_of_stars_feature = HlEX.ConvertAbilityToFeature(FlurryOfStarsAbility, "", "", "Feature", "Ability", false);

            flurry_of_stars_feature.AddComponent(Helpers.Create<ConsiderWeaponCategoriesAsLightWeapon>(c => {
                 c.Categories = new WeaponCategory[] { WeaponCategory.Dart, WeaponCategory.Shuriken };
             }));

            LoSContext.Logger.LogPatch("Created Flurry of Stars ninja trick.", flurry_of_stars_feature);


        }
















    }
}

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

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class Kamikaze
    {
        private static readonly string KamikazeFeatureName = "NinjaTrickKamikazeFeature.Name";
        private static readonly string KamikazeAbilityName = "NinjaTrickKamikazeAbility.Name";
        private static readonly string KamikazeDismissAbilityName = "NinjaTrickKamikazeDismissAbility.Name";
        private static readonly string KamikazeFeatureDescription = "NinjaTrickKamikazeFeature.Description";
        private static readonly string KamikazeAbilityDescription = "NinjaTrickKamikazeAbility.Description";
        private static readonly string KamikazeDismissAbilityDescription = "NinjaTrickKamikazeDismissAbility.Description";
        static public BlueprintFeature NinjaTrickKamikazeFeature;

        public static void ConfigureKamikaze()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var Rage_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("a1ffec0ce7c167a40aaea13dc49b757b");

            var Bloodrager_Fey_Fury_Of_The_Fey_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("c011a9d324475b74ab066a9657292e7d");

            var Rage_Buff_Fx_Asset_ID = Rage_Buff.FxOnStart.AssetId;

            var Vicious_Enchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a1455a289da208144981e4b1ef92cc56");

            var KamikazeIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "Kamikaze.png");

            var KamikazeDismissIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KamikazeDismiss");

            var Kamikaze_Buff = Bloodrager_Fey_Fury_Of_The_Fey_Buff.CreateCopy(LoSContext, "KamikazeBuff", bp => {
                                                                                    bp.FlattenAllActions()
                                                                                        .OfType<BuffEnchantAnyWeapon>()
                                                                                        .ForEach(a => {
                                                                                            a.m_EnchantmentBlueprint = Vicious_Enchantment.ToReference<BlueprintItemEnchantmentReference>();
                                                                                        });
                                                                                    bp.m_Icon = KamikazeIcon;
                                                                                    bp.SetName(LoSContext, KamikazeFeatureName);
                                                                                    bp.SetDescription(LoSContext, KamikazeFeatureDescription);
                                                                                    bp.FxOnStart = HlEX.CreatePrefabLink(Rage_Buff_Fx_Asset_ID);
                                                                                });

            var Apply_Kamikaze_Buff = HlEX.CreateContextActionApplyBuff(Kamikaze_Buff.ToReference<BlueprintBuffReference>(),
                                                                HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default), DurationRate.TenMinutes),
                                                                false, false, false, false, false);


            var KamikazeAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaTrickKamikazeAbility", bp => {
                bp.SetName(LoSContext, KamikazeAbilityName);
                bp.SetDescription(LoSContext, KamikazeAbilityDescription);
                bp.m_Icon = KamikazeIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(LoSContext, "NinjaTrickKamikazeAbility.Duration", "1 round/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Kamikaze_Buff));
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            KamikazeAbility.SetMiscAbilityParametersSelfOnly();

            var Dismiss_Kamikaze_Buff = HlEX.CreateConditional(HlEX.CreateContextConditionHasBuffFromCaster(Kamikaze_Buff),HlEX.CreateContextActionRemoveBuff(Kamikaze_Buff), null);

            var KamikazeDismissAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaTrickKamikazeDismissAbility", bp => {
                bp.SetName(LoSContext, KamikazeDismissAbilityName);
                bp.SetDescription(LoSContext, KamikazeDismissAbilityDescription);
                bp.m_Icon = KamikazeDismissIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Dismiss_Kamikaze_Buff));
                bp.AddComponent(kiResource.CreateResourceLogic());
            });


            var Kamikaze_Variants = new BlueprintAbility[] { KamikazeAbility, KamikazeDismissAbility };


            var Kamikaze_Wrapper_Ability = HlEX.CreateVariantWrapper("NinjaTrickKamikazeBaseAbility", Kamikaze_Variants);

            Kamikaze_Wrapper_Ability.SetName(LoSContext, KamikazeFeatureName);
            Kamikaze_Wrapper_Ability.SetDescription(LoSContext, KamikazeFeatureDescription);

            var kamikaze_feature = HlEX.ConvertAbilityToFeature(Kamikaze_Wrapper_Ability, "", "", "Feature", "BaseAbility", false);


        }

    }
}

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
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem;
using static RootMotion.FinalIK.RagdollUtility;
using System;
using static RootMotion.FinalIK.InteractionTrigger;

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class VanishingTrick
    {

        private static readonly string VanishingTrickFeatureName = "NinjaTrickVanishingTrickFeature.Name";
        private static readonly string VanishingTrickFeatureDescription = "NinjaTrickVanishingTrickFeature.Description";

        public static void ConfigureVanishingTrick()
        {
            var Invisibility_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("525f980cb29bc2240b93e953974cb325");
            var Improved_Invisibility_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6b35473a237a6045969253beb09777c");

            var VanishingTrickIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "VanishingTrick.png");

            var Apply_Invisibility_Buff = Helpers.Create<ContextActionApplyBuff>(a => {
                                                                                                    a.m_Buff = Invisibility_Buff.ToReference<BlueprintBuffReference>();
                                                                                                    a.DurationValue = new ContextDurationValue()
                                                                                                    {
                                                                                                        BonusValue = new ContextValue() {

                                                                                                            ValueType = ContextValueType.Rank,
                                                                                                            ValueRank = 0
                                                                                                        },
                                                                                                        Rate = DurationRate.Rounds,
                                                                                                        DiceCountValue = 0,
                                                                                                        DiceType = DiceType.Zero

                                                                                                    };
                                                                                                    a.IsFromSpell = false;
                                                                                                    a.IsNotDispelable = true;
                                                                                                    a.ToCaster = false;
                                                                                                    a.AsChild = false;
                                                                                                    a.Permanent = false;
                                                                                            });


            var Apply_Improved_Invisibility_Buff = Helpers.Create<ContextActionApplyBuff>(a => {
                                                                                                    a.m_Buff = Improved_Invisibility_Buff.ToReference<BlueprintBuffReference>();
                                                                                                    a.DurationValue = new ContextDurationValue()
                                                                                                    {
                                                                                                        BonusValue = new ContextValue()
                                                                                                        {

                                                                                                            ValueType = ContextValueType.Rank,
                                                                                                            ValueRank = 0
                                                                                                        },
                                                                                                        Rate = DurationRate.Rounds,
                                                                                                        DiceCountValue = 0,
                                                                                                        DiceType = DiceType.Zero

                                                                                                    };
                                                                                                    a.IsFromSpell = false;
                                                                                                    a.IsNotDispelable = true;
                                                                                                    a.ToCaster = false;
                                                                                                    a.AsChild = false;
                                                                                                    a.Permanent = false;
                                                                                               });

            var upgrade_action



            var VanishingTrickAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaTrickVanishingTrickAbility", bp => {
                bp.SetName(LoSContext, VanishingTrickFeatureName);
                bp.SetDescription(LoSContext, VanishingTrickFeatureDescription);
                bp.m_Icon = VanishingTrickIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(LoSContext, "VanishingTrickAbility.Duration", "1 round/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();

            });

        }
    }
}

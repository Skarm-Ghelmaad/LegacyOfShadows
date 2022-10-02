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

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class InvisibleBlade
    {
        private static readonly string InvisibleBladeFeatureName = "NinjaTrickInvisibleBladeFeature.Name";
        private static readonly string InvisibleBladeFeatureDescription = "NinjaTrickInvisibleBladeFeature.Description";
        static public BlueprintFeature NinjaTrickInvisibleBladeFeature;

        public static void ConfigureInvisibleBlade()
        {
            var InvisibleBladeIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "InvisibleBlade.png");

            var InvisibleBladeFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "NinjaTrickInvisibleBladeFeature", bp => {
                bp.SetName(LoSContext, InvisibleBladeFeatureName);
                bp.SetDescription(LoSContext, InvisibleBladeFeatureName);
                bp.m_Icon = InvisibleBladeIcon;
            });

            VanishingTrick.ConfigureVanishingTrick();

        }


    }
}

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

namespace LegacyOfShadows.NewContent.Features
{
    internal class NoTrace
    {
        private static readonly string NoTraceFeatureName = "NinjaNoTraceFeature";
        private static readonly string NoTraceDescription = "NinjaNoTraceFeature.Description";



        public static void ConfigureNoTrace() 
        {
            var NoTraceIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("97a6aa2b64dd21a4fac67658a91067d7").Icon; // No Trace uses Fast Stealth icon

            var NoTraceFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "NinjaNoTraceFeature", bp => {
                bp.SetName(LoSContext, NoTraceFeatureName);
                bp.SetDescription(LoSContext, NoTraceDescription);
                bp.m_Icon = NoTraceIcon;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SkillStealth;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 6;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                });
            });


        }

    }

}

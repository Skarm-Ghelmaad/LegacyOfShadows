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

namespace LegacyOfShadows.NewContent.Features
{
    // This feature required the creation of the "AddContextFlankingAttackBonus" component.

    internal class Dispatchment
    {
        private static readonly string DispatchmentFeatureName = "NinjaDispatchmentFeature.Name";
        private static readonly string DispatchmentDescription = "NinjaDispatchmentFeature.Description";

        private static BlueprintCharacterClassReference[] RogueClassArray()
        {
            return new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
        }



        public static void ConfigureDispatchment() 
        {



            var DispatchmentIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11").Icon; // Dispatchment uses Outflank icon

            var DispatchmentFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "NinjaDispatchmentFeature", bp => {
                bp.SetName(LoSContext, DispatchmentFeatureName);
                bp.SetDescription(LoSContext, DispatchmentDescription);
                bp.m_Icon = DispatchmentIcon;
                bp.AddComponent<AddContexFlankingAttackBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() { ValueType = ContextValueType.Rank };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_Class = RogueClassArray();
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.StartPlusDivStep;
                    c.m_StartLevel = -1;
                    c.m_StepLevel = 5;
                });
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;

            });

            LoSContext.Logger.LogPatch("Created Dispatchment.", DispatchmentFeature);


        }

    }

}

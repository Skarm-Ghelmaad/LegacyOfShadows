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
using Kingmaker.Blueprints.Classes.Selection;
using HlEX = LegacyOfShadows.Utilities.HelpersExtension;
using static TabletopTweaks.Core.Utilities.FeatTools;

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class StyleMaster
    {

        private static readonly string StyleMasterFeatureSelectionName = "NinjaStyleMasterFeatureSelection.Name";
        private static readonly string StyleMasterFeatureSelectionDescription = "NinjaStyleMasterFeatureSelection.Description";
        static public BlueprintFeature NinjaStyleMasterFeatureSelection;

        public static void ConfigureStyleMaster()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var Style_Master_Feature_Selection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(LoSContext, "NinjaStyleMasterFeatureSelection", bp => {
                bp.SetName(LoSContext, StyleMasterFeatureSelectionName);
                bp.SetDescription(LoSContext, StyleMasterFeatureSelectionDescription);
                bp.m_Icon = null;
                bp.CreatePrerequisiteNoFeature(NinjaStyleMasterFeatureSelection);

                var current_style_feats = FeatToolsExtension.GetStyleFeats();

                foreach (var current_style_feat in current_style_feats)
                {
                    bp.AddFeatures(current_style_feat);
                }



            });


        }




    }
}

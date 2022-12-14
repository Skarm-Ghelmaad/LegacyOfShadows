using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static TabletopTweaks.Core.Utilities.FeatTools;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using LegacyOfShadows.NewContent.Archetypes;
using HarmonyLib;

namespace LegacyOfShadows.Utilities
{

    // Since I have ported coverters for specific class abilities, I have also added variants of Vek's methods to add them to selections.

    public static class FeatToolsExtension
    {
        public static void AddAsMonkKiPower(BlueprintFeature feature)
        {
            var MonkKiPowerSelections = new BlueprintFeatureSelection[] { Selections.MonkKiPowerSelection };
            MonkKiPowerSelections.ForEach(selection => selection.AddFeatures(feature));


        }

        public static void AddAsScaledFistKiPower(BlueprintFeature feature)
        {
            var ScaledFistKiPowerSelections = new BlueprintFeatureSelection[] { Selections.ScaledFistKiPowerSelection };
            ScaledFistKiPowerSelections.ForEach(selection => selection.AddFeatures(feature));


        }

        public static void AddAsStyleStrike(BlueprintFeature feature)
        {
            var StyleStrikeSelections = new BlueprintFeatureSelection[] { Selections.MonkStyleStrike };
            StyleStrikeSelections.ForEach(selection => selection.AddFeatures(feature));


        }

        public static void AddAsNinjaTrick(BlueprintFeature feature, bool advanced_trick = false)
        {
            var ninja_tricks = BlueprintTools.GetModBlueprintReference<BlueprintFeatureSelectionReference>(LoSContext, "NinjaTrickSelection");
            var advanced_talents = BlueprintTools.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");
            var NinjaTrickSelections = new BlueprintFeatureSelection[] { ninja_tricks };
            feature.Groups = feature.Groups.AddToArray(FeatureGroup.RogueTalent);
            NinjaTrickSelections.ForEach(selection => selection.AddFeatures(feature));

            if (advanced_trick)
            {
                feature.AddPrerequisiteFeature(advanced_talents);

            }


        }

        public static void AddAsWildTalent(BlueprintFeature feature)
        {
            var WildTalentSelections = new BlueprintFeatureSelection[] { Selections.WildTalentSelection };
            WildTalentSelections.ForEach(selection => selection.AddFeatures(feature));


        }


        public static BlueprintFeature[] GetStyleFeats()
        {
            return FeatTools.Selections.BasicFeatSelection.AllFeatures
                    .Select(reference => reference)
                    .Where(feature => feature.GetComponent<FeatureTagsComponent>().FeatureTags.HasFlag(FeatureTag.Style))
                    .ToArray();
        }










    }
}

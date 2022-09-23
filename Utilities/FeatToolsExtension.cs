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




















    }
}

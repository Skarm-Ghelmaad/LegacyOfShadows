using HarmonyLib;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using static TabletopTweaks.Core.Utilities.BloodlineTools;
using static TabletopTweaks.Core.Utilities.ClassTools;

namespace LegacyOfShadows.NewContent
{
    [HarmonyPatch(typeof(BlueprintsCache), "Init")]
    static class BlueprintsCache_Init_Patch
    {
        static bool Initialized;

        [HarmonyPriority(Priority.First)]
        [HarmonyPostfix]
        static void CreateNewBlueprints()
        {
            var test = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c773973cd73d4cd7aa4ccf3868dfeba9");
            test.TemporaryContext(bp => {
                bp.SetComponents();
                LoSContext.Logger.LogPatch(bp);
            });
            if (Initialized) return;
            Initialized = true;
            LoSContext.Logger.LogHeader("Loading New Content");

            //New archetypes
            Archetypes.Ninja.ConfigureNinjaArchetype(); 

        }

    }

}

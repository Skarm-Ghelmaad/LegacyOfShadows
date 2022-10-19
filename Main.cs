//using LegacyOfShadows.Feats;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using System;
using System.Linq;
using UnityModManagerNet;
using TabletopTweaks.Core.Utilities;
using LegacyOfShadows.ModLogic;


namespace LegacyOfShadows
{
  static class Main
  {

    public static bool IsInGame => Game.Instance.Player?.Party.Any() ?? false;
    public static bool Enabled;
    public static ModContextLegacyOfShadows LoSContext;
    private static readonly LogWrapper Logger = LogWrapper.Get("LegacyOfShadows");

    static bool Load(UnityModManager.ModEntry modEntry)
    {
      try
      {
        Enabled = true;

        var harmony = new Harmony(modEntry.Info.Id);
        LoSContext = new ModContextLegacyOfShadows(modEntry);
        LoSContext.ModEntry.OnSaveGUI = OnSaveGUI;
        LoSContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;

#if DEBUG
        LoSContext.Debug = true;
        LoSContext.Blueprints.Debug = true;
#endif
        harmony.PatchAll();
        Logger.Info("Finished patching.");

        PostPatchInitializer.Initialize(LoSContext);
        return true;
        
       }

            catch (Exception e)
            {
                Main.LoSContext.Logger.LogError(e, e.Message);
                return false;
            }
        }

       static void OnSaveGUI(UnityModManager.ModEntry modEntry)
            {
                    LoSContext.SaveAllSettings();
            }



    }
}


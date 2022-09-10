using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using LegacyOfShadows.Config;
using UnityModManagerNet;

namespace LegacyOfShadows.ModLogic
{
    class ModContextLegacyOfShadows : ModContextBase
    {
        public Config.NewContent NewContent;

        public ModContextLegacyOfShadows(UnityModManager.ModEntry modEntry) : base(modEntry)
        {
#if DEBUG   
            Debug = true;
#endif
            LoadAllSettings();

        }

        public override void LoadAllSettings()
        {
            LoadBlueprints("LegacyOfShadows.Config", this);
            LoadSettings("NewContent.json", "LegacyOfShadows.Config", ref NewContent);

            LoadLocalization("LegacyOfShadows.Localization");

        }

        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                Blueprints.RemoveUnused();
                SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
        public override void SaveAllSettings()
        {
            base.SaveAllSettings();
            SaveSettings("NewContent.json", NewContent);
        }
    }
}

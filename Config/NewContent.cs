using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Config;

namespace LegacyOfShadows.Config
{
    public class NewContent : IUpdatableSettings
    {
        public bool NewSettingsOffByDefault = false;

        public SettingGroup Archetypes;
        public SettingGroup Feats;
        public SettingGroup NinjaTricks;
        public SettingGroup RogueTalents;
        public SettingGroup SlayerTalents;
        public SettingGroup WildTalents;
        public SettingGroup Items;
        public SettingGroup Spells;
        public void Init()
        {


        }

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as NewContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            Archetypes.LoadSettingGroup(loadedSettings.Archetypes, NewSettingsOffByDefault);
            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            Items.LoadSettingGroup(loadedSettings.Items, NewSettingsOffByDefault);
            NinjaTricks.LoadSettingGroup(loadedSettings.NinjaTricks, NewSettingsOffByDefault);
            RogueTalents.LoadSettingGroup(loadedSettings.RogueTalents, NewSettingsOffByDefault);
            SlayerTalents.LoadSettingGroup(loadedSettings.SlayerTalents, NewSettingsOffByDefault);
            WildTalents.LoadSettingGroup(loadedSettings.WildTalents, NewSettingsOffByDefault);
            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);


        }
    }
}

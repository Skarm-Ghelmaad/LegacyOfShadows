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
        public SettingGroup Spells;
        public SettingGroup Feats;
        public SettingGroup Items;
        public SettingGroup NinjaTricks;
        public SettingGroup RogueTalents;
        public SettingGroup SlayerTalents;
        public SettingGroup WildTalents;
        public void Init()
        {


        }

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as NewContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);
            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            Items.LoadSettingGroup(loadedSettings.Items, NewSettingsOffByDefault);
            NinjaTricks.LoadSettingGroup(loadedSettings.NinjaTricks, NewSettingsOffByDefault);
            RogueTalents.LoadSettingGroup(loadedSettings.RogueTalents, NewSettingsOffByDefault);
            SlayerTalents.LoadSettingGroup(loadedSettings.SlayerTalents, NewSettingsOffByDefault);
            WildTalents.LoadSettingGroup(loadedSettings.WildTalents, NewSettingsOffByDefault);


        }
    }
}

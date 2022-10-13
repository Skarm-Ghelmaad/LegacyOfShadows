using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Diagnostics.Tracing;
using Kingmaker.EntitySystem.Stats;
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
using HlEX = LegacyOfShadows.Utilities.HelpersExtension;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Base.NewContent.Features;
using System.Security.AccessControl;


namespace LegacyOfShadows.MechanicsChanges
{
    public class MartialArtsTraining
    {
        // This is an attempt to allow for the acquisition (and stacking) of the Monk's Unarmed Strike feature.




        public static void ConfigureMonkMartialArtsTraining()
        {
            ConfigureMartialArtsTrainingFakeLevelFeatures();




        }

        public static void ConfigureMartialArtsTrainingFakeLevelFeatures()
        {

            var MartialArtsTrainingAzureIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingAzure.png");
            var MartialArtsTrainingBlackIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingBlack.png");
            var MartialArtsTrainingBlueIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingBlue.png");
            var MartialArtsTrainingBrownIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingBrown");
            var MartialArtsTrainingDarkGreenIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingDarkGreen");
            var MartialArtsTrainingGrayIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingGray.png");
            var MartialArtsTrainingLightGreenIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingLightGreen.png");
            var MartialArtsTrainingOrangeIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingOrange.png");
            var MartialArtsTrainingPurpleIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingPurple.png");
            var MartialArtsTrainingRedIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingRed.png");
            var MartialArtsTrainingYellowIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingYellow.png");


            //var MartialArtsTrainingAlchemistFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingAlchemistFeature", bp => {
            //    bp.SetName(LoSContext, MartialArtsTrainingAlchemistFeatureName);
            //    bp.SetDescription(LoSContext, MartialArtsTrainingAlchemistFeatureDescription);
            //    bp.m_Icon = MartialArtsTrainingAzureIcon;
            //});











        }


    }
}

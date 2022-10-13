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

        private static readonly string MartialArtsTrainingAlchemistFeatureName = "MartialArtsTrainingAlchemistFeature.Name";
        private static readonly string MartialArtsTrainingAlchemistFeatureDescription = "MartialArtsTrainingAlchemistFeature.Description";
        private static readonly string MartialArtsTrainingArcanistFeatureName = "MartialArtsTrainingArcanistFeature.Name";
        private static readonly string MartialArtsTrainingArcanistFeatureDescription = "MartialArtsTrainingArcanistFeature.Description";
        private static readonly string MartialArtsTrainingBarbarianFeatureName = "MartialArtsTrainingBarbarianFeature.Name";
        private static readonly string MartialArtsTrainingBarbarianFeatureDescription = "MartialArtsTrainingBarbarianFeature.Description";
        private static readonly string MartialArtsTrainingBardFeatureName = "MartialArtsTrainingBardFeature.Name";
        private static readonly string MartialArtsTrainingBardFeatureDescription = "MartialArtsTrainingBardFeature.Description";
        private static readonly string MartialArtsTrainingBloodragerFeatureName = "MartialArtsTrainingBloodragerFeature.Name";
        private static readonly string MartialArtsTrainingBloodragerFeatureDescription = "MartialArtsTrainingBloodragerFeature.Description";
        private static readonly string MartialArtsTrainingCavalierFeatureName = "MartialArtsTrainingCavalierFeature.Name";
        private static readonly string MartialArtsTrainingCavalierFeatureDescription = "MartialArtsTrainingCavalierFeature.Description";
        private static readonly string MartialArtsTrainingClericFeatureName = "MartialArtsTrainingClericFeature.Name";
        private static readonly string MartialArtsTrainingClericFeatureDescription = "MartialArtsTrainingClericFeature.Description";
        private static readonly string MartialArtsTrainingDruidFeatureName = "MartialArtsTrainingDruidFeature.Name";
        private static readonly string MartialArtsTrainingDruidFeatureDescription = "MartialArtsTrainingDruidFeature.Description";
        private static readonly string MartialArtsTrainingFighterFeatureName = "MartialArtsTrainingFighterFeature.Name";
        private static readonly string MartialArtsTrainingFighterFeatureDescription = "MartialArtsTrainingFighterFeature.Description";
        private static readonly string MartialArtsTrainingHunterFeatureName = "MartialArtsTrainingHunterFeature.Name";
        private static readonly string MartialArtsTrainingHunterFeatureDescription = "MartialArtsTrainingHunterFeature.Description";
        private static readonly string MartialArtsTrainingInquisitorFeatureName = "MartialArtsTrainingInquisitorFeature.Name";
        private static readonly string MartialArtsTrainingInquisitorFeatureDescription = "MartialArtsTrainingInquisitorFeature.Description";
        private static readonly string MartialArtsTrainingKineticistFeatureName = "MartialArtsTrainingKineticistFeature.Name";
        private static readonly string MartialArtsTrainingKineticistFeatureDescription = "MartialArtsTrainingKineticistFeature.Description";
        private static readonly string MartialArtsTrainingMagusFeatureName = "MartialArtsTrainingMagusFeature.Name";
        private static readonly string MartialArtsTrainingMagusFeatureDescription = "MartialArtsTrainingMagusFeature.Description";
        private static readonly string MartialArtsTrainingMonkFeatureName = "MartialArtsTrainingMonkFeature.Name";
        private static readonly string MartialArtsTrainingMonkFeatureDescription = "MartialArtsTrainingMonkFeature.Description";
        private static readonly string MartialArtsTrainingOracleFeatureName = "MartialArtsTrainingOracleFeature.Name";
        private static readonly string MartialArtsTrainingOracleFeatureDescription = "MartialArtsTrainingOracleFeature.Description";
        private static readonly string MartialArtsTrainingPaladinFeatureName = "MartialArtsTrainingPaladinFeature.Name";
        private static readonly string MartialArtsTrainingPaladinFeatureDescription = "MartialArtsTrainingPaladinFeature.Description";
        private static readonly string MartialArtsTrainingRangerFeatureName = "MartialArtsTrainingRangerFeature.Name";
        private static readonly string MartialArtsTrainingRangerFeatureDescription = "MartialArtsTrainingRangerFeature.Description";
        private static readonly string MartialArtsTrainingRogueFeatureName = "MartialArtsTrainingRogueFeature.Name";
        private static readonly string MartialArtsTrainingRogueFeatureDescription = "MartialArtsTrainingRogueFeature.Description";
        private static readonly string MartialArtsTrainingShamanFeatureName = "MartialArtsTrainingShamanFeature.Name";
        private static readonly string MartialArtsTrainingShamanFeatureDescription = "MartialArtsTrainingShamanFeature.Description";
        private static readonly string MartialArtsTrainingSkaldFeatureName = "MartialArtsTrainingSkaldFeature.Name";
        private static readonly string MartialArtsTrainingSkaldFeatureDescription = "MartialArtsTrainingSkaldFeature.Description";
        private static readonly string MartialArtsTrainingSlayerFeatureName = "MartialArtsTrainingSlayerFeature.Name";
        private static readonly string MartialArtsTrainingSlayerFeatureDescription = "MartialArtsTrainingSlayerFeature.Description";
        private static readonly string MartialArtsTrainingSorcererFeatureName = "MartialArtsTrainingSorcererFeature.Name";
        private static readonly string MartialArtsTrainingSorcererFeatureDescription = "MartialArtsTrainingSorcererFeature.Description";
        private static readonly string MartialArtsTrainingWarpriestFeatureName = "MartialArtsTrainingWarpriestFeature.Name";
        private static readonly string MartialArtsTrainingWarpriestFeatureDescription = "MartialArtsTrainingWarpriestFeature.Description";
        private static readonly string MartialArtsTrainingWitchFeatureName = "MartialArtsTrainingWitchFeature.Name";
        private static readonly string MartialArtsTrainingWitchFeatureDescription = "MartialArtsTrainingWitchFeature.Description";
        private static readonly string MartialArtsTrainingWizardFeatureName = "MartialArtsTrainingWizardFeature.Name";
        private static readonly string MartialArtsTrainingWizardFeatureDescription = "MartialArtsTrainingWizardFeature.Description";
        private static readonly string MartialArtsTrainingAssassinFeatureName = "MartialArtsTrainingAssassinFeature.Name";
        private static readonly string MartialArtsTrainingAssassinFeatureDescription = "MartialArtsTrainingAssassinFeature.Description";
        private static readonly string MartialArtsTrainingArcaneTricksterFeatureName = "MartialArtsTrainingArcaneTricksterFeature.Name";
        private static readonly string MartialArtsTrainingArcaneTricksterFeatureDescription = "MartialArtsTrainingArcaneTricksterFeature.Description";
        private static readonly string MartialArtsTrainingAldoriSwordlordFeatureName = "MartialArtsTrainingAldoriSwordlordFeature.Name";
        private static readonly string MartialArtsTrainingAldoriSwordlordFeatureDescription = "MartialArtsTrainingAldoriSwordlordFeature.Description";
        private static readonly string MartialArtsTrainingDuelistFeatureName = "MartialArtsTrainingDuelistFeature.Name";
        private static readonly string MartialArtsTrainingDuelistFeatureDescription = "MartialArtsTrainingDuelistFeature.Description";
        private static readonly string MartialArtsTrainingDragonDiscipleFeatureName = "MartialArtsTrainingDragonDiscipleFeature.Name";
        private static readonly string MartialArtsTrainingDragonDiscipleFeatureDescription = "MartialArtsTrainingDragonDiscipleFeature.Description";
        private static readonly string MartialArtsTrainingEldritchKnightFeatureName = "MartialArtsTrainingEldritchKnightFeature.Name";
        private static readonly string MartialArtsTrainingEldritchKnightFeatureDescription = "MartialArtsTrainingEldritchKnightFeature.Description";
        private static readonly string MartialArtsTrainingHellknightFeatureName = "MartialArtsTrainingHellknightFeature.Name";
        private static readonly string MartialArtsTrainingHellknightFeatureDescription = "MartialArtsTrainingHellknightFeature.Description";
        private static readonly string MartialArtsTrainingHellknightSigniferFeatureName = "MartialArtsTrainingHellknightSigniferFeature.Name";
        private static readonly string MartialArtsTrainingHellknightSigniferFeatureDescription = "MartialArtsTrainingHellknightSigniferFeature.Description";
        private static readonly string MartialArtsTrainingLoremasterFeatureName = "MartialArtsTrainingLoremasterFeature.Name";
        private static readonly string MartialArtsTrainingLoremasterFeatureDescription = "MartialArtsTrainingLoremasterFeature.Description";
        private static readonly string MartialArtsTrainingMysticTheurgeFeatureName = "MartialArtsTrainingMysticTheurgeFeature.Name";
        private static readonly string MartialArtsTrainingMysticTheurgeFeatureDescription = "MartialArtsTrainingMysticTheurgeFeature.Description";
        private static readonly string MartialArtsTrainingStalwartDefenderFeatureName = "MartialArtsTrainingStalwartDefenderFeature.Name";
        private static readonly string MartialArtsTrainingStalwartDefenderFeatureDescription = "MartialArtsTrainingStalwartDefenderFeature.Description";
        private static readonly string MartialArtsTrainingWinterWitchFeatureName = "MartialArtsTrainingWinterWitchFeature.Name";
        private static readonly string MartialArtsTrainingWinterWitchFeatureDescription = "MartialArtsTrainingWinterWitchFeature.Description";
        private static readonly string AdvancedMartialArtsTrainingFakeLevelFeatureName = "AdvancedMartialArtsTrainingFakeLevelFeature.Name";
        private static readonly string AdvancedMartialArtsTrainingFakeLevelFeatureDescription = "AdvancedMartialArtsTrainingFakeLevelFeature.Description";
        static public BlueprintUnitFactReference MartialArtsTrainingAlchemistFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingArcanistFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingBarbarianFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingBardFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingBloodragerFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingCavalierFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingClericFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingDruidFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingFighterFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingHunterFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingInquisitorFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingKineticistFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingMagusFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingMonkFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingOracleFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingPaladinFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingRangerFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingRogueFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingShamanFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingSkaldFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingSlayerFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingSorcererFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingWarpriestFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingWitchFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingWizardFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingAssassinFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingArcaneTricksterFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingAldoriSwordlordFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingDuelistFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingDragonDiscipleFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingEldritchKnightFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingHellknightFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingHellknightSigniferFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingLoremasterFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingMysticTheurgeFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingStalwartDefenderFeature;
        static public BlueprintUnitFactReference MartialArtsTrainingWinterWitchFeature;
        static public BlueprintUnitFactReference AdvancedMartialArtsTrainingFakeLevelFeature;


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


            var MartialArtsTrainingAlchemistFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingAlchemistFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingAlchemistFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingAlchemistFeatureDescription);
                bp.m_Icon = MartialArtsTrainingAzureIcon;
            });

            var MartialArtsTrainingArcanistFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingArcanistFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingArcanistFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingArcanistFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlueIcon;
            });

            var MartialArtsTrainingBarbarianFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingBarbarianFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingBarbarianFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingBarbarianFeatureDescription);
                bp.m_Icon = MartialArtsTrainingRedIcon;
            });

            var MartialArtsTrainingBardFakeLeveEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingBardFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingBardFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingBardFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingBloodragerFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingBloodragerFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingBloodragerFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingBloodragerFeatureDescription);
                bp.m_Icon = MartialArtsTrainingRedIcon;
            });

            var MartialArtsTrainingCavalierFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingCavalierFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingCavalierFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingCavalierFeatureDescription);
                bp.m_Icon = MartialArtsTrainingGrayIcon;
            });

            var MartialArtsTrainingClericFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingClericFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingClericFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingClericFeatureDescription);
                bp.m_Icon = MartialArtsTrainingYellowIcon;
            });

            var MartialArtsTrainingDruidFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingDruidFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingDruidFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingDruidFeatureDescription);
                bp.m_Icon = MartialArtsTrainingDarkGreenIcon;
            });

            var MartialArtsTrainingFighterFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingFighterFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingFighterFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingFighterFeatureDescription);
                bp.m_Icon = MartialArtsTrainingGrayIcon;
            });

            var MartialArtsTrainingHunterFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingHunterFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingHunterFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingHunterFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBrownIcon;
            });

            var MartialArtsTrainingInquisitorFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingInquisitorFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingInquisitorFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingInquisitorFeatureDescription);
                bp.m_Icon = MartialArtsTrainingYellowIcon;
            });

            var MartialArtsTrainingKineticistFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingKineticistFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingKineticistFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingKineticistFeatureDescription);
                bp.m_Icon = MartialArtsTrainingAzureIcon;
            });

            var MartialArtsTrainingMagusFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingMagusFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingMagusFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingMagusFeatureDescription);
                bp.m_Icon = MartialArtsTrainingAzureIcon;
            });

            var MartialArtsTrainingMonkFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingMonkFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingMonkFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingMonkFeatureDescription);
                bp.m_Icon = MartialArtsTrainingOrangeIcon;
            });

            var MartialArtsTrainingOracleFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingOracleFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingOracleFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingOracleFeatureDescription);
                bp.m_Icon = MartialArtsTrainingLightGreenIcon;
            });

            var MartialArtsTrainingPaladinFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingPaladinFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingPaladinFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingPaladinFeatureDescription);
                bp.m_Icon = MartialArtsTrainingYellowIcon;
            });

            var MartialArtsTrainingRangerFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingRangerFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingRangerFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingRangerFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBrownIcon;
            });

            var MartialArtsTrainingRogueFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingRogueFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingRogueFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingRogueFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
            });

            var MartialArtsTrainingShamanFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingShamanFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingShamanFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingShamanFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingSkaldFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingSkaldFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingSkaldFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingSkaldFeatureDescription);
                bp.m_Icon = MartialArtsTrainingRedIcon;
            });

            var MartialArtsTrainingSlayerFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingSlayerFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingSlayerFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingSlayerFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
            });

            var MartialArtsTrainingSorcererFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingSorcererFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingSorcererFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingSorcererFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingWarpriestFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingWarpriestFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingWarpriestFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingWarpriestFeatureDescription);
                bp.m_Icon = MartialArtsTrainingRedIcon;
            });

            var MartialArtsTrainingWitchFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingWitchFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingWitchFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingWitchFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingWizardFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingWizardFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingWizardFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingWizardFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlueIcon;
            });

            var MartialArtsTrainingAssassinFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingAssassinFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingAssassinFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingAssassinFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
            });

            var MartialArtsTrainingArcaneTricksterFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingArcaneTricksterFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingArcaneTricksterFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingArcaneTricksterFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingAldoriSwordlordFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingAldoriSwordlordFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingAldoriSwordlordFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingAldoriSwordlordFeatureDescription);
                bp.m_Icon = MartialArtsTrainingGrayIcon;
            });

            var MartialArtsTrainingDuelistFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingDuelistFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingDuelistFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingDuelistFeatureDescription);
                bp.m_Icon = MartialArtsTrainingGrayIcon;
            });

            var MartialArtsTrainingDragonDiscipleFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingDragonDiscipleFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingDragonDiscipleFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingDragonDiscipleFeatureDescription);
                bp.m_Icon = MartialArtsTrainingRedIcon;
            });

            var MartialArtsTrainingEldritchKnightFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingEldritchKnightFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingEldritchKnightFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingEldritchKnightFeatureDescription);
                bp.m_Icon = MartialArtsTrainingAzureIcon;
            });

            var MartialArtsTrainingHellknightFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingHellknightFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingHellknightFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingHellknightFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
            });

            var MartialArtsTrainingHellknightSigniferFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingHellknightSigniferFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingHellknightSigniferFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingHellknightSigniferFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
            });

            var MartialArtsTrainingLoremasterFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingLoremasterFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingLoremasterFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingLoremasterFeatureDescription);
                bp.m_Icon = MartialArtsTrainingPurpleIcon;
            });

            var MartialArtsTrainingMysticTheurgeFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingMysticTheurgeFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingMysticTheurgeFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingMysticTheurgeFeatureDescription);
                bp.m_Icon = MartialArtsTrainingYellowIcon;
            });

            var MartialArtsTrainingStalwartDefenderFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingStalwartDefenderFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingStalwartDefenderFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingStalwartDefenderFeatureDescription);
                bp.m_Icon = MartialArtsTrainingGrayIcon;
            });

            var MartialArtsTrainingWinterWitchFakeLevelEnablingFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingWinterWitchFeature", bp => {
                bp.SetName(LoSContext, MartialArtsTrainingWinterWitchFeatureName);
                bp.SetDescription(LoSContext, MartialArtsTrainingWinterWitchFeatureDescription);
                bp.m_Icon = MartialArtsTrainingAzureIcon;
            });

            var AdvancedMartialArtsTrainingFakeLevelFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "AdvancedMartialArtsTrainingFakeLevelFeature", bp => {
                bp.SetName(LoSContext, AdvancedMartialArtsTrainingFakeLevelFeatureName);
                bp.SetDescription(LoSContext, AdvancedMartialArtsTrainingFakeLevelFeatureDescription);
                bp.m_Icon = MartialArtsTrainingOrangeIcon;
                bp.Ranks = 3;
            });









        }


    }
}

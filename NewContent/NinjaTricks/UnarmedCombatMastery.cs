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
using HlEX = LegacyOfShadows.Utilities.HelpersExtension;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem;
using static RootMotion.FinalIK.RagdollUtility;
using System;
using static RootMotion.FinalIK.InteractionTrigger;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using LegacyOfShadows.MechanicsChanges;
using static LegacyOfShadows.MechanicsChanges.MartialArtsTraining;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Base.NewContent.Features;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class UnarmedCombatMastery
    {
        private static readonly string UnarmedCombatMasteryNinjaFeatureName = "NinjaTrickUnarmedCombatMasteryNinjaFeature.Name";
        private static readonly string UnarmedCombatMasteryNinjaFeatureDescription = "NinjaTrickUnarmedCombatMasteryNinjaFeature.Description";
        static public BlueprintFeature NinjaTrickUnarmedCombatMasteryNinjaFeature;
        static public BlueprintFeature NinjaTrickUnarmedCombatMasteryRogueFeature;

        public static void ConfigureUnarmedCombatMastery()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
            var monk_1d6_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("c3fbeb2ffebaaa64aa38ce7a0bb18fb0");
            var improved_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");

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
            var UnarmedCombatMasteryIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "UnarmedCombatMastery.png");

            MartialArtsTraining.ConfigureMonkMartialArtsTraining();

            var RogueMartialArtsTrainingProgression = Helpers.CreateBlueprint<BlueprintProgression>(LoSContext, "RogueMartialArtsTrainingProgression", bp => {
                bp.SetName(LoSContext, "Rogue Martial Training");
                bp.SetDescription(LoSContext, "The character treats his rogue level -4 as monk level for the purposes of unarmed strike damage.");
                bp.m_Icon = MartialArtsTrainingBlackIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry
                    {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            MartialArtsTrainingFakeLevel.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddClass(ClassTools.ClassReferences.RogueClass);
                bp.m_Classes[0].AdditionalLevel = -4;
            });

            var UnarmedCombatMasteryNinjaFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(LoSContext, "NinjaTrickUnarmedCombatMasteryNinjaFeature", bp => {
                bp.SetName(LoSContext, UnarmedCombatMasteryNinjaFeatureName);
                bp.SetDescription(LoSContext, UnarmedCombatMasteryNinjaFeatureDescription);
                bp.m_Icon = MartialArtsTrainingBlackIcon;
                bp.AddComponent(HlEX.CreateHasFactFeatureUnlock(monk_1d6_unarmed_strike.ToReference<BlueprintUnitFactReference>(), UniversalUnarmedStrike.ToReference<BlueprintUnitFactReference>(), true));
                bp.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = RogueMartialArtsTrainingProgression.ToReference<BlueprintFeatureReference>();
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                     c.m_Feature = improved_unarmed_strike.ToReference<BlueprintFeatureReference>();
                     c.Group = Prerequisite.GroupType.All;
                 }));
            });

            var UnarmedCombatMasteryRogueFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(LoSContext, "NinjaTrickUnarmedCombatMasteryRogueFeature", bp => {
                bp.SetName(LoSContext, "Unarmed Combat Mastery");
                bp.SetDescription(LoSContext, "A rogue who selects this trick deals damage with her unarmed strikes as if she were a monk of her rogue level –4. If the rogue has levels in monk (or other similar features), this ability stacks with monk levels to determine how much damage she can do with her unarmed strikes. A rogue must have the Improved Unarmed Strike feat before taking this trick.");
                bp.m_Icon = UnarmedCombatMasteryIcon;
                bp.AddComponent(HlEX.CreateHasFactFeatureUnlock(monk_1d6_unarmed_strike.ToReference<BlueprintUnitFactReference>(), UniversalUnarmedStrike.ToReference<BlueprintUnitFactReference>(), true));
                bp.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = RogueMartialArtsTrainingProgression.ToReference<BlueprintFeatureReference>();
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = improved_unarmed_strike.ToReference<BlueprintFeatureReference>();
                    c.Group = Prerequisite.GroupType.All;
                }));
            });


        }

    }
}

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
using LegacyOfShadows.NewComponents.OwlcatReplacements;
using Kingmaker.RuleSystem;

namespace LegacyOfShadows.MechanicsChanges
{
    //internal class PoisonCraftTraining
    //{
    //    // This is an attempt to allow for the acquisition (and stacking) of the Assassin's Poison Use feature.

    //    public static void ConfigurePoisonCraftTrainingFakeLevelFeature()
    //    {

    //        var PoisonCraftTrainingIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "PoisonCraftTraining.png");

    //        var PoisonCraftTrainingFakeLevel = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "PoisonCraftTrainingFakeLevel", bp => {
    //            bp.IsClassFeature = true;
    //            bp.HideInUI = true;
    //            bp.Ranks = 40;
    //            bp.HideInCharacterSheetAndLevelUp = true;
    //            bp.SetName(LoSContext, "Poisoncraft Training Fake Level");
    //            bp.SetDescription(LoSContext, "This feature grant fake assassin levels that are considered to calculate his assassin level for purpose of the Poison Use feature.");
    //            bp.m_Icon = PoisonCraftTrainingIcon;
    //        });


    //    }
    //}
}

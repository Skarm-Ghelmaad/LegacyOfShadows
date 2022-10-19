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
using System.Drawing;
using Kingmaker.UnitLogic.Buffs;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;


namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class SeeTheUnseen
    {
        private static readonly string SeeTheUnseenFeatureName = "NinjaTrickSeeTheUnseenFeature.Name";
        private static readonly string SeeTheUnseenFeatureDescription = "NinjaTrickSeeTheUnseenFeature.Description";


        public static void ConfigureSeeTheUnseen()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var SeeTheUnseenIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "SeeTheUnseen.png");

            var See_Invisibility_Spell = BlueprintTools.GetBlueprint<BlueprintAbility>("30e5dc243f937fc4b95d2f8f4e1b7ff3");

            var SeeTheUnseenAbility = HlEX.ConvertSpellToSupernatural(See_Invisibility_Spell, RogueArray, StatType.Charisma, kiResource, "NinjaTrick", "Ability", "", "SeeInvisibility", "", "", "", "SeeTheUnseen", "", "", false, false, null, 1);

            SeeTheUnseenAbility.SetName(LoSContext, SeeTheUnseenFeatureName);
            SeeTheUnseenAbility.SetDescription(LoSContext, SeeTheUnseenFeatureDescription);
            SeeTheUnseenAbility.m_Icon = SeeTheUnseenIcon;
            SeeTheUnseenAbility.ActionType = UnitCommand.CommandType.Swift;

            var see_the_unseen_feature = HlEX.ConvertAbilityToFeature(SeeTheUnseenAbility, "", "", "Feature", "Ability", false);

            LoSContext.Logger.LogPatch("Created See The Unseen ninja trick.", see_the_unseen_feature);

        }


    }
}

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
using System;

namespace LegacyOfShadows.NewContent.NinjaTricks
{
    internal class ShadowClone
    {
        private static readonly string ShadowCloneFeatureName = "NinjaTrickShadowCloneFeature.Name";
        private static readonly string ShadowCloneFeatureDescription = "NinjaTrickShadowCloneFeature.Description";


        public static void ConfigureShadowClone()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var ShadowCloneIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "ShadowClone.png");

            var Mirror_Image_Spell = BlueprintTools.GetBlueprint<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564");

            var ShadowCloneAbility = HlEX.ConvertSpellToSupernatural(Mirror_Image_Spell, RogueArray, StatType.Charisma, kiResource, "NinjaTrick", "Ability", "", "MirrorImage", "", "", "", "ShadowClone", "", "", false, false, null, 1 );
            
            ShadowCloneAbility.SetName(LoSContext, ShadowCloneFeatureName);
            ShadowCloneAbility.SetDescription(LoSContext, ShadowCloneFeatureDescription);
            ShadowCloneAbility.m_Icon = ShadowCloneIcon;

            var shadow_clone_feature = HlEX.ConvertAbilityToFeature(ShadowCloneAbility, "", "", "Feature", "Ability", false);

            LoSContext.Logger.LogPatch("Created Shadow Clone ninja trick.", shadow_clone_feature);


        }


    }
}

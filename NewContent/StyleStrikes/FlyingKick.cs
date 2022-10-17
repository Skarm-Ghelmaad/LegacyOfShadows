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
using System.Configuration;
using LegacyOfShadows.Utilities;


namespace LegacyOfShadows.NewContent.StyleStrikes
{
    internal class FlyingKick
    {

        private static readonly string FlyingKickFeatureName = "MonkFlyingKickFeature.Name";
        private static readonly string FlyingKickDescription = "MonkFlyingKickFeature.Description";



        public static void ConfigureFlyingKick()
        {
            var FlyingKickIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "FlyingKick.png");

            var FlyingKickBuff = Helpers.CreateBlueprint<BlueprintBuff>(LoSContext, "MonkFlyingKickBuff", bp => {
                bp.SetName(LoSContext, "");
                bp.SetDescription(LoSContext, "");
                bp.m_Icon = null;
                bp.AddComponent<AddMechanicsFeature>(c => {
                    c.m_Feature = Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Pounce;
                });
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath | BlueprintBuff.Flags.HiddenInUi;
            });

            var FlyingKickToggle = HelpersExtension.ConvertBuffToActivatableAbility(FlyingKickBuff, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free, false, "ToggleAbility", "Buff");

            FlyingKickToggle.Group = Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.StyleStrike;
            FlyingKickToggle.SetName(LoSContext, FlyingKickFeatureName);
            FlyingKickToggle.SetDescription(LoSContext, FlyingKickDescription);
            FlyingKickToggle.m_Icon = FlyingKickIcon;


            var FlyingKickFeature = HelpersExtension.ConvertActivatableAbilityToFeature(FlyingKickToggle, "", "", "Feature", "ToggleAbility", false);

            FeatToolsExtension.AddAsStyleStrike(FlyingKickFeature);

            LoSContext.Logger.LogPatch("Created Flying Kick style strike.", FlyingKickFeature);

        }



    }
}

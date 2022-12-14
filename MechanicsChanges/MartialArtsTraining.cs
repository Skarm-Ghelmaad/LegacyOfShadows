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
    public class MartialArtsTraining
    {
        // This is an attempt to allow for the acquisition (and stacking) of the Monk's Unarmed Strike feature.

        static public BlueprintFeature MartialArtsTrainingFakeLevel;
        private static readonly string UniversalUnarmedStrikeName = "UniversalUnarmedStrike.Name";
        private static readonly string UniversalUnarmedStrikeDescription = "UniversalUnarmedStrike.Description";
        static public BlueprintUnitProperty MartialArtsTrainingProperty;
        static public BlueprintFeature UniversalUnarmedStrike;


        public static void ConfigureMonkMartialArtsTraining()
        {
            ConfigureMartialArtsTrainingFakeLevelFeature();

            var MartialArtsTrainingOrangeIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "MartialArtsTrainingOrange.png");

            var monk_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");

            var monk_1d6_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("c3fbeb2ffebaaa64aa38ce7a0bb18fb0");
            var monk_1d8_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("8267a0695a4df3f4ca508499e6164b98");
            var monk_1d10_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("f790a36b5d6f85a45a41244f50b947ca");
            var monk_2d6_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("b3889f445dbe42948b8bb1ba02e6d949");
            var monk_2d8_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("078636a2ce835e44394bb49a930da230");
            var monk_2d10_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("df38e56fa8b3f0f469d55f9aa26b3f5c");

            var hammerblow_buff = BlueprintTools.GetBlueprint<BlueprintBuff>("928f89c30f4ad6a48862980e5f6f81cf");

            var monk_other_fists = new BlueprintFeature[] { monk_1d8_unarmed_strike, monk_1d10_unarmed_strike, monk_2d6_unarmed_strike, monk_2d8_unarmed_strike, monk_2d10_unarmed_strike };



            #region |-----------------------------------------------------/ CREATED MARTIAL ARTS TRAINING PROPERTY /-------------------------------------------------------------|

            var MartialArtsTrainingProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(LoSContext, "MartialArtsTrainingProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new FactRankGetter(){
                                m_Fact = MartialArtsTrainingFakeLevel.ToReference<BlueprintUnitFactReference>()
                            }
                        },
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new ClassLevelGetter(){
                                m_Class = ClassTools.ClassReferences.MonkClass
                            }
                        }
                    };
                });
            });

            LoSContext.Logger.LogPatch("Created Martial Arts Training property.", MartialArtsTrainingProperty);

            #endregion


            #region |------------------------------------------------------------/ "CONDENSE" MONK UNARMED STRIKE /-------------------------------------------------------------|

            DiceFormula[] fist_dice_formulas = new DiceFormula[] {
                                                                    new DiceFormula(1, DiceType.D6),
                                                                    new DiceFormula(1, DiceType.D8),
                                                                    new DiceFormula(1, DiceType.D10),
                                                                    new DiceFormula(2, DiceType.D6),
                                                                    new DiceFormula(2, DiceType.D8),
                                                                    new DiceFormula(2, DiceType.D10)
                                                                };
            monk_1d6_unarmed_strike.ComponentsArray = new BlueprintComponent[]
                                                                                {

                                                                                    HlEX.CreateContextWeaponDamageDiceReplacementWeaponCategory(new WeaponCategory[] { WeaponCategory.UnarmedStrike }, fist_dice_formulas, HlEX.CreateContextValue(AbilityRankType.Default)),
                                                                                };

            monk_1d6_unarmed_strike.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = MartialArtsTrainingProperty.ToReference<BlueprintUnitPropertyReference>();
                c.m_Progression = ContextRankProgression.DivStep;
                c.m_StepLevel = 4;
            });

            monk_1d6_unarmed_strike.ReapplyOnLevelUp = true;
            monk_1d6_unarmed_strike.m_Icon = MartialArtsTrainingOrangeIcon;

            monk_progression.LevelEntries = HlEX.RemoveEntries(monk_progression.LevelEntries, f => monk_other_fists.Contains(f), keep_empty_entries: true);

            LoSContext.Logger.LogPatch("Condensed Monk's unarmed strike damage.", monk_1d6_unarmed_strike);

            #endregion


            #region |------------------------------------------------------/ CREATE GENERIC (MONK) UNARMED STRIKE /------------------------------------------------------------|

            var generic_1d6_unarmed_strike = monk_1d6_unarmed_strike.CreateCopy(LoSContext, "UniversalUnarmedStrike", bp => {
                 bp.SetName(LoSContext, UniversalUnarmedStrikeName);
                 bp.SetDescription(LoSContext, UniversalUnarmedStrikeDescription);

             });

            LoSContext.Logger.LogPatch("Creaded generic (monk) unarmed strike.", generic_1d6_unarmed_strike);


            #endregion


            #region |------------------------------------------------------------/ UPDATED HAMMERBLOW STYLE STRIKE /-------------------------------------------------------------|

            var double_damage_dice_vanilla = hammerblow_buff.GetComponent<DoubleDamageDiceOnAttack>();

            hammerblow_buff.ReplaceComponents<DoubleDamageDiceOnAttack>(Helpers.Create<DoubleDamageDiceOnAttackLOS>(c =>
                                                                                                                        {
                                                                                                                            c.CriticalHit = double_damage_dice_vanilla.CriticalHit;
                                                                                                                            c.OnlyOnFullAttack = double_damage_dice_vanilla.OnlyOnFullAttack;
                                                                                                                            c.m_WeaponType = double_damage_dice_vanilla.m_WeaponType;
                                                                                                                            c.OnlyOnFirstAttack = double_damage_dice_vanilla.OnlyOnFirstAttack;

                                                                                                                        }));
            LoSContext.Logger.LogPatch("Patched Hammerblow buff.", hammerblow_buff);

            #endregion



        }

        public static void ConfigureMartialArtsTrainingFakeLevelFeature()
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

            var MartialArtsTrainingFakeLevel = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "MartialArtsTrainingFakeLevel", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 40;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(LoSContext, "Martial Arts Training Fake Level");
                bp.SetDescription(LoSContext, "This feature grant fake monk levels that are considered to calculate his monk level for purpose of the Unarmed Strike feature.");
                bp.m_Icon = MartialArtsTrainingOrangeIcon;
            });

            LoSContext.Logger.LogPatch("Created Martial Arts Training Fake Level.", MartialArtsTrainingFakeLevel);

        }


    }
}

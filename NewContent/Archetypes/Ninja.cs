using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using LegacyOfShadows.NewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using LegacyOfShadows.MechanicsChanges;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Mechanics;
using HlEX = LegacyOfShadows.Utilities.HelpersExtension;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using LegacyOfShadows.Utilities;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using RootMotion.FinalIK;
using static TabletopTweaks.Core.Utilities.FeatTools;
using System.Runtime.ConstrainedExecution;
using Kingmaker.Blueprints.Classes.Prerequisites;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using Kingmaker.UnitLogic.ActivatableAbilities;
using LegacyOfShadows.NewComponents.OwlcatReplacements;
using Kingmaker.UnitLogic.Abilities;
using static Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart;
using Kingmaker.Blueprints.Classes.Selection;
using LegacyOfShadows.NewContent.NinjaTricks;


namespace LegacyOfShadows.New_Content.Archetypes
{
    public class Ninja
    {

        private static readonly string NinjaProficienciesFeatureName = "NinjaProficiencies.Name";
        private static readonly string NinjaProficienciesFeatureDescription = "NinjaProficiencies.Description";
        static public BlueprintAbility NinjaTrickKiExtraAttackAbility;
        static public BlueprintAbility NinjaTrickKiSpeedBoostAbility;
        static public BlueprintAbility InstinctiveStealthAbility;
        static public BlueprintFeatureSelection NinjaTrickSelection;


        static void ConfigureNinjaProficiencies()
        {
            var Rogue_Proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("33e2a7e4ad9daa54eaf808e1483bb43c");
            var Dueling_Sword_Proficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("9c37279588fd9e34e9c4cb234857492c");

            #region | Create Ninja Proficiencies |
            
            var NinjaProficienciesFeature = Rogue_Proficiencies.CreateCopy(LoSContext, "NinjaProficiencies", bp =>
            {
                bp.ReplaceComponents<AddProficiencies>(Helpers.Create<AddProficiencies>(c =>
                {
                    c.WeaponProficiencies = new WeaponCategory[] {
                                                                    WeaponCategory.Kama,
                                                                    WeaponCategory.Nunchaku,
                                                                    WeaponCategory.Sai,
                                                                    WeaponCategory.Shortbow,
                                                                    WeaponCategory.Shortsword,
                                                                    WeaponCategory.Shuriken,
                                                                    WeaponCategory.Scimitar
                                                                };

                }));
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { Dueling_Sword_Proficiency.ToReference<BlueprintUnitFactReference>() };
                });
                bp.SetName(LoSContext, NinjaProficienciesFeatureName);
                bp.SetDescription(LoSContext, NinjaProficienciesFeatureDescription);

            }); 
            #endregion

        }

        static void ConfigureNinjaKiPool()
        {
           MechanicsChanges.KiResourceChanges.ConfigureBasicKiResourceChanges();

           var InstinctiveStealthIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "InstinctiveStealth.png");
           var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
           var Abundant_Ki_Pool = BlueprintTools.GetBlueprint<BlueprintFeature>("e8752f9126d986748b10d0bdac693264");
           var Ki_Extra_Attack_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("cadf8a5c42002494cabfc6c1196b514a");
           var Monk_Ki_Extra_Attack_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("7f6ea312f5dad364fa4a896d7db39fdd");
           var Monk_Ki_Sudden_Speed_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("8c98b8f3ac90fa245afe14116e48c7da");
           var Expeditious_Retreat_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("9ea4ec3dc30cd7940a372a4d699032e7");
           var Shadow_Veil_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("5ceedff361efd4c4eb8e8369c13b03ea");
           var Shadow_Veil_Buff_Fx_Asset_ID = Shadow_Veil_Buff.FxOnStart.AssetId;

            #region | Create Ninja Ki abilities |
            
            var NinjaKiSpeedBuff = Expeditious_Retreat_Buff.CreateCopy(LoSContext, "NinjaTrickKiSpeedBuff", bp =>
            {
                bp.ReplaceComponents<BuffMovementSpeed>(Helpers.Create<BuffMovementSpeed>(c =>
                {
                    c.Value = 20;
                }));

            });

            var Instinctive_Stealth_Buff = Helpers.CreateBlueprint<BlueprintBuff>(LoSContext, "NinjaTrickInstinctiveStealthBuff", bp =>
            {
                bp.SetName(LoSContext, "Instinctive Stealth");
                bp.m_Icon = InstinctiveStealthIcon;
                bp.FxOnStart = HlEX.CreatePrefabLink(Shadow_Veil_Buff_Fx_Asset_ID);
                bp.FxOnRemove = new PrefabLink();
                bp.AddComponent(Helpers.Create<AddStatBonus>(c =>
                {
                    c.Stat = StatType.SkillStealth;
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Value = 4;
                }));
            });

            var Apply_Instinctive_Stealth_Buff = HlEX.CreateContextActionApplyBuff(Instinctive_Stealth_Buff.ToReference<BlueprintBuffReference>(),
                                                                                   HlEX.CreateContextDuration(1, DurationRate.Rounds),
                                                                                   false, false, false, false, false);

            var NinjaExtraAttackAbility = Monk_Ki_Extra_Attack_Ability.CreateCopy(LoSContext, "NinjaTrickKiExtraAttackAbility", bp =>
            {
                bp.SetName(LoSContext, "Ninja Trick: Extra Attack");
                bp.SetDescription(LoSContext, "By spending 1 point from his ki pool as a swift action, a ninja can make one additional attack at his highest attack bonus when making a full attack. This bonus attack stacks with haste and similar effects.");

            });

            var NinjaKiSpeedBoostAbility = Monk_Ki_Sudden_Speed_Ability.CreateCopy(LoSContext, "NinjaTrickKiSpeedBoostAbility", bp =>
            {
                bp.SetName(LoSContext, "Ninja Trick: Speed Burst");
                bp.SetDescription(LoSContext, "A ninja with this ki power can spend 1 point from his ki pool as a swift action to grant himself a sudden burst of speed. This increases the ninja's base land speed by 20 feet for 1 round.");
                bp.FlattenAllActions()
                    .OfType<ContextActionApplyBuff>()
                    .ForEach(a =>
                    {
                        a.m_Buff = NinjaKiSpeedBuff.ToReference<BlueprintBuffReference>();
                        a.DurationValue = HlEX.CreateContextDuration(1, DurationRate.Rounds);
                    });

            });

            var InstinctiveStealthAbility = Helpers.CreateBlueprint<BlueprintAbility>(LoSContext, "NinjaTrickInstinctiveStealthAbility", bp =>
            {
                bp.SetName(LoSContext, "Ninja Trick: Instinctive Stealth");
                bp.SetDescription(LoSContext, "A ninja with this ki power can spend 1 point from his ki pool as a swift action to grant himself an insight on which place and ways are the best to be stealthy in the current circumstances. This grants him a +4 insight bonus on Stealth checks for 1 round.");
                bp.m_Icon = InstinctiveStealthIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(LoSContext, "NinjaTrickInstinctiveStealthAbility.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Instinctive_Stealth_Buff));
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            InstinctiveStealthAbility.SetMiscAbilityParametersSelfOnly();

            var Ninja_Extra_Attack_feature = HlEX.ConvertAbilityToFeature(NinjaExtraAttackAbility, "", "", "Feature", "Ability", false);

            #endregion

            #region | Changes to Ninja Ki Pool |
            
            var KiPool = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "KiPoolNinjaFeature", bp =>
            {
                bp.SetName(LoSContext, "Ki Pool");
                bp.SetDescription(LoSContext, "At 2nd level, a ninja gains a pool of ki points, supernatural energy she can use to accomplish amazing feats. The number of points in the ninja’s ki pool is equal to 1/2 her ninja level + her Charisma modifier.\n"
                                            + "By spending 1 point from her ki pool, a ninja can make one additional attack at her highest attack bonus, but she can do so only when making a full attack. In addition, she can spend 1 point to increase her speed by 20 feet for 1 round.\n"
                                            + "Moreover, she can also spend 1 point to gain a +4 insight bonus on her Stealth checks for 1 round. Each of these powers is activated as a swift action. A ninja can gain additional powers that consume points from her ki pool by selecting certain ninja tricks.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { };
                bp.IsPrerequisiteFor = new List<BlueprintFeatureReference> { Abundant_Ki_Pool.ToReference<BlueprintFeatureReference>() };
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { NinjaKiSpeedBoostAbility.ToReference<BlueprintUnitFactReference>(), InstinctiveStealthAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent(HlEX.CreateAddFeatureOnClassLevel(Ninja_Extra_Attack_feature.ToReference<BlueprintFeatureReference>(), new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass }, 5, true));
            });

            #endregion

            #region | Add Ninja Ki Pool to Abundant Ki Pool feat|

            Abundant_Ki_Pool.AddPrerequisiteFeature(KiPool, GroupType.Any);  // Added Ninja Ki Pool to Abundant Ki Prerequisites. 
            
            #endregion

        }

        static void ConfigureNinjaStyleStrikes()
        {
            var NinjaStyleStrikeIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "NinjaStyleStrike.png");
            var monk_style_strikes = Selections.MonkStyleStrike;

            #region | Create Ninja Style Strikes from Monk Style Strike |
            
            var ninja_style_strikes = monk_style_strikes.CreateCopy(LoSContext, "NinjaStyleStrike", bp =>
            {
                bp.SetDescription(LoSContext, "At 5th level, a ninja can learn one type of style strike, as the monk class feature. Whenever she spends ki from her ki pool to make an additional attack, she can designate that additional attack as a style strike, regardless of the weapon she uses to make the attack. The attack is resolved as normal, but it has a different effect depending upon the type of strike chosen. At 10th level and every 5 levels thereafter, a ninja learns an additional style strike. She must choose which style strike to apply before the attack roll is made. Unlike a monk, a ninja does not gain the ability to designate more than one attack as a style strike per round.");
                bp.m_Features = new BlueprintFeatureReference[0];
            });

            var ninja_style_strikes_old_features = ninja_style_strikes.m_AllFeatures;

            var new_abilities = new List<BlueprintAbility>();

            foreach (var stl_strk in ninja_style_strikes_old_features)
            {
                var stl_strk_toggle = stl_strk.Get().GetComponent<AddFacts>().m_Facts[0].Get() as BlueprintActivatableAbility;
                var stl_strk_buff = stl_strk_toggle.m_Buff.Get().CreateCopy(LoSContext, "Ninja" + stl_strk_toggle.m_Buff.Get().name);

                stl_strk_buff.GetComponent<DoubleDamageDiceOnAttackLOS>().TemporaryContext(c =>
                {
                    c.m_WeaponType = null;
                });
                stl_strk_buff.GetComponent<AddInitiatorAttackWithWeaponTrigger>().TemporaryContext(c =>
                {
                    c.m_WeaponType = null;
                });
                stl_strk_buff.GetComponent<IgnoreDamageReductionOnAttack>().TemporaryContext(c =>
                {
                    c.m_WeaponType = null;
                });

                stl_strk_buff.m_Flags = (BlueprintBuff.Flags)0;
                stl_strk_buff.SetName(LoSContext, stl_strk_toggle.m_DisplayName);
                stl_strk_buff.SetDescription(LoSContext, stl_strk_toggle.m_Description);
                stl_strk_buff.m_Icon = stl_strk_toggle.m_Icon;

                var apply_stl_strk_buff = HlEX.CreateContextActionApplyBuff(stl_strk_buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(1),
                                                                            false, false, false, false, false);
                var stl_strk_ability = NinjaTrickKiExtraAttackAbility.CreateCopy(LoSContext, "Ninja" + stl_strk_toggle.name);

                stl_strk_ability.GetComponent<AbilityEffectRunAction>().Actions.Actions.AppendToArray(apply_stl_strk_buff);

                new_abilities.Add(stl_strk_ability);

                var stl_strk_new_feature = stl_strk.Get().CreateCopy(LoSContext, "Ninja" + stl_strk.Get().name);

                stl_strk_new_feature.ComponentsArray = new BlueprintComponent[0];

                ninja_style_strikes.m_AllFeatures.AppendToArray(stl_strk_new_feature.ToReference<BlueprintFeatureReference>());

                stl_strk_ability.AddComponent(HlEX.CreateAbilityShowIfCasterHasFact(stl_strk_new_feature.ToReference<BlueprintUnitFactReference>()));

                stl_strk_ability.SetName(LoSContext, NinjaTrickKiExtraAttackAbility.m_DisplayName + " (" + stl_strk_toggle.m_DisplayName + ")");

                stl_strk_ability.SetDescription(LoSContext, NinjaTrickKiExtraAttackAbility.m_Description + "\n" + stl_strk_toggle.m_DisplayName + ": " + stl_strk_toggle.m_Description);

                stl_strk_ability.m_Icon = stl_strk_toggle.m_Icon;

            }

            #endregion

            #region | Create Ninja Style Strike wrapper |

            var stl_strk_wrapper = HlEX.CreateVariantWrapper("NinjaStyleStrikesAbilityBase", new_abilities.ToArray());

            stl_strk_wrapper.SetName(LoSContext, NinjaTrickKiExtraAttackAbility.m_DisplayName + " (" + ninja_style_strikes.m_DisplayName + ")");

            stl_strk_wrapper.SetDescription(LoSContext, NinjaTrickKiExtraAttackAbility.m_Description + " (" + ninja_style_strikes.m_DisplayName + ": " + ninja_style_strikes.m_Description + ")");

            stl_strk_wrapper.m_Icon = NinjaStyleStrikeIcon;

            ninja_style_strikes.ComponentsArray = new BlueprintComponent[] { HlEX.CreateAddFacts(stl_strk_wrapper.ToReference<BlueprintUnitFactReference>()) }; 
                
            #endregion


        }

        static void ConfigureNinjaTrick()
        {
            var rogue_talent_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var improved_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");
            var evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
            var NinjaTrickSelectionIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "NinjaTrickSelection.png");


            var ninja_trick = rogue_talent_selection.CreateCopy(LoSContext, "NinjaTrickSelection");

            ninja_trick.SetName(LoSContext, "Ninja Tricks");
            ninja_trick.SetDescription(LoSContext, "As a ninja continues her training, she learns a number of tricks that allow her to confuse her foes and grant her supernatural abilities. Starting at 2nd level, a ninja gains one ninja trick. She gains one additional ninja trick for every 2 levels attained after 2nd. Unless otherwise noted, a ninja cannot select an individual ninja trick more than once.");
            ninja_trick.m_Icon = NinjaTrickSelectionIcon;

            FeatToolsExtension.AddAsNinjaTrick(improved_unarmed_strike, false);

            StyleMaster.ConfigureStyleMaster();
            AccelerationOfForm.ConfigureAccelerationOfForm();
            ShadowClone.ConfigureShadowClone();
            InvisibleBlade.ConfigureInvisibleBlade();       // This also triggers the creation of Vanishing Trick.
            SeeTheUnseen.ConfigureSeeTheUnseen();
            HerbalCompound.ConfigureHerbalCompound();
            Kamikaze.ConfigureKamikaze();
            UnarmedCombatMastery.ConfigureUnarmedCombatMastery();
            FlurryOfStars.ConfigureFlurryOfStars();


            FeatToolsExtension.AddAsNinjaTrick(improved_unarmed_strike, false);
            FeatToolsExtension.AddAsNinjaTrick(StyleMaster.NinjaStyleMasterFeatureSelection, false);
            FeatToolsExtension.AddAsNinjaTrick(AccelerationOfForm.NinjaTrickAccelerationOfFormFeature, false);
            FeatToolsExtension.AddAsNinjaTrick(ShadowClone.NinjaTrickShadowCloneFeature, false);
            FeatToolsExtension.AddAsNinjaTrick(VanishingTrick.NinjaTrickVanishingTrickFeature, false);
            FeatToolsExtension.AddAsNinjaTrick(InvisibleBlade.NinjaTrickInvisibleBladeFeature, true);
            FeatToolsExtension.AddAsNinjaTrick(SeeTheUnseen.NinjaTrickSeeTheUnseenFeature, true);
            FeatToolsExtension.AddAsNinjaTrick(HerbalCompound.NinjaTrickHerbalCompoundFeature, false);
            FeatToolsExtension.AddAsNinjaTrick(Kamikaze.NinjaTrickKamikazeFeature, false);
            FeatToolsExtension.AddAsNinjaTrick(UnarmedCombatMastery.NinjaTrickUnarmedCombatMasteryNinjaFeature, true);
            FeatToolsExtension.AddAsNinjaTrick(evasion, true);
            FeatToolsExtension.AddAsNinjaTrick(FlurryOfStars.NinjaTrickFlurryOfStarsFeature, false);


        }


        //static void ConfigureNinjaPoisonUse()
        //{
        //    var Assassin_Poison_Use = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6");
        //    var Assassin_Poison_Resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d54b614eb42da7d48b927b57de337b95");

        //}







    }
}

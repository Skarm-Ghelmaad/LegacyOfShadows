using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using LegacyOfShadows.Utilities;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.FactLogic;
using LegacyOfShadows.NewComponents;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.NewComponents;

namespace LegacyOfShadows.MechanicsChanges
{

    // This class is used to make changes to the Ki resource to allow for a more universal use for it.
    public class KiResourceChanges
    {

        private static readonly string WisdomKiPoolCanonFeatureName = "WisdomKiPoolCanonFeature.Name";
        private static readonly string CharismaKiPoolCanonFeatureName = "CharismaKiPoolCanonFeature.Name";
        private static readonly string WisdomKiPoolCanonFeatureDescription = "WisdomKiPoolCanonFeature.Description";
        private static readonly string CharismaKiPoolCanonFeatureDescription = "CharismaKiPoolCanonFeature.Description";


        public static void ConfigureBasicKiResourceChanges()
        {
            var kiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e9590244effb4be4f830b1e3fffced13");
            var scaledFistKiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("7d002c1025fbfe2458f1509bf7a89ce1");
            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var scaledFistKiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("ae98ab7bda409ef4bb39149a212d6732");

            var wisdomKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolWisdom.png");
            var charismaKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolCharisma.png");


            #region | Changes to Monk's Ki Power |

            // Alter Ki pool to not automatically add Wis bonus. 

            kiResource.m_MaxAmount.IncreasedByStat = false;
            kiResource.m_Max = 5000;

            #endregion

            #region | Changes to Scaled Fist's Ki Power |

            // Alter Scaled Fist's Ki pool to not automatically add Cha bonus. 

            scaledFistKiResource.m_MaxAmount.IncreasedByStat = false;
            scaledFistKiResource.m_Max = 5000;

            #endregion

            #region | Creation of Wis and Charisma Ki Resouce Bonus Feature |

            // A resource bonus based on Wisdom and Charisma are created for Monk, Scaled Fist and Ninja. 
            // These are introduced for canon, in order to align the basic and the advanced ki resource.
            // Note that these features are hidden in the UI.

            var wisdom_KiPoolCanon = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "WisdomKiPoolCanonFeature", bp => {
                bp.SetName(LoSContext, WisdomKiPoolCanonFeatureName);
                bp.SetDescription(LoSContext, WisdomKiPoolCanonFeatureDescription);
                bp.m_Icon = wisdomKiPoolIcon;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent(Helpers.Create<IncreaseResourceAmountBasedOnStat>(c => {
                    c.m_Resource = kiResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Subtract = false;
                    c.NotUseHighestStat = true;
                    c.ResourceBonusStat = StatType.Wisdom;

                }));
            });

            LoSContext.Logger.LogPatch("Created traditional Ki Pool modifier for Wisdom", wisdom_KiPoolCanon);

            var charisma_KiPoolCanon = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "CharismaKiPoolCanonFeature", bp => {
                bp.SetName(LoSContext, CharismaKiPoolCanonFeatureName);
                bp.SetDescription(LoSContext, CharismaKiPoolCanonFeatureDescription);
                bp.m_Icon = wisdomKiPoolIcon;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent(Helpers.Create<IncreaseResourceAmountBasedOnStat>(c => {
                    c.m_Resource = kiResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Subtract = false;
                    c.NotUseHighestStat = true;
                    c.ResourceBonusStat = StatType.Charisma;
                }));
            });

            LoSContext.Logger.LogPatch("Created traditional Ki Pool modifier for Charisma", charisma_KiPoolCanon);

            #endregion

            #region | Changed Scaled Fist Ki Pool to use generic Monk Ki resource. |

            scaledFistKiPowerFeature.GetComponent<AddAbilityResources>().TemporaryContext(c => {
                c.m_Resource = kiResource.ToReference<BlueprintAbilityResourceReference>();
            });
            LoSContext.Logger.LogPatch("Enabled generic Monk Ki for Scaled Fist", scaledFistKiPowerFeature);


            #endregion


            #region | Replace Scaled Fist resource with basic monk resource. |

            foreach (BlueprintAbility scaledFistKiAbility in KiTools.MonkScaledFistKiAbilities.AllMonkScaledFistKiAbilties)
            {
                scaledFistKiAbility.GetComponent<AbilityResourceLogic>().TemporaryContext(c => {
                    c.m_RequiredResource = kiResource.ToReference<BlueprintAbilityResourceReference>();
                });
                LoSContext.Logger.LogPatch("Altered Scaled Fist Monk abilities to use generic Monk Ki", scaledFistKiAbility);
            }

            #endregion


            #region | Attach resource bonus features to the the original Ki Pool feature. |

            // Add the feature which grant the distinct (and not embedded) Ki Stat modifier.

            var canon_ki_modifier_exclusions = new BlueprintUnitFactReference[] { wisdom_KiPoolCanon.ToReference<BlueprintUnitFactReference>(), charisma_KiPoolCanon.ToReference<BlueprintUnitFactReference>() };

            kiPowerFeature.AddComponent<HasFactsFeaturesUnlock> (c => {
                c.m_CheckedFacts = canon_ki_modifier_exclusions;
                c.m_Features = new BlueprintUnitFactReference[] { wisdom_KiPoolCanon.ToReference<BlueprintUnitFactReference>() };
            });

            scaledFistKiPowerFeature.AddComponent<HasFactsFeaturesUnlock>(c => {
                c.m_CheckedFacts = canon_ki_modifier_exclusions;
                c.m_Features = new BlueprintUnitFactReference[] { charisma_KiPoolCanon.ToReference<BlueprintUnitFactReference>() };
            });

            #endregion


            #region | Make the Extra Attack Buff's name and description generic. |


            var Ki_Extra_Attack_Buff = BlueprintTools.GetBlueprint<BlueprintFeature>("cadf8a5c42002494cabfc6c1196b514a");

            Ki_Extra_Attack_Buff.SetName(LoSContext, "Ki - Extra Attack");
            Ki_Extra_Attack_Buff.SetDescription(LoSContext, "By spending points from his ki pool, the character can make one additional attack at his highest attack bonus when making a full attack. This bonus attack stacks with haste and similar effects.");


            #endregion


        }

        public static void ConfigureAdvancedKiResourceChanges()
        {

            var KiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e9590244effb4be4f830b1e3fffced13");
            var ScaledFistKiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("7d002c1025fbfe2458f1509bf7a89ce1");
            var KiPowerResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var ScaledFistKiPowerResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("ae98ab7bda409ef4bb39149a212d6732");

            var StrengthKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolStrength.png");
            var DexterityKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolDexterity.png");
            var ConstitutionKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolConstitution");
            var IntelligenceKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolIntelligence.png");
            var WisdomKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolWisdom.png");
            var CharismaKiPoolIcon = AssetLoader.LoadInternal(LoSContext, folder: "assets/icons", file: "KiPoolCharisma.png");





















        }


    }
}

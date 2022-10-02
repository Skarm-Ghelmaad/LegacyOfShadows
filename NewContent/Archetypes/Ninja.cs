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

namespace LegacyOfShadows.New_Content.Archetypes
{
    public class Ninja
    {

        private static readonly string NinjaProficienciesFeatureName = "NinjaProficiencies.Name";
        private static readonly string NinjaProficienciesFeatureDescription = "NinjaProficiencies.Description";

        static void ConfigureNinjaProficiencies()
        {
            var Rogue_Proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("33e2a7e4ad9daa54eaf808e1483bb43c");
            var Dueling_Sword_Proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("x");

            Rogue_Proficiencies.CreateCopy(LoSContext, "NinjaProficiencies", bp => {
                bp.ReplaceComponents<AddProficiencies>(Helpers.Create<AddProficiencies>(c => {
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
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { Dueling_Sword_Proficiencies.ToReference<BlueprintUnitFactReference>() };
                });
                bp.SetName(LoSContext, NinjaProficienciesFeatureName);
                bp.SetDescription(LoSContext, NinjaProficienciesFeatureDescription);

            });

        }
















    }
}

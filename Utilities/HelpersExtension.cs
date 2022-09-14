﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI;
using Kingmaker.UI.Log;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Designers.Mechanics.Prerequisites;
using System.IO;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Dreamteck.Splines.Primitives;
using TabletopTweaks.Core.Utilities;
using static LegacyOfShadows.Main;
using LegacyOfShadows.NewComponents;
using LegacyOfShadows.Utilities;
using Epic.OnlineServices;
using static TabletopTweaks.Core.Utilities.ClassTools;
using static LayoutRedirectElement;
using System.Drawing;
using System.Xml.Linq;
using TabletopTweaks.Core.UMMTools.Utility;
using HarmonyLib;

namespace LegacyOfShadows.Utilities
{
    public static class HelpersExtension
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++/ GETTER & SETTERS /++++++++++++++++++++++++++++++++++++++++++++++++++//

        // BORROWED CODE
        // Note: This was cannibalized from KingmakerToolkit.Shared in Races Unleashed Kingmaker mod and ported to WotR and was further adjusted by borrowing code from CotW Kingmaker mod. 

        public static T GetField<T>(object obj, string name)
        {
            return (T)((object)HarmonyLib.AccessTools.Field(obj.GetType(), name).GetValue(obj));
        }

        public static object GetField(Type type, object obj, string name)
        {
            return HarmonyLib.AccessTools.Field(type, name).GetValue(obj);
        }

        public static object GetField(object obj, string name)
        {
            return HarmonyLib.AccessTools.Field(obj.GetType(), name).GetValue(obj);
        }

        public static void SetField(object obj, string name, object value)
        {
            HarmonyLib.AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++/ CREATORS /++++++++++++++++++++++++++++++++++++++++++++++++++//

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were borrowed from Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // Copyright (c) 2019 Jennifer Messerly
        // Copyright (c) 2020 Denis Biryukov
        // This code is licensed under MIT license (see LICENSE for details)
        // -------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------/ GENERIC CREATORS /----------------------------------------------------//

        public static PrefabLink CreatePrefabLink(string asset_id)
        {
            var link = new PrefabLink();
            link.AssetId = asset_id;
            return link;
        }

        public static UnitViewLink CreateUnitViewLink(string asset_id)
        {
            var link = new UnitViewLink();
            link.AssetId = asset_id;
            return link;
        }

        //----------------------------------------/ QUICK COMPONENT CREATORS /----------------------------------------------------//

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were inspired by Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // -------------------------------------------------------------------------------------------------------------------------

        public static ContextCalculateAbilityParamsBasedOnClass CreateContextCalculateAbilityParamsBasedOnClass( BlueprintCharacterClassReference character_class,
                                                                                                                 StatType stat, 
                                                                                                                 bool use_kineticist_main_stat = false)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>();
            c.m_CharacterClass = character_class;
            c.StatType = stat;
            c.UseKineticistMainStat = use_kineticist_main_stat;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClasses (BlueprintCharacterClassReference[] character_classes,
                                                                                                                                   StatType stat )
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses createContextCalculateAbilityParamsBasedOnClassesWithProperty (BlueprintCharacterClassReference[] character_classes,
                                                                                                                                 BlueprintUnitPropertyReference property,
                                                                                                                                 StatType stat = StatType.Charisma )
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes   (BlueprintCharacterClassReference[] character_classes,
                                                                                                                                     BlueprintArchetypeReference[] archetypes,
                                                                                                                                     StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            return c;
        }

        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypesWithProperty (BlueprintCharacterClassReference[] character_classes,
                                                                                                                                               BlueprintArchetypeReference[] archetypes,
                                                                                                                                               BlueprintUnitPropertyReference property,
                                                                                                                                               StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }


        public static BlueprintAbilityResource CreateAbilityResource(String name,
                                                                      Sprite icon = null,
                                                                      params BlueprintComponent[] components  )

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource> (LoSContext, name, bp => {
                                                                                bp.m_Icon = icon;
                                                                                bp.SetComponents(components);
                                                                                });
            return resource;
        }

        public static AbilityResourceLogic CreateResourceLogic(this BlueprintAbilityResource resource, 
                                                                bool spend = true, 
                                                                int amount = 1, 
                                                                bool cost_is_custom = false)

        {
            var arl = Helpers.Create<AbilityResourceLogic>();
            arl.m_IsSpendResource = spend;
            arl.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
            arl.Amount = amount;
            arl.CostIsCustom = cost_is_custom;
            return arl;
        }

        //----------------------------------------/ RESOURCE-RELATED FUNCTIONS /----------------------------------------------------//

        public static void SetFixedResource(this BlueprintAbilityResource resource, int baseValue)
        {
            var amount = resource.m_MaxAmount;
            amount.BaseValue = baseValue;

            // Enusre arrays are at least initialized to empty.
            var emptyClasses = Array.Empty<BlueprintCharacterClassReference>();
            var emptyArchetypes = Array.Empty<BlueprintArchetypeReference>();


            if ( amount.m_Class == null) amount.m_Class = emptyClasses;
            if (amount.m_ClassDiv == null) amount.m_ClassDiv = emptyClasses;
            if (amount.m_Archetypes == null) amount.m_Archetypes = emptyArchetypes;
            if (amount.m_ArchetypesDiv == null) amount.m_ArchetypesDiv = emptyArchetypes;

            resource.m_MaxAmount = amount;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++// CONVERTERS //++++++++++++++++++++++++++++++++++++++++++++++++++//

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were borrowed from Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // Copyright (c) 2019 Jennifer Messerly
        // Copyright (c) 2020 Denis Biryukov
        // This code is licensed under MIT license (see LICENSE for details)
        // -------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------/ CONVERTERS FOR FEATURE CREATION  /----------------------------------------------------//

        // This converter creates a feature that matches the ability from which has been created.
        // This was mostly used (in Holic75's mod) to create features that add bonus or required features (such as Iroran Paladin's auto-selection of Irori as Deity)

        static public BlueprintFeature ConvertFeatureToFeature(BlueprintFeature feature1,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "",
                                                                    string suffixRemove = "",
                                                                    string replaceOldText1 = "",
                                                                    string replaceOldText2 = "",
                                                                    string replaceOldText3 = "",
                                                                    string replaceNewText1 = "",
                                                                    string replaceNewText2 = "",
                                                                    string replaceNewText3 = "",
                                                                    bool hide = true
                                                                )
        {

            string feature1AltName = feature1.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                feature1AltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                feature1AltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                feature1AltName = prefixAdd + feature1AltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                feature1AltName = feature1AltName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                feature1AltName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                feature1AltName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                feature1AltName.Replace(replaceOldText3, replaceNewText3);
            }

            var feature2 = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, feature1AltName, bp => {
                bp.SetName(LoSContext, feature1.Name);
                bp.SetDescription(LoSContext, feature1.Description);
                bp.m_Icon = feature1.Icon;
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = feature1.ToReference<BlueprintFeatureReference>();
                });


                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature2.HideInCharacterSheetAndLevelUp = true;
            }

            return feature2;
        }

        // This converter creates a feature that matches the ability from which has been created.

        static public BlueprintFeature ConvertAbilityToFeature ( BlueprintAbility ability,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "Feature",
                                                                    string suffixRemove = "Ability",
                                                                    bool hide = true            
                                                                )
        {

            string abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName.Replace(prefixRemove,"");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName.Replace(suffixRemove,"");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp => {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // This converter creates a feature that matches the ability from which has been created (version without replacements)

        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
                                                            bool hide = true
                                                        )
        {

            string abilityAltName = ability.Name + "Feature";


            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp => {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }


        // This converter creates a feature that matches the ability from which has been created but adds without checking.

        static public BlueprintFeature ConvertAbilityToFeatureNoCheck(BlueprintAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "Ability",
                                                                        bool hide = true
                                                                    )
        {

            string abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp => {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[]{ ability.ToReference<BlueprintUnitFactReference>() };
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }



        // This converter creates a feature that matches the buff from which has been created.

        static public BlueprintFeature ConvertBuffToFeature(BlueprintBuff buff,
                                                            string prefixAdd = "",
                                                            string prefixRemove = "",
                                                            string suffixAdd = "Feature",
                                                            string suffixRemove = "",
                                                            bool hide = true
                                                        )
        {

            string buffAltName = buff.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                buffAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                buffAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                buffAltName = prefixAdd + buffAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                buffAltName = buffAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, buffAltName, bp => {
                bp.SetName(LoSContext, buff.Name);
                bp.SetDescription(LoSContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.SetComponents(buff.ComponentsArray);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            return feature;
        }


        //------------------------------------------------/ CONVERTERS FOR ABILITIES FROM SPELLS  /----------------------------------------------------//

        // This converter creates a spell-like ability from an existing spell.
        // Compared to the original Holic75's version, I have added more optional string parameters to allow to customize the name of the spell-like ability.

        static public BlueprintAbility ConvertSpellToSpellLike(BlueprintAbility spell,
                                                               BlueprintCharacterClassReference[] classes,
                                                               StatType stat, 
                                                               BlueprintAbilityResource resource = null,
                                                               string prefixAdd = "",
                                                               string prefixRemove = "",
                                                               string suffixAdd = "",
                                                               string suffixRemove = "",
                                                               string replaceOldText1 = "",
                                                               string replaceOldText2 = "",
                                                               string replaceOldText3 = "",
                                                               string replaceNewText1 = "",
                                                               string replaceNewText2 = "",
                                                               string replaceNewText3 = "",
                                                               bool no_resource = false,
                                                               bool no_scaling = false,
                                                               string guid = "",
                                                               BlueprintArchetypeReference[] archetypes = null,
                                                               int cost = 1

                                                              )
        {

            string spellliketName = spell.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                spellliketName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                spellliketName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spellliketName = prefixAdd + spellliketName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                spellliketName = spellliketName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                spellliketName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                spellliketName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                spellliketName.Replace(replaceOldText3, replaceNewText3);
            }

            var ability = spell.CreateCopy(LoSContext, spellliketName);

            if (!no_scaling)
            {
                ability.RemoveComponents<SpellListComponent>();
            }

            ability.Type = AbilityType.SpellLike;

            if (!no_scaling)
            {
                ability.AddComponent(CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(classes, archetypes, stat));
            }

            ability.MaterialComponent = BlueprintTools.GetBlueprint<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3").MaterialComponent; // Fireball spell (no component)

            if (!no_resource)
            {
                var resource2 = resource;
                if (resource2 == null)
                {
                    resource2 = CreateAbilityResource(spellliketName + "Resource", null);
                    resource2.SetFixedResource(cost);
                }
                ability.AddComponent(CreateResourceLogic(resource2, amount: cost));
            }

            ability.Parent = null;
            return ability;

        }















































    }
}

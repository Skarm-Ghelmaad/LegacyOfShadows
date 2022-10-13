using System;
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
using Kingmaker.UnitLogic.Class.Kineticist;
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
using Owlcat.QA.Validation;
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
using Owlcat.Runtime.Core.Physics.PositionBasedDynamics.Forces;
using System.ComponentModel;
using AK.Wwise;
using System.Reflection.Emit;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using static Kingmaker.Dungeon.Actions.DungeonShowResults;
using TabletopTweaks.Core.NewComponents.Properties;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Class.Kineticist.Properties;
using UnityEngine.Assertions.Must;


namespace LegacyOfShadows.Utilities
{
    public static class HelpersExtension
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++/ GETTER & SETTERS /++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region |---------------------------------------------------------/ FIELD GETTERS & SETTERS /---------------------------------------------------------|

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


        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++/ CREATORS /++++++++++++++++++++++++++++++++++++++++++++++++++//

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were borrowed from Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // Copyright (c) 2019 Jennifer Messerly
        // Copyright (c) 2020 Denis Biryukov
        // This code is licensed under MIT license (see LICENSE for details)
        // -------------------------------------------------------------------------------------------------------------------------


        #region |------------------------------------------------------------/ GENERIC CREATORS /-------------------------------------------------------------|



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

        #endregion

        #region |----------------------------------------------------/ QUICK COMPONENT CREATORS /-------------------------------------------------------------|


        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were inspired by Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // -------------------------------------------------------------------------------------------------------------------------

        public static ContextCalculateAbilityParamsBasedOnClass CreateContextCalculateAbilityParamsBasedOnClass(BlueprintCharacterClassReference character_class,
                                                                                                                 StatType stat,
                                                                                                                 bool use_kineticist_main_stat = false)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>();
            c.m_CharacterClass = character_class;
            c.StatType = stat;
            c.UseKineticistMainStat = use_kineticist_main_stat;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClasses(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                   StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses createContextCalculateAbilityParamsBasedOnClassesWithProperty(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                 BlueprintUnitPropertyReference property,
                                                                                                                                 StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }


        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(BlueprintCharacterClassReference[] character_classes,
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

        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypesWithProperty(BlueprintCharacterClassReference[] character_classes,
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
                                                                      params BlueprintComponent[] components)

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(LoSContext, name, bp =>
            {
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

        // This is essentially copied from Holic's version, but I have added the reference to Vek's Create since I use that one.

        public static AbilityTargetsAround CreateAbilityTargetsAround(Feet radius, TargetType targetType, ConditionsChecker conditions = null, Feet spreadSpeed = default(Feet), bool includeDead = false)
        {
            var around = Helpers.Create<AbilityTargetsAround>();
            SetField(around, "m_Radius", radius);
            SetField(around, "m_TargetType", targetType);
            SetField(around, "m_IncludeDead", includeDead);
            SetField(around, "m_Condition", conditions ?? new ConditionsChecker() { Conditions = Array.Empty<Condition>() });
            SetField(around, "m_SpreadSpeed", spreadSpeed);
            return around;
        }


        #endregion

        #region |---------------------------------------------------/ RESOURCE-RELATED FUNCTIONS /------------------------------------------------------------|


        public static void SetFixedResource(this BlueprintAbilityResource resource, int baseValue)
        {
            var amount = resource.m_MaxAmount;
            amount.BaseValue = baseValue;

            // Enusre arrays are at least initialized to empty.
            var emptyClasses = Array.Empty<BlueprintCharacterClassReference>();
            var emptyArchetypes = Array.Empty<BlueprintArchetypeReference>();


            if (amount.m_Class == null) amount.m_Class = emptyClasses;
            if (amount.m_ClassDiv == null) amount.m_ClassDiv = emptyClasses;
            if (amount.m_Archetypes == null) amount.m_Archetypes = emptyArchetypes;
            if (amount.m_ArchetypesDiv == null) amount.m_ArchetypesDiv = emptyArchetypes;

            resource.m_MaxAmount = amount;
        }

        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++// CONVERTERS //++++++++++++++++++++++++++++++++++++++++++++++++++//

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were borrowed from Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // Copyright (c) 2019 Jennifer Messerly
        // Copyright (c) 2020 Denis Biryukov
        // This code is licensed under MIT license (see LICENSE for details)
        // -------------------------------------------------------------------------------------------------------------------------

        #region |-------------------------------------------/ CONVERTERS FOR ACTIVABLE ABILITY CREATION  /----------------------------------------------------|


        // This converter creates a toggle that activates the buff. This is the full version, which adds all kind of name modification features.

        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string prefixAdd = "",
                                                                                  string prefixRemove = "",
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                  string replaceOldText1 = "",
                                                                                  string replaceOldText2 = "",
                                                                                  string replaceOldText3 = "",
                                                                                  string replaceNewText1 = "",
                                                                                  string replaceNewText2 = "",
                                                                                 string replaceNewText3 = "",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                toggleName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                toggleName = prefixAdd + toggleName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                toggleName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                toggleName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                toggleName.Replace(replaceOldText3, replaceNewText3);
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(LoSContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(LoSContext, buff.Name);
                bp.SetDescription(LoSContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }

        // This converter creates a toggle that activates the buff. This is the barebone version which lists only suffix to remove and to add.

        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.Name;

            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(LoSContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(LoSContext, buff.Name);
                bp.SetDescription(LoSContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }


        #endregion

        #region |------------------------------------------------/ CONVERTERS FOR FEATURE CREATION  /---------------------------------------------------------|


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

            var feature2 = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, feature1AltName, bp =>
            {
                bp.SetName(LoSContext, feature1.Name);
                bp.SetDescription(LoSContext, feature1.Description);
                bp.m_Icon = feature1.Icon;
                bp.AddComponent<AddFeatureOnApply>(c =>
                {
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

        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
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

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp =>
            {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
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


            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp =>
            {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
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

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, abilityAltName, bp =>
            {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
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

        // This converter creates a feature that matches the activatable ability from which has been created and adds it without checking.

        static public BlueprintFeature ConvertActivatableAbilityToFeature(BlueprintActivatableAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "ToggleAbility",
                                                                        bool hide = true
                                                                    )
        {

            string activatable_abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                activatable_abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                activatable_abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                activatable_abilityAltName = prefixAdd + activatable_abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                activatable_abilityAltName = activatable_abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, activatable_abilityAltName, bp =>
            {
                bp.SetName(LoSContext, ability.Name);
                bp.SetDescription(LoSContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
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

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, buffAltName, bp =>
            {
                bp.SetName(LoSContext, buff.Name);
                bp.SetDescription(LoSContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.SetComponents(buff.ComponentsArray);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            return feature;
        }

        #endregion

        #region |------------------------------------------------/ ABILITY VARIANTS MANIPULATION  /-----------------------------------------------------------|

        // -------------------------------------------------------------------------------------------------------------------------
        // Note: These were borrowed from Holic75's KingmakerRebalance/CotW Kingmaker mod. 
        // Copyright (c) 2019 Jennifer Messerly
        // Copyright (c) 2020 Denis Biryukov
        // This code is licensed under MIT license (see LICENSE for details)
        // -------------------------------------------------------------------------------------------------------------------------


        // These creators create a list of variant of the parent BlueprintAbility from a given variants list and then adds the parent as such in each of these variants' Blueprints

        public static AbilityVariants CreateAbilityVariants(this BlueprintAbility parent, IEnumerable<BlueprintAbility> variants) => CreateAbilityVariants(parent, variants.ToArray());

        public static AbilityVariants CreateAbilityVariants(this BlueprintAbility parent, params BlueprintAbility[] variants)
        {
            var a = Helpers.Create<AbilityVariants>();

            BlueprintAbilityReference[] variants_reference = new BlueprintAbilityReference[variants.Length];

            for (int i = 0; i < variants_reference.Length; i++)
            {
                variants_reference[i] = variants[i].ToReference<BlueprintAbilityReference>();
            }

            a.m_Variants = variants_reference;
            foreach (var vr in variants)
            {
                vr.m_Parent = parent.ToReference<BlueprintAbilityReference>();
            }
            return a;
        }

        // This method adds variants to an existing ability variants list and then adds the parent as such in each of these new variants' Blueprints

        public static bool AddToAbilityVariants(this BlueprintAbility parent, params BlueprintAbility[] variants)
        {
            var cmp = parent.GetComponent<AbilityVariants>();

            BlueprintAbilityReference[] variants_reference = new BlueprintAbilityReference[variants.Length];

            for (int i = 0; i < variants_reference.Length; i++)
            {
                variants_reference[i] = variants[i].ToReference<BlueprintAbilityReference>();
            }

            cmp.m_Variants = cmp.m_Variants.AppendToArray(variants_reference);

            foreach (var vr in variants)
            {
                vr.m_Parent = parent.ToReference<BlueprintAbilityReference>();
            }

            return true;

        }

        // This method creates a wrapper for certain variants

        public static BlueprintAbility CreateVariantWrapper(string name, params BlueprintAbility[] variants)
        {
            var first_variant = variants[0];

            var wrapper = first_variant.CreateCopy(LoSContext, name, bp =>
            {

                List<BlueprintComponent> cmps = new List<BlueprintComponent>();
                cmps.Add(CreateAbilityVariants(bp, variants));
                bp.ComponentsArray = cmps.ToArray();

            });

            return wrapper;
        }

        // This method creates spell-like variants from a spell (with variants). This is the most complete version, which allows to add a specific prefix for the wrapper and a lot of text modifications.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).


        static public BlueprintAbility ConvertSpellToSpellLikeVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
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
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {
            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;
            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSpellLike(spell_variants[i], classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }

            string spelllikeWrapperName = spell.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                spelllikeWrapperName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                spelllikeWrapperName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeWrapperName = prefixAdd + spelllikeWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                spelllikeWrapperName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                spelllikeWrapperName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                spelllikeWrapperName.Replace(replaceOldText3, replaceNewText3);
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(spelllikeWrapperName, abilities);

            wrapper.SetName(LoSContext, spell.Name);
            wrapper.SetDescription(LoSContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;

        }


        // This method creates spell-like variants from a spell (with variants), but drops all the string alterations BUT the prefix AND the prefix for the wrapper.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).


        static public BlueprintAbility ConvertSpellToSpellLikeVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {
            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;
            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSpellLike(spell_variants[i], classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }

            string spelllikeWrapperName = spell.Name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeWrapperName = prefixAdd + spelllikeWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(spelllikeWrapperName, abilities);

            wrapper.SetName(LoSContext, spell.Name);
            wrapper.SetDescription(LoSContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;

        }

        // This method creates supernatural variants from a spell (with variants).  This is the most complete version, which allows to add a specific prefix for the wrapper and a lot of text modifications.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).


        static public BlueprintAbility ConvertSpellToSupernaturalVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
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
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {

            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSupernatural(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;

            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSupernatural(spell_variants[i], classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }


            string supernaturalWrapperName = spell.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                supernaturalWrapperName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                supernaturalWrapperName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                supernaturalWrapperName = prefixAdd + supernaturalWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                supernaturalWrapperName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                supernaturalWrapperName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                supernaturalWrapperName.Replace(replaceOldText3, replaceNewText3);
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixforWrapperAdd;
            }


            var wrapper = CreateVariantWrapper(supernaturalWrapperName, abilities);

            wrapper.SetName(LoSContext, spell.Name);
            wrapper.SetDescription(LoSContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;


        }

        // This method creates supernatural variants from a spell (with variants), but drops all the string alterations BUT the prefix AND the prefix for the wrapper.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).


        static public BlueprintAbility ConvertSpellToSupernaturalVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {

            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSupernatural(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;

            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSupernatural(spell_variants[i], classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }


            string supernaturalWrapperName = spell.Name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                supernaturalWrapperName = prefixAdd + supernaturalWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(supernaturalWrapperName, abilities);

            wrapper.SetName(LoSContext, spell.Name);
            wrapper.SetDescription(LoSContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;


        }


        #endregion

        #region |------------------------------------------------/ CONVERTERS FOR ABILITIES FROM SPELLS  /----------------------------------------------------|


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
                                                               BlueprintArchetypeReference[] archetypes = null,
                                                               int cost = 1

                                                              )
        {

            string spelllikeName = spell.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                spelllikeName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                spelllikeName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeName = prefixAdd + spelllikeName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                spelllikeName = spelllikeName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                spelllikeName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                spelllikeName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                spelllikeName.Replace(replaceOldText3, replaceNewText3);
            }

            var ability = spell.CreateCopy(LoSContext, spelllikeName);

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
                    resource2 = CreateAbilityResource(spelllikeName + "Resource", null);
                    resource2.SetFixedResource(cost);
                }
                ability.AddComponent(CreateResourceLogic(resource2, amount: cost));
            }

            ability.Parent = null;
            return ability;

        }


        // This converter variant converts a spell-like ability from an existing spell, but drops all the string alterations BUT the prefix.

        static public BlueprintAbility ConvertSpellToSpellLike(BlueprintAbility spell,
                                                               BlueprintCharacterClassReference[] classes,
                                                               StatType stat,
                                                               BlueprintAbilityResource resource = null,
                                                               string prefixAdd = "",
                                                               bool no_resource = false,
                                                               bool no_scaling = false,
                                                               BlueprintArchetypeReference[] archetypes = null,
                                                               int cost = 1

                                                              )
        {

            string spelllikeName = spell.Name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeName = prefixAdd + spelllikeName;
            }

            var ability = spell.CreateCopy(LoSContext, spelllikeName);

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
                    resource2 = CreateAbilityResource(spelllikeName + "Resource", null);
                    resource2.SetFixedResource(cost);
                }
                ability.AddComponent(CreateResourceLogic(resource2, amount: cost));
            }

            ability.Parent = null;
            return ability;

        }

        // This converter creates a supernatural ability from an existing spell.
        // Compared to the original Holic75's version, I have added more optional string parameters to allow to customize the name of the supernatural ability,
        // moreover I have completely redone the part of non-dispellable buffs to avoid to have to port the changeAction method, which seemed either impossible or (more likely)
        // too hard for me to port!

        static public BlueprintAbility ConvertSpellToSupernatural(BlueprintAbility spell,
                                                                   BlueprintCharacterClassReference[] classes,
                                                                   StatType stat,
                                                                   BlueprintAbilityResource resource = null,
                                                                   string prefixAdd = "",
                                                                   bool no_resource = false,
                                                                   bool no_scaling = false,
                                                                   BlueprintArchetypeReference[] archetypes = null,
                                                                   int cost = 1

                                                                  )
        {

            var ability = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes: archetypes, cost: cost);
            ability.Type = AbilityType.Supernatural;
            ability.SpellResistance = false;
            ability.RemoveComponents<SpellComponent>();
            ability.AvailableMetamagic = (Metamagic)0;

            GameAction[] action_storage = new GameAction[0];

            //make buffs non dispellable
            var actions = ability.GetComponent<AbilityEffectRunAction>();

            ability.FlattenAllActions()
                   .OfType<ContextActionApplyBuff>()
                        .ForEach(b =>
                        {
                            b.IsNotDispelable = true;
                            b.IsFromSpell = false;
                        });


            return ability;

        }


        // This converter creates a supernatural ability from an existing spell, but drops all the string alterations BUT the prefix.

        static public BlueprintAbility ConvertSpellToSupernatural(BlueprintAbility spell,
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
                                                                   BlueprintArchetypeReference[] archetypes = null,
                                                                   int cost = 1

                                                                  )
        {

            var ability = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes: archetypes, cost: cost);
            ability.Type = AbilityType.Supernatural;
            ability.SpellResistance = false;
            ability.RemoveComponents<SpellComponent>();
            ability.AvailableMetamagic = (Metamagic)0;

            GameAction[] action_storage = new GameAction[0];

            //make buffs non dispellable
            var actions = ability.GetComponent<AbilityEffectRunAction>();

            ability.FlattenAllActions()
                   .OfType<ContextActionApplyBuff>()
                        .ForEach(b =>
                        {
                            b.IsNotDispelable = true;
                            b.IsFromSpell = false;
                        });


            return ability;

        }


        #endregion

        #region |------------------------------------------------/ CONVERTERS FOR SPECIFIC FEATURE CREATION  /------------------------------------------------|


        // These converters are meant to create specific class features from existing spells.

        // KINETIC TALENTS

        // This converts a spell to a kinetic talent, allowing for different name alterations.

        static public BlueprintAbility ConvertSpellToKineticistTalent(BlueprintAbility spell,
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
                                                                       int burn_cost = 0)
        {
            var kineticist = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391").ToReference<BlueprintCharacterClassReference>();

            var ability = ConvertSpellToSpellLike(spell, new BlueprintCharacterClassReference[] { kineticist }, StatType.Unknown, null, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource: true, no_scaling: true, null, 0);

            ability.AddComponents(Helpers.Create<AbilityKineticist>(a => { a.Amount = burn_cost; a.WildTalentBurnCost = burn_cost; }),
                                  Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>(c => { c.m_CharacterClass = kineticist; c.StatType = StatType.Constitution; c.UseKineticistMainStat = true; }));

            ability.RemoveComponents<SpellListComponent>();
            return ability;

        }

        // This converts a spell to a kinetic talent with just a simplified name change (prefix added).

        static public BlueprintAbility ConvertSpellToKineticistTalent(BlueprintAbility spell,
                                                               string prefixAdd = "",
                                                               int burn_cost = 0)
        {
            var kineticist = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391").ToReference<BlueprintCharacterClassReference>();

            var ability = ConvertSpellToSpellLike(spell, new BlueprintCharacterClassReference[] { kineticist }, StatType.Unknown, null, prefixAdd, no_resource: true, no_scaling: true, null, 0);

            ability.AddComponents(Helpers.Create<AbilityKineticist>(a => { a.Amount = burn_cost; a.WildTalentBurnCost = burn_cost; }),
                                  Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>(c => { c.m_CharacterClass = kineticist; c.StatType = StatType.Constitution; c.UseKineticistMainStat = true; }));

            ability.RemoveComponents<SpellListComponent>();
            return ability;

        }


        // KI POWERS

        // This converts a spell to a ki power, allowing for different name alterations.

        public static void ConvertSpellToMonkKiPower(BlueprintAbility spell,
                                                     int required_level,
                                                     bool personal_only,
                                                     int cost = 1)
        {
            var monk = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124").ToReference<BlueprintCharacterClassReference>();

            var wis_resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82").ToReference<BlueprintAbilityResourceReference>(); // standard Ki resource (based on Wis)
            var cha_resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("7d002c1025fbfe2458f1509bf7a89ce1").ToReference<BlueprintAbilityResourceReference>(); // Scaled Fist's Ki resource (based on Cha)

            var monk_ki_power_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551").ToReference<BlueprintFeatureSelectionReference>();
            var scaled_fist_ki_power_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541").ToReference<BlueprintFeatureSelectionReference>();

            var sensei_mystic_powers = BlueprintTools.GetBlueprint<BlueprintFeature>("d5f7bcde6e7e5ed498f430ebf5c29837").ToReference<BlueprintFeatureReference>(); // Sensei Mystic Powers
            var sensei_mystic_powers_mass = BlueprintTools.GetBlueprint<BlueprintFeature>("a316044187ec61344ba33535f42f6a4d").ToReference<BlueprintFeatureReference>(); // Sensei Mass Mystic Powers

            string action_type = "standard";
            if (spell.ActionType == UnitCommand.CommandType.Swift)
            {
                action_type = "swift";
            }
            else if (spell.ActionType == UnitCommand.CommandType.Move)
            {
                action_type = "move";
            }
            else if (spell.IsFullRoundAction)
            {
                action_type = "full-round";
            }

            var description = $"A monk with this ki power can spend {cost} point{(cost != 1 ? "s" : "")} from his ki pool to apply effect of the {spell.Name} spell to himself as a {action_type} action.\n"
            + spell.Name + ": " + spell.Description;

            var name = "Ki Power: " + spell.Name;

            var monk_ability = ConvertSpellToSpellLikeVariants(spell, new BlueprintCharacterClassReference[] { monk }, StatType.Wisdom, wis_resource, "Base", "KiPower", false, false, personal_only, null, cost);

            var scaled_fist_ability = ConvertSpellToSpellLikeVariants(spell, new BlueprintCharacterClassReference[] { monk }, StatType.Charisma, cha_resource, "Base", "ScaledFistKiPower", false, false, personal_only, null, cost);

            monk_ability.SetNameDescription(LoSContext, name, description);

            scaled_fist_ability.SetNameDescription(LoSContext, name, description);

            if (monk_ability.HasVariants)
            {

                var monk_ability_variants_reference = monk_ability.GetComponent<AbilityVariants>().m_Variants;

                foreach (var v in monk_ability_variants_reference)
                {
                    var v_orig = v.Get();
                    v_orig.SetName(LoSContext, "Ki Power: " + v_orig.Name);
                }

            }

            if (scaled_fist_ability.HasVariants)
            {

                var scaled_fist_ability_variants_reference = scaled_fist_ability.GetComponent<AbilityVariants>().m_Variants;

                foreach (var v in scaled_fist_ability_variants_reference)
                {
                    var v_orig = v.Get();
                    v_orig.SetName(LoSContext, "Ki Power: " + v_orig.Name);
                }

            }

            var monk_feature = ConvertAbilityToFeature(monk_ability, "", "", "Feature", "Ability", false);

            var scaled_fist_feature = ConvertAbilityToFeature(scaled_fist_ability, "", "", "Feature", "Ability", false);

            monk_feature.AddPrerequisite<PrerequisiteClassLevel>(p => { p.m_CharacterClass = monk; p.Level = required_level; });

            scaled_fist_feature.AddPrerequisite<PrerequisiteClassLevel>(p => { p.m_CharacterClass = monk; p.Level = required_level; });

            FeatToolsExtension.AddAsMonkKiPower(monk_feature);

            FeatToolsExtension.AddAsScaledFistKiPower(scaled_fist_feature);

            var mystic_wisdom_ability = monk_ability.CreateCopy(LoSContext, "SenseiAdvice" + monk_ability.name, bp =>
            {

                bp.Range = AbilityRange.Close;
                bp.SetMiscAbilityParametersSingleTargetRangedFriendly();
                bp.SetName(LoSContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: "));
                var cmp = bp.GetComponent<AbilityResourceLogic>();
                cmp.Amount = cmp.Amount + 1;

            });

            if (mystic_wisdom_ability.HasVariants)
            {
                var mystic_wisdom_ability_variants = mystic_wisdom_ability.GetComponent<AbilityVariants>().m_Variants;

                int num_variants = mystic_wisdom_ability_variants.Length;

                var mystic_wisdom_abilities = new BlueprintAbility[num_variants];

                var mystic_wisdom_abilities_reference = new BlueprintAbilityReference[num_variants];

                for (int i = 0; i < num_variants; i++)
                {
                    mystic_wisdom_abilities[i] = mystic_wisdom_abilities[i].CreateCopy(LoSContext, "SenseiAdvice" + mystic_wisdom_abilities[i].name, bp =>
                    {

                        bp.Range = AbilityRange.Close;
                        bp.SetMiscAbilityParametersSingleTargetRangedFriendly();
                        bp.SetName(LoSContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: "));
                        var cmp = bp.GetComponent<AbilityResourceLogic>();
                        cmp.Amount = cmp.Amount + 1;
                        bp.Parent = mystic_wisdom_ability;

                    });

                    mystic_wisdom_abilities_reference[i] = mystic_wisdom_abilities[i].ToReference<BlueprintAbilityReference>();
                }
                mystic_wisdom_ability.GetComponent<AbilityVariants>().m_Variants = mystic_wisdom_abilities_reference;

                sensei_mystic_powers.Get().AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = monk_feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = mystic_wisdom_ability.ToReference<BlueprintUnitFactReference>();
                });


            }


            var mystic_wisdom_ability_mass = monk_ability.CreateCopy(LoSContext, "SenseiAdviceMass" + monk_ability.name, bp =>
            {

                bp.SetName(LoSContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: Mass "));
                var cmp = bp.GetComponent<AbilityResourceLogic>();
                cmp.Amount = cmp.Amount + 2;

            });

            if (mystic_wisdom_ability_mass.HasVariants)
            {

                var mystic_wisdom_ability_mass_variants = mystic_wisdom_ability_mass.GetComponent<AbilityVariants>().m_Variants;

                int num_variants = mystic_wisdom_ability_mass_variants.Length;

                var mystic_wisdom_abilities_mass = new BlueprintAbility[num_variants];

                var mystic_wisdom_abilities_mass_reference = new BlueprintAbilityReference[num_variants];

                for (int i = 0; i < num_variants; i++)
                {
                    mystic_wisdom_abilities_mass[i] = mystic_wisdom_abilities_mass[i].CreateCopy(LoSContext, "SenseiAdviceMass" + mystic_wisdom_abilities_mass[i].name, bp =>
                    {

                        bp.SetName(LoSContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: Mass "));
                        bp.Parent = mystic_wisdom_ability_mass;
                        bp.AddComponent(CreateAbilityTargetsAround(30.Feet(), TargetType.Ally));
                        var cmp = bp.GetComponent<AbilityResourceLogic>();
                        cmp.Amount = cmp.Amount + 2;

                    });

                    mystic_wisdom_abilities_mass_reference[i] = mystic_wisdom_abilities_mass[i].ToReference<BlueprintAbilityReference>();
                }


            }
            else
            {

                mystic_wisdom_ability_mass.AddComponent(CreateAbilityTargetsAround(30.Feet(), TargetType.Ally));

            }

            sensei_mystic_powers_mass.Get().AddComponent<AddFeatureIfHasFact>(c =>
            {
                c.m_CheckedFact = monk_feature.ToReference<BlueprintUnitFactReference>();
                c.m_Feature = mystic_wisdom_ability_mass.ToReference<BlueprintUnitFactReference>();
            });


        }

        #endregion


        #region |------------------------------------------------/ ABILITY PARAMETERS QUICK CONFIGURATORS  /--------------------------------------------------|



        public static void SetMiscAbilityParametersSingleTargetRangedHarmful(this BlueprintAbility ability,
                                                                             bool works_on_allies = false,
                                                                             Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
        {
            ability.CanTargetFriends = works_on_allies;
            ability.CanTargetEnemies = true;
            ability.CanTargetSelf = false;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
            ability.EffectOnAlly = works_on_allies ? AbilityEffectOnUnit.Harmful : AbilityEffectOnUnit.None;
            ability.Animation = animation;
        }




        public static void SetMiscAbilityParametersSingleTargetRangedFriendly(this BlueprintAbility ability,
                                                                              bool works_on_self = false,
                                                                              Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
        {
            ability.CanTargetFriends = true;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = works_on_self;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;
        }


        public static void SetMiscAbilityParametersTouchHarmful(this BlueprintAbility ability,
                                                                bool works_on_allies = true,
                                                                Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
        {
            ability.CanTargetFriends = works_on_allies;
            ability.CanTargetEnemies = true;
            ability.CanTargetSelf = works_on_allies;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
            ability.EffectOnAlly = works_on_allies ? AbilityEffectOnUnit.Harmful : AbilityEffectOnUnit.None;
            ability.Animation = animation;

        }


        public static void SetMiscAbilityParametersTouchFriendly(this BlueprintAbility ability,
                                                                 bool works_on_self = true,
                                                                 Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
        {
            ability.CanTargetFriends = true;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = works_on_self;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;

        }

        public static void SetMiscAbilityParametersRangedDirectional(this BlueprintAbility ability,
                                                                      bool works_on_units = true,
                                                                      AbilityEffectOnUnit effect_on_ally = AbilityEffectOnUnit.Harmful,
                                                                      AbilityEffectOnUnit effect_on_enemy = AbilityEffectOnUnit.Harmful,
                                                                      Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
        {
            ability.CanTargetFriends = works_on_units;
            ability.CanTargetEnemies = works_on_units;
            ability.CanTargetSelf = works_on_units;
            ability.CanTargetPoint = true;
            ability.EffectOnEnemy = effect_on_enemy;
            ability.EffectOnAlly = effect_on_ally;
            ability.Animation = animation;

        }


        public static void SetMiscAbilityParametersSelfOnly(this BlueprintAbility ability,
                                                            Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
        {
            ability.CanTargetFriends = false;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = true;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;

        }





        #endregion


        #region |------------------------------------------------/ CONTEXT PARAMETERS CREATORS  /--------------------------------------------------|

        // These are taken straight-away from CotW.


        public static ContextValue CreateContextValueRank(AbilityRankType value = AbilityRankType.Default) => value.CreateContextValue();

        public static ContextValue CreateContextValue(this AbilityRankType value)
        {
            return new ContextValue() { ValueType = ContextValueType.Rank, ValueRank = value };
        }

        public static ContextValue CreateContextValue(this AbilitySharedValue value)
        {
            return new ContextValue() { ValueType = ContextValueType.Shared, ValueShared = value };
        }

        public static ContextDiceValue CreateContextDiceValue(this DiceType dice, ContextValue diceCount = null, ContextValue bonus = null)
        {
            return new ContextDiceValue()
            {
                DiceType = dice,
                DiceCountValue = diceCount ?? CreateContextValueRank(),
                BonusValue = bonus ?? 0
            };
        }


        public static ContextDurationValue CreateContextDuration(ContextValue bonus = null, DurationRate rate = DurationRate.Rounds, DiceType diceType = DiceType.Zero, ContextValue diceCount = null)
        {
            return new ContextDurationValue()
            {
                BonusValue = bonus ?? CreateContextValueRank(),
                Rate = rate,
                DiceCountValue = diceCount ?? 0,
                DiceType = diceType
            };
        }


        public static ContextDurationValue CreateContextDurationNonExtandable(ContextValue bonus = null, DurationRate rate = DurationRate.Rounds, DiceType diceType = DiceType.Zero, ContextValue diceCount = null)
        {
            var d = new ContextDurationValue()
            {
                BonusValue = bonus ?? CreateContextValueRank(),
                Rate = rate,
                DiceCountValue = diceCount ?? 0,
                DiceType = diceType
            };
            d.m_IsExtendable = false;

            return d;
        }

        #endregion


        #region |------------------------------------------------/ CONDITION CHECKERS CREATORS  /--------------------------------------------------|

        public static ConditionsChecker CreateConditionsCheckerAnd(params Condition[] conditions)
        {
            return new ConditionsChecker() { Conditions = conditions, Operation = Operation.And };
        }

        public static ConditionsChecker CreateConditionsCheckerOr(params Condition[] conditions)
        {
            return new ConditionsChecker() { Conditions = conditions, Operation = Operation.Or };
        }

        #endregion


        #region |------------------------------------------------------/ CONDITIONAL CREATORS  /--------------------------------------------------------|

        public static Conditional CreateConditional(Condition condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        public static Conditional CreateConditional(Condition[] condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }


        public static Conditional CreateConditionalOr(Condition[] condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerOr(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        public static Conditional CreateConditional(Condition[] condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        public static Conditional CreateConditional(ConditionsChecker conditions, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = conditions;
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        public static Conditional CreateConditional(Condition condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = new ConditionsChecker() { Conditions = new Condition[] { condition } };
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse ?? Array.Empty<GameAction>());
            return c;
        }

        public static Conditional CreateConditional(ConditionsChecker condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = condition;
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse ?? Array.Empty<GameAction>());
            return c;
        }

        #endregion


        #region |------------------------------------------------------/ CONTEXT CONDITION CREATORS  /--------------------------------------------------------|

        public static ContextConditionAlignment CreateContextConditionAlignment(AlignmentComponent alignment, bool check_caster = false, bool not = false)
        {
            var c = Helpers.Create<ContextConditionAlignment>();
            c.Alignment = alignment;
            c.Not = not;
            c.CheckCaster = check_caster;
            return c;
        }

        static public ContextConditionCasterHasFact CreateContextConditionCasterHasFact(BlueprintUnitFact fact, bool has = true)
        {
            var c = Helpers.Create<ContextConditionCasterHasFact>();
            c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
            c.Not = !has;
            return c;
        }

        public static ContextConditionHasBuff CreateConditionHasBuff(this BlueprintBuff buff)
        {
            var hasBuff = Helpers.Create<ContextConditionHasBuff>();
            hasBuff.m_Buff = buff.ToReference<BlueprintBuffReference>();
            return hasBuff;
        }

        public static ContextConditionHasBuff CreateConditionHasNoBuff(this BlueprintBuff buff)
        {
            var hasBuff = Helpers.Create<ContextConditionHasBuff>();
            hasBuff.m_Buff = buff.ToReference<BlueprintBuffReference>();
            hasBuff.Not = true;
            return hasBuff;
        }

        public static ContextConditionHasBuffFromCaster CreateContextConditionHasBuffFromCaster(BlueprintBuff buff, bool not = false)
        {
            var c = Helpers.Create<ContextConditionHasBuffFromCaster>();
            c.m_Buff = buff.ToReference<BlueprintBuffReference>();
            c.Not = not;
            return c;
        }

        static public ContextConditionHasFact CreateContextConditionHasFact(BlueprintUnitFact fact, bool has = true)
        {
            var c = Helpers.Create<ContextConditionHasFact>();
            c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
            c.Not = !has;
            return c;
        }

        static public ContextConditionIsCaster CreateContextConditionIsCaster(bool not = false)
        {
            var c = Helpers.Create<ContextConditionIsCaster>();
            c.Not = not;
            return c;
        }

        #endregion


        #region |------------------------------------------------------/ ACTION CREATORS  /--------------------------------------------------------|

        public static AbilityEffectRunAction CreateRunActions(params GameAction[] actions)
        {
            var result = Helpers.Create<AbilityEffectRunAction>();
            result.Actions = Helpers.CreateActionList(actions);
            return result;
        }


        public static AddFactContextActions CreateAddFactContextActions(GameAction[] onActivate = null, GameAction[] onDeactivate = null, GameAction[] onNewRound = null)
        {
            var a = Helpers.Create<AddFactContextActions>();
            a.Activated = Helpers.CreateActionList(onActivate);
            a.Deactivated = Helpers.CreateActionList(onDeactivate);
            a.NewRound = Helpers.CreateActionList(onNewRound);
            return a;
        }



        public static ContextActionApplyBuff CreateContextActionApplyBuff(this BlueprintBuff buff, ContextDurationValue duration, bool fromSpell, bool dispellable = true, bool toCaster = false, bool asChild = false, bool permanent = false)
        {
            var result = Helpers.Create<ContextActionApplyBuff>();
            result.m_Buff = buff.ToReference<BlueprintBuffReference>();
            result.DurationValue = duration;
            result.IsFromSpell = fromSpell;
            result.IsNotDispelable = !dispellable;
            result.ToCaster = toCaster;
            result.AsChild = asChild;
            result.Permanent = permanent;
            return result;
        }

        static public ContextActionRemoveBuff CreateContextActionRemoveBuff(BlueprintBuff buff)
        {
            var r = Helpers.Create<ContextActionRemoveBuff>();
            r.m_Buff = buff.ToReference<BlueprintBuffReference>();
            return r;
        }

        static public ContextActionRemoveBuffsByDescriptor CreateContextActionRemoveBuffsByDescriptor(SpellDescriptor descriptor, bool not_self = true)
        {
            var r = Helpers.Create<ContextActionRemoveBuffsByDescriptor>();
            r.SpellDescriptor = descriptor;
            r.NotSelf = not_self;
            return r;
        }

        #endregion


        #region |------------------------------------------------------/ COMPONENT CREATORS  /--------------------------------------------------------|

        public static AbilityAreaEffectRunAction CreateAreaEffectRunAction(GameAction unitEnter = null, GameAction unitExit = null, GameAction unitMove = null, GameAction round = null)
        {
            var a = Helpers.Create<AbilityAreaEffectRunAction>();
            a.UnitEnter = Helpers.CreateActionList(unitEnter);
            a.UnitExit = Helpers.CreateActionList(unitExit);
            a.UnitMove = Helpers.CreateActionList(unitMove);
            a.Round = Helpers.CreateActionList(round);
            return a;
        }



        public static AbilityAreaEffectRunAction CreateAreaEffectRunAction(GameAction[] unitEnter = null, GameAction[] unitExit = null, GameAction[] unitMove = null, GameAction[] round = null)
        {
            var a = Helpers.Create<AbilityAreaEffectRunAction>();
            a.UnitEnter = Helpers.CreateActionList(unitEnter);
            a.UnitExit = Helpers.CreateActionList(unitExit);
            a.UnitMove = Helpers.CreateActionList(unitMove);
            a.Round = Helpers.CreateActionList(round);
            return a;
        }

        // These SpawnFx are copied straight-away from CotW

        public static AbilitySpawnFx CreateAbilitySpawnFx(string asset_id, 
                                                          AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                          AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                          AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;

            return a;
        }

        public static AbilitySpawnFx CreateAbilitySpawnFxTime(string asset_id, AbilitySpawnFxTime time,
                                                              AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                              AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                              AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;
            a.Time = time;
            return a;
        }

        public static AbilitySpawnFx CreateAbilitySpawnFxDestroyOnCast(string asset_id, 
                                                                       AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                                       AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                                       AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;
            a.DestroyOnCast = true;
            return a;
        }

        #endregion

        #region  |------------------------------------------------------/ PREREQUISITE CREATORS  /--------------------------------------------------------|


        public static PrerequisiteNoFeature CreatePrerequisiteNoFeature(this BlueprintFeature feat, bool any = false)
        {
            var p = Helpers.Create<PrerequisiteNoFeature>();
            p.m_Feature = feat.ToReference<BlueprintFeatureReference>();
            p.Group = any ? Prerequisite.GroupType.Any : Prerequisite.GroupType.All;
            return p;
        }



        #endregion


        #region  |------------------------------------------------------/ CUSTOM PROPERTY CREATORS  /--------------------------------------------------------|



        public static CompositeCustomPropertyGetter CreateCompositeCustomPropertyGetter(CompositeCustomPropertyGetter.Mode mode, CompositeCustomPropertyGetter.ComplexCustomProperty[] properties)
        {
            var cpg = Helpers.Create<CompositeCustomPropertyGetter>();
            cpg.CalculationMode = mode;
            cpg.Properties = properties;
            return cpg;

        }

        public static CompositePropertyGetter CreateCompositePropertyGetter(CompositePropertyGetter.Mode mode, CompositePropertyGetter.ComplexProperty[] properties )
	    {
            var cpg = Helpers.Create<CompositePropertyGetter>();
            cpg.CalculationMode = mode;
            cpg.Properties = properties;
            return cpg;
        }

        public static CompositePropertyGetter.ComplexProperty CreateComplexProperty(UnitProperty property, int bonus = 0, float numerator = 1.0f, float denominator = 1.0f)
        {
            var cp = Helpers.Create<CompositePropertyGetter.ComplexProperty>();
            cp.Property = property;
            cp.Bonus = bonus;
            cp.Numerator = numerator;
            cp.Denominator = denominator;
            return cp;
        }

        public static CompositeCustomPropertyGetter.ComplexCustomProperty ComplexCustomProperty(PropertyValueGetter property, int bonus = 0, float numerator = 1.0f, float denominator = 1.0f)
        {
            var ccp = Helpers.Create<CompositeCustomPropertyGetter.ComplexCustomProperty>();
            ccp.Property = property;
            ccp.Bonus = bonus;
            ccp.Numerator = numerator;
            ccp.Denominator = denominator;
            return ccp;
        }

        public static CastingAttributeGetter CreateCastingAttributeGetter()
        {
            var ppv = Helpers.Create<CastingAttributeGetter>();
            return ppv;
        }


        public static CustomProgressionPropertyGetter CreateCustomProgressionPropertyGetter( UnitProperty property, int start = 1, int step = 1)
        {
            var ppv = Helpers.Create<CustomProgressionPropertyGetter>();
            ppv.Property = property;
            ppv.Start = start;
            ppv.Step = step;
            return ppv;
        }

        public static MaxAttributeBonusGetter CreateMaxAttributeBonusGetter()
        {
            var ppv = Helpers.Create<MaxAttributeBonusGetter>();
            return ppv;
        }

        public static MaxCastingAttributeGetter CreateMaxCastingAttributeGetter()
        {
            var ppv = Helpers.Create<MaxCastingAttributeGetter>();
            return ppv;
        }

        public static AnimalPetOwnerRankGetter CreateAnimalPetOwnerRankGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<AnimalPetOwnerRankGetter>();
            ppv.Property = property;
            return ppv;
        }

        public static ArcaneSpellFailureChanceGetter CreateArcaneSpellFailureChanceGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<ArcaneSpellFailureChanceGetter>();
            return ppv;
        }

        public static AreaCrComplexGetter CreateAreaCrComplexGetter(int bonus, int multiplier = 1, int denominator = 1)
        {
            var ppv = Helpers.Create<AreaCrComplexGetter>();
            ppv.Bonus = bonus;
            ppv.Multiplier = multiplier;
            ppv.Denominator = denominator;
            return ppv;
        }

        public static ClassLevelGetter CreateClassLevelGetter(BlueprintCharacterClassReference character_class, BlueprintArchetypeReference archetype = null)
        {
            var ppv = Helpers.Create<ClassLevelGetter>();
            ppv.m_Class = character_class;
            ppv.m_Archetype = archetype;
            return ppv;
        }

        public static CompanionsCountGetter CreateCompanionsCountGetter()
        {
            var ppv = Helpers.Create<CompanionsCountGetter>();
            return ppv;
        }

        public static FactRankGetter CreateFactRankGetter(BlueprintUnitFactReference fact)
        {
            var ppv = Helpers.Create<FactRankGetter>();
            ppv.m_Fact = fact;
            return ppv;
        }

        public static PropertyWithFactRankGetter CreatePropertyWithFactRankGetter(BlueprintUnitFactReference fact, int rank_multiplier = 1)
        {
            var ppv = Helpers.Create<PropertyWithFactRankGetter>();
            ppv.m_Fact = fact;
            ppv.m_RankMultiplier = rank_multiplier;
            return ppv;
        }

        public static ShieldBonusGetter CreateShieldBonusGetter()
        {
            var ppv = Helpers.Create<ShieldBonusGetter>();
            return ppv;
        }

        public static SimplePropertyGetter CreateSimplePropertyGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<SimplePropertyGetter>();
            ppv.Property = property;
            return ppv;
        }

        public static SkillRankGetter CreateSkillRankGetter(StatType skill)
        {
            var ppv = Helpers.Create<SkillRankGetter>();
            ppv.Skill = skill;
            return ppv;
        }

        public static SkillValueGetter CreateSkillValueGetter(StatType skill)
        {
            var ppv = Helpers.Create<SkillValueGetter>();
            ppv.Skill = skill;
            return ppv;
        }

        public static SpellLevelGetter CreateSpellLevelGetter(bool from_cast_rule)
        {
            var ppv = Helpers.Create<SpellLevelGetter>();
            ppv.FromCastRule = from_cast_rule;
            return ppv;
        }

        public static StatValueGetter CreateStatValueGetter(StatValueGetter.ReturnType type)
        {
            var ppv = Helpers.Create<StatValueGetter>();
            ppv.ValueType = type;
            return ppv;
        }


        public static SummClassLevelGetter CreateSummClassLevelGetter(BlueprintCharacterClassReference[] character_classes, BlueprintArchetypeReference[] archetypes = null)
        {
            var ppv = Helpers.Create<SummClassLevelGetter>();
            ppv.m_Class = character_classes;
            ppv.m_Archetypes = archetypes;
            return ppv;
        }

        public static UnitWeaponEnhancementGetter CreateUnitWeaponEnhancementGetter()
        {
            var ppv = Helpers.Create<UnitWeaponEnhancementGetter>();
            return ppv;
        }

        public static BaseAtackGetter CreateBaseAtackGetter()
        {
            var ppv = Helpers.Create<BaseAtackGetter>();
            return ppv;
        }

        public static BaseAttackPropertyWithFeatureList CreateBaseAttackPropertyWithFeatureList( int base_value, int base_attack_divisor, int base_attack_zero, int feature_bonus, BlueprintFeatureReference[] features  )
        {
            var ppv = Helpers.Create<BaseAttackPropertyWithFeatureList>();
            ppv.BaseValue = base_value;
            ppv.BaseAttackDiv = base_attack_divisor;
            ppv.BaseAttackZero = base_attack_zero;
            ppv.FeatureBonus = feature_bonus;
            ppv.m_Features = features;
            return ppv;
        }

        public static CurrentMeleeWeaponDamageStatGetter CreateCurrentMeleeWeaponDamageStatGetter()
        {
            var ppv = Helpers.Create<CurrentMeleeWeaponDamageStatGetter>();
            return ppv;
        }

        public static CurrentWeaponCriticalMultiplierGetter CreateCurrentWeaponCriticalMultiplierGetter()
        {
            var ppv = Helpers.Create<CurrentWeaponCriticalMultiplierGetter>();
            return ppv;
        }

        public static FightingDefensivelyACBonusProperty CreateFightingDefensivelyACBonusProperty()
        {
            var ppv = Helpers.Create<FightingDefensivelyACBonusProperty>();
            return ppv;
        }

        public static FightingDefensivelyAttackPenaltyProperty CreateFightingDefensivelyAttackPenaltyProperty()
        {
            var ppv = Helpers.Create<FightingDefensivelyAttackPenaltyProperty>();
            return ppv;
        }

        public static LevelBasedPropertyWithFeatureList CreateLevelBasedPropertyWithFeatureList(int base_value, int level_divisor, int level_zero, int feature_bonus, BlueprintFeatureReference[] features)
        {
            var ppv = Helpers.Create<LevelBasedPropertyWithFeatureList>();
            ppv.BaseValue = base_value;
            ppv.LevelDiv = level_divisor;
            ppv.LevelZero = level_zero;
            ppv.m_Features = features;
            ppv.FeatureBonus = feature_bonus;
            return ppv;
        }

        public static StatBonusIfHasFactProperty CreateStatBonusIfHasFactProperty(int multiplier, StatType stat, BlueprintUnitFactReference fact)
        {
            var ppv = Helpers.Create<StatBonusIfHasFactProperty>();
            ppv.Multiplier = multiplier;
            ppv.Stat = stat;
            ppv.m_RequiredFact = fact;
            return ppv;
        }


        #endregion



    }

}

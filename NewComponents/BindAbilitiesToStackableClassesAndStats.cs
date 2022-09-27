using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using TabletopTweaks.Core.Utilities;

namespace LegacyOfShadows.NewComponents
{
    // Thìs new component that is inspired by the BindAbilitiesToHighest and BindAbilitiesToClass which has the following additional features:
    // - Allow to set the Attribute associated with the selected abilities with a feature.
    // - Allow to set the classes and archetypes associated with the selected abilities with a feature.
    // Essentially works like this:
    // - A base class is set upon which all abilities are based.
    // - A list of features associated with specific classes is added.
    // - A list of features associated wth specific archetypes is added.
    // - A list of features associated with a specific attribute is added.
    // - The total of applicable class levels in the base class and any class added by a feature is used to calculate parameters.
    // - The attribute to use for parameter is defined by a feature.

    [AllowMultipleComponents]
    [ComponentName("Bind ability parameters to stacks of Classes and Archetypes and Attribute bonuses defined by certain features.")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("D7C7CC2870524FE8B83D7276AAF9F1AD")]
    public class BindAbilitiesToStackableClassesAndStats : UnitFactComponentDelegate, IRulebookHandler<RuleDispelMagic>, IRulebookHandler<RuleSpellResistanceCheck>, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, IInitiatorRulebookHandler<RuleDispelMagic>, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, IInitiatorRulebookSubscriber
    {

        public ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Abilites
        {
            get
            {
                return this.m_Abilites;
            }
        }

        public BlueprintCharacterClass CharacterClass
        {
            get
            {
                BlueprintCharacterClassReference characterClass = this.m_CharacterClass;
                if (characterClass == null)
                {
                    return null;
                }
                return characterClass.Get();
            }
        }

        public ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> StackableClasses
        {
            get
            {

                UnitDescriptor unit = this.m_EventInitializator;


                BlueprintCharacterClassReference[] stackableClasses = new BlueprintCharacterClassReference[0];

                foreach (var stckcls in m_StackableClasses)
                {

                    if ((stckcls.Key != null) && unit.HasFact(stckcls.Key))
                    {
                        if (stackableClasses.Length == 1)
                        {
                            stackableClasses[0] = stckcls.Value;

                        }
                        else
                        {
                            stackableClasses.AppendToArray(stckcls.Value);

                        }
                    }

                }

                if (stackableClasses[0] != null)
                {
                    return stackableClasses;
                }
                else
                {
                    return null;
                }
            }
        }

        public ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference> StackableArchetypes
        {
            get
            {

                UnitDescriptor unit = this.m_EventInitializator;


                BlueprintArchetypeReference[] stackableArchetypes = new BlueprintArchetypeReference[0];

                foreach (var stckarc in m_StackableArchetypes)
                {

                    if ((stckarc.Key != null) && unit.HasFact(stckarc.Key))
                    {
                        if (stackableArchetypes.Length == 1)
                        {
                            stackableArchetypes[0] = stckarc.Value;

                        }
                        else
                        {
                            stackableArchetypes.AppendToArray(stckarc.Value);

                        }
                    }

                }

                if (stackableArchetypes[0] != null)
                {
                    return stackableArchetypes;
                }
                else
                {
                    return null;
                }
            }
        }


        public StatType FindApplicableAttributeStat(UnitDescriptor unit)
        {


            var resulting_stat = this.DefaultStat;

            foreach (var st_fea in this.m_StatFeature)
            {

                if ((st_fea.Key != null) && unit.HasFact(st_fea.Key))
                {
                    return resulting_stat = st_fea.Value;
                }
            }

            return resulting_stat;

        }


        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {

            if (this.Abilites.Contains(evt.Spell))
            {
                int level = this.GetLevel(evt.Initiator.Descriptor);
                evt.ReplaceStat = new StatType?(this.FindApplicableAttributeStat(evt.Initiator.Descriptor));
                evt.ReplaceCasterLevel = new int?(this.GetLevelBase(level));
                evt.ReplaceSpellLevel = new int?(this.Cantrip ? 0 : Math.Min(level / 2, 10));            // While this system allows to stack for purpose of Caster Level, I have capped the spell level to 10th to avoid troubles with actual spells.
            }


        }


        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }


        public void OnEventAboutToTrigger(RuleDispelMagic evt)
        {
            IEnumerable<BlueprintAbility> source = this.Abilites;
            AbilityData ability = evt.Reason.Ability;
            if (source.Contains((ability != null) ? ability.Blueprint : null) && this.FullCasterChecks)
            {
                evt.Bonus += this.GetLevelDiff(evt.Initiator.Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleDispelMagic evt)
        {
        }


        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            if (this.Abilites.Contains(evt.Ability) && this.FullCasterChecks)
            {
                evt.AddSpellPenetration(this.GetLevelDiff(evt.Initiator.Descriptor), ModifierDescriptor.UntypedStackable);
            }
        }


        public void OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {
        }


        public int GetLevel(UnitDescriptor unit)
        {
            this.m_EventInitializator = unit;

            int CL = 0;

            CL = ReplaceCasterLevelOfAbility.CalculateClassLevel(this.CharacterClass, this.StackableClasses.ToArray<BlueprintCharacterClass>(), unit, this.StackableArchetypes.ToArray<BlueprintArchetype>());

            if (this.SetMinCasterLevel && (CL < this.m_MinCasterLevel))
            {
                return this.m_MinCasterLevel;
            }
            else
            {
                return CL;

            }

        }


        public int GetLevelBase(int level)
        {

            if ((int)(float)(level % 2) == 0)
            {
                this.Odd = false;
            }
            else
            {
                this.Odd = true;
            }
            return (level - (this.Odd ? 1 : 0)) / (this.Cantrip ? 1 : this.LevelStep);

        }


        public int GetLevelDiff(UnitDescriptor unit)
        {
            this.m_EventInitializator = unit;
            int level = this.GetLevel(unit);
            int levelBase = this.GetLevelBase(level);
            return Math.Max(level - levelBase, 0);


        }
















        [SerializeField]
        [FormerlySerializedAs("Abilites")]
        public BlueprintAbilityReference[] m_Abilites;

        [SerializeField]
        [FormerlySerializedAs("CharacterClass")]
        public BlueprintCharacterClassReference m_CharacterClass;

        public IDictionary<BlueprintFeatureReference, BlueprintCharacterClassReference> m_StackableClasses = new Dictionary<BlueprintFeatureReference, BlueprintCharacterClassReference>();

        public IDictionary<BlueprintFeatureReference, BlueprintArchetypeReference> m_StackableArchetypes = new Dictionary<BlueprintFeatureReference, BlueprintArchetypeReference>();

        private UnitDescriptor m_EventInitializator;

        public bool SetMinCasterLevel = false;

        public int m_MinCasterLevel = 1;

        public bool Cantrip;

        [HideIf("Cantrip")]
        public int LevelStep = 1;

        [HideIf("Cantrip")]
        public bool Odd;

        public StatType DefaultStat;

        public IDictionary<BlueprintFeatureReference, StatType> m_StatFeature = new Dictionary<BlueprintFeatureReference, StatType>();

        public bool FullCasterChecks = true;


    }
}

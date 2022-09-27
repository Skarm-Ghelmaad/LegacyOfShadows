using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes;
using System;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker.Utility;
using System.Collections.Generic;


namespace LegacyOfShadows.NewComponents
{
    // I like to create Gestalt characters with the Toybox, so it is fully possible that a character level could differ from the sum of all its class levels.
    // This is a special variant of IncreaseResourcesByClass which allows for:
    // - Calculation of a resource based on the highest between the character level (in a normal character) or the sum of all class levels (for a gestalt character).
    // - Exclude levels of specific classes from the calculation.
    // - Exclude levels of specific archetypes from the calculation.
    // - Count level of specific (partial) classes with a multiplier.
    // - Count level of specific (partial) archetypes with a multiplier.
    // - A more detailed calculation (not just by Level but also by LevelStartPlusDivStep, just like the normal resource calculation).
    // - A resource multiplier and a resource divider (to further tweak the calculation).
    // - The subtraction of the calculated amount (which allows to reduce a resource)

    [TypeId("28E39A6C16F142578132DD1A2BA81ED3")]
    public class IncreaseResourcesByGestaltCharacterLevel : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {

        public ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> ExcludedClass
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference>(this.m_ExcludedClass);
            }
        }

        public ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference> ExcludedArchetype
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference>(this.m_ExcludedArchetype);
            }
        }

        public ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> PartialClass
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference>(this.m_PartialClass);
            }
        }

        public ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference> PartialArchetype
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference>(this.m_PartialArchetype);
            }
        }


        public int CalculateTrueCharacterLevel()
        {

            UnitDescriptor unit = base.Owner;

            List<BlueprintCharacterClass> unitClasses = unit.Progression.ClassesOrder;

            List<BlueprintCharacterClass> mergedExcludedClasses = new List<BlueprintCharacterClass>();

            List<BlueprintCharacterClass> mergedPartialClasses = new List<BlueprintCharacterClass>();

            int vanillaCharacterLevel = unit.Progression.CharacterLevel;
            int gestaltCharacterLevel = 0;

            float final_Partial_Multiplier = (this.UsePartialMultiplier ? this.PartialMultiplier : 1.00f) / (this.UsePartialDivisor ? this.PartialDivisor : 1.00f);             

            if ((float)(Math.Abs(final_Partial_Multiplier)) > 1.00f)
            {
                final_Partial_Multiplier = 1.00f * Math.Sign(final_Partial_Multiplier);        // The Partial multiplier is supposed to be positive and between 0 and 1.00f as it is a replacement of the OtherClassesModifier in the BlueprintAbilityResource. A negative value is managed, but not supposed to be used.

            }


            if (this.ApplyClassExclusion == true)
            {
                mergedExcludedClasses = ExcludedClass.ToList();

                if (this.ApplyArchetypeExclusion == true)
                {
                    
                    foreach (BlueprintArchetype exclArchetype in this.ExcludedArchetype)
                    {
                        var parent_excluded_class = exclArchetype.m_ParentClass;

                        if ((parent_excluded_class != null) & (!mergedExcludedClasses.Any(parent_excluded_class)))
                        {
                            mergedExcludedClasses.Add(parent_excluded_class);
                        }

                    }

                }

            }
            else if ((this.ApplyClassExclusion == false) & (this.ApplyArchetypeExclusion == true))
            {
                if (ApplyArchetypeExclusion == true)
                {

                    foreach (BlueprintArchetype exclArchetype in this.ExcludedArchetype)
                    {
                        var parent_excluded_class = exclArchetype.m_ParentClass;

                        if ((parent_excluded_class != null) & (!mergedExcludedClasses.Any(parent_excluded_class)))
                        {
                                mergedExcludedClasses.Add(parent_excluded_class);

                        }


                    }

                }

            }


            if (this.UsePartialClasses == true)
            {
                mergedPartialClasses = PartialClass.ToList();

                if (this.UsePartialArchetypes == true)
                {

                    foreach (BlueprintArchetype partArchetype in this.PartialArchetype)
                    {
                        var parent_partial_class = partArchetype.m_ParentClass;

                        if ((parent_partial_class != null) & (!mergedPartialClasses.Any(parent_partial_class)))
                        {
                            mergedPartialClasses.Add(parent_partial_class);
                        }

                    }

                }

            }
            else if ((this.UsePartialClasses == false) & (this.UsePartialArchetypes == true))
            {
                if (this.UsePartialArchetypes == true)
                {

                    foreach (BlueprintArchetype partArchetype in this.PartialArchetype)
                    {
                        var parent_partial_class = partArchetype.m_ParentClass;

                        if ((parent_partial_class != null) & (!mergedPartialClasses.Any(parent_partial_class)))
                        {
                            mergedPartialClasses.Add(parent_partial_class);

                        }


                    }

                }

            }

            foreach (BlueprintCharacterClass unitCharacterClass in unitClasses)
            {
                bool flag1 = true;
                bool flag2 = true;


                if ((this.ApplyClassExclusion) | (this.ApplyArchetypeExclusion))
                {
                    if (mergedExcludedClasses.Any(unitCharacterClass))
                    {
                        flag1 = false;
                        vanillaCharacterLevel -= unit.Progression.GetClassLevel(unitCharacterClass);

                    }
                }

                if (ExcludeMythic)
                {
                    if (unitCharacterClass.IsMythic)
                    {
                        flag2 = false;

                    }
                }


                if (((this.UsePartialClasses) | (this.UsePartialArchetypes)) & (mergedExcludedClasses.Any(unitCharacterClass)))
                {
                    vanillaCharacterLevel -= ((flag1 && flag2) ? (int)((float)(unit.Progression.GetClassLevel(unitCharacterClass)) * ((1.00f * Math.Sign(final_Partial_Multiplier)) - final_Partial_Multiplier)) : 0);        // Here the goals is to manage both a positive and a negative multiplier.
                    gestaltCharacterLevel += ((flag1 && flag2) ? (int)((float)(unit.Progression.GetClassLevel(unitCharacterClass)) * final_Partial_Multiplier) : 0);

                }
                else
                {
                    gestaltCharacterLevel += ((flag1 && flag2) ? unit.Progression.GetClassLevel(unitCharacterClass) : 0);
                }

            }

            if (gestaltCharacterLevel > vanillaCharacterLevel)
            {
                return gestaltCharacterLevel;

            }
            else
            {
                return vanillaCharacterLevel;

            }


        }


        public int CalculateBonusAmount(int truecharacterlevel)
        {
            int num = 0;

            float final_Resource_Multiplier = (this.UseResourceMultiplier ? this.ResourceMultiplier : 1.00f) / (this.UseResourceDivisor ? this.ResourceDivisor : 1.00f);

            if (this.IncreasedByLevel)
            {
                num = (int)((float)(this.BaseValue + (truecharacterlevel * this.LevelIncrease)) * final_Resource_Multiplier);

                goto end_result;

            }
            else if (IncreasedByLevelStartPlusDivStep)
            {
                if (this.StartingLevel <= truecharacterlevel)
                {
                    if (this.LevelStep == 0)
                    {
                        PFLog.Default.Error("LevelStep is 0. Can't divide by 0", Array.Empty<object>());
                        goto end_result;

                    }
                    else
                    {
                        num += Math.Max((int)(((float)(this.StartingIncrease + this.PerStepIncrease * (truecharacterlevel - this.StartingLevel)) / (float)this.LevelStep) * final_Resource_Multiplier), this.MinClassLevelIncrease);
                        goto end_result;
                    }

                }

            }

            
        end_result:

            return num;


        }





        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {

            if (base.Fact.Active && resource == m_Resource.Get())
            {

                int trueCharacterLevel = CalculateTrueCharacterLevel();
                int resource_amount = CalculateBonusAmount(trueCharacterLevel);


                if (Subtract)
                {
                    bonus -= resource_amount;
                }
                else
                {
                    bonus += resource_amount;
                }
            }



        }


        // This component works as follows:
        // - Counts all class levels of classes and archetypes which aren't excluded or listed as partial to the gestalt character level calculation.
        // - Remove excluded classes and archetypes from the character class.
        // - Add partial classes and archetypes' levels multiplied by chosen multiplier and divider to the gestalt character level calculation.
        // - Remove the complement to 1 of the chosen multiplier and divider multiplied by partial classes and archetypes' levels from character class.
        // - Compare adjusted character level with gestalt character level and choose the highest option.
        // - Calculate the bonus with the chosen method and then multiplied by the chosen resource multiplier and divider.
        // - Either add or subtract to define if this component is an increase or a reduction.



        public BlueprintAbilityResourceReference m_Resource;

        public bool ApplyClassExclusion = false;

        public bool ApplyArchetypeExclusion = false;

        public bool UsePartialClasses = false;

        public bool UsePartialArchetypes = false;

        public bool ExcludeMythic = false;

        public bool UsePartialMultiplier = false;

        public bool UsePartialDivisor = false;

        public bool UseResourceMultiplier = false;

        public bool UseResourceDivisor = false;


        [UsedImplicitly]
        [ShowIf("ApplyClassExclusion")]
        public BlueprintCharacterClassReference[] m_ExcludedClass;

        [UsedImplicitly]
        [ShowIf("ApplyArchetypeExclusion")]
        public BlueprintArchetypeReference[] m_ExcludedArchetype;

        [UsedImplicitly]
        [ShowIf("UsePartialClasses")]
        public BlueprintCharacterClassReference[] m_PartialClass;

        [UsedImplicitly]
        [ShowIf("UsePartialArchetypes")]
        public BlueprintArchetypeReference[] m_PartialArchetype;

        [UsedImplicitly]
        public int BaseValue = 0;

        [UsedImplicitly]
        public bool IncreasedByLevel;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevel")]
        public int LevelIncrease;

        [UsedImplicitly]
        public bool IncreasedByLevelStartPlusDivStep;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int StartingLevel;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int StartingIncrease;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int LevelStep;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int PerStepIncrease;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int MinClassLevelIncrease;

        [UsedImplicitly]
        [ShowIf("UsePartialMultiplier")]
        public float PartialMultiplier = 1.00f;       // This is a multiplier used to adjust the levels from partial classes or archetypes.

        [UsedImplicitly]
        [ShowIf("UsePartialDivisor")]
        public float PartialDivisor = 1.00f;          // This is a divider used to adjust the levels from partial classes or archetypes.

        [UsedImplicitly]
        [ShowIf("UseResourceMultiplier")]
        public float ResourceMultiplier = 1.00f;      // This is a resource multiplier which is used to tweak the adjustment.

        [UsedImplicitly]
        [ShowIf("UseResourceDivisor")]
        public float ResourceDivisor = 1.00f;        // This is a resource divisor which is used to tweak the adjustment.

        public bool Subtract = false;



    }
}

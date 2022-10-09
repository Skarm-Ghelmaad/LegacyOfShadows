using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Properties;
using LegacyOfShadows.NewComponents.Properties;
using static TabletopTweaks.Core.NewComponents.Properties.CompositeCustomPropertyGetter;
using Kingmaker.UnitLogic.Mechanics.Properties;
using UnityEngine;

namespace LegacyOfShadows.NewComponents.Properties
{

    // The original plan was to make it a CompositeCustomPropertyGetter sub-class, but in the end I've made it into a new PropertyValueGetter sub-class.

    public class FactEnabledCompositeCustomPropertyGetter: PropertyValueGetter
    {
        public override int GetBaseValue(UnitEntityData unit)
        {
            switch (CalculationMode)
            {
                case Mode.Sum:
                    return Properties.Select(property => property.Calculate(unit)).Sum();
                case Mode.Highest:
                    return Properties.Select(property => property.Calculate(unit)).Max();
                case Mode.Lowest:
                    return Properties.Select(property => property.Calculate(unit)).Min();
                default:
                    return 0;
            }
        }

        public FactEnabledComplexCustomProperty[] Properties = new FactEnabledComplexCustomProperty[0];
        public Mode CalculationMode;

        public enum Mode : int
        {
            Sum,
            Highest,
            Lowest
        }


        public class FactEnabledComplexCustomProperty
        {
            public FactEnabledComplexCustomProperty() { }

            public int Calculate(UnitEntityData unit)
            {
                var iHasFact = 0;

                if (((unit.HasFact(this.m_CheckedFact) && (!this.Not))) || ((!unit.HasFact(this.m_CheckedFact)) && (this.Not)))
                {
                    iHasFact = 1;
                }

                return Bonus + Mathf.FloorToInt((Numerator / Denominator) * Property.GetBaseValue(unit)) * iHasFact;
            }

            public PropertyValueGetter Property;
            public BlueprintUnitFactReference m_CheckedFact;
            public int Bonus;
            public float Numerator = 1;
            public float Denominator = 1;
            public bool Not = false;
        }


        
    }
}

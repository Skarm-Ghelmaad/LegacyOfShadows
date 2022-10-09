using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Properties;
using UnityEngine;
using static TabletopTweaks.Core.NewComponents.Properties.CompositeCustomPropertyGetter;

namespace LegacyOfShadows.NewComponents.Properties
{
    public class FactEnabledCompositePropertyGetter : PropertyValueGetter
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

        public FactEnabledComplexProperty[] Properties = new FactEnabledComplexProperty[0];
        public Mode CalculationMode;

        public enum Mode : int
        {
            Sum,
            Highest,
            Lowest
        }

        public class FactEnabledComplexProperty
        {
            public FactEnabledComplexProperty() { }

            public int Calculate(UnitEntityData unit)
            {
                var iHasFact = 0;

                if (((unit.HasFact(this.m_CheckedFact) && (!this.Not))) || ((!unit.HasFact(this.m_CheckedFact)) && (this.Not)))
                {
                    iHasFact = 1;
                }

                return Bonus + Mathf.FloorToInt((Numerator / Denominator) * Property.GetInt(unit)) * iHasFact;
            }

            public UnitProperty Property;
            public BlueprintUnitFactReference m_CheckedFact;
            public int Bonus;
            public float Numerator = 1;
            public float Denominator = 1;
            public bool Not = false;
        }




    }
}

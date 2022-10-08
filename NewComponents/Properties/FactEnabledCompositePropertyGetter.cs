using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Properties;
using static TabletopTweaks.Core.NewComponents.Properties.CompositeCustomPropertyGetter;

namespace LegacyOfShadows.NewComponents.Properties
{
    public class FactEnabledCompositePropertyGetter : CompositePropertyGetter
    {
        public override int GetBaseValue(UnitEntityData unit)
        {
            switch (CalculationMode)
            {
                case Mode.Sum:
                    Properties = CheckProperties(unit);
                    return Properties.Select(property => property.Calculate(unit)).Sum();
                case Mode.Highest:
                    Properties = CheckProperties(unit);
                    return Properties.Select(property => property.Calculate(unit)).Max();
                case Mode.Lowest:
                    Properties = CheckProperties(unit);
                    return Properties.Select(property => property.Calculate(unit)).Min();
                default:
                    return 0;
            }
        }

        public ComplexProperty[] CheckProperties(UnitEntityData unit)
        {


            List<ComplexProperty> results_list = new List<ComplexProperty>();

            foreach (var cond_property in this.m_ConditionalProperties)
            {

                if ((cond_property.Key != null) && ((unit.HasFact(cond_property.Key) && (!Not)) || (!unit.HasFact(cond_property.Key) && (Not))))
                {
                    results_list.Add(cond_property.Value);
                }

            }

            return results_list.ToArray();
        }



        public IDictionary<BlueprintUnitFactReference, ComplexProperty> m_ConditionalProperties = new Dictionary<BlueprintUnitFactReference, ComplexProperty>();

        public bool Not = false;






    }
}

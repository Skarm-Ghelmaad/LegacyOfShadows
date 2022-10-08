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


namespace LegacyOfShadows.NewComponents.Properties
{


    public class FactEnabledCompositeCustomPropertyGetter: CompositeCustomPropertyGetter
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

        public ComplexCustomProperty[] CheckProperties(UnitEntityData unit)
        {


                List<ComplexCustomProperty> results_list = new List<ComplexCustomProperty>();

                    foreach (var cond_property in this.m_ConditionalProperties)
                    {

                        if ((cond_property.Key != null) && ((unit.HasFact(cond_property.Key) && (!Not)) || (!unit.HasFact(cond_property.Key) && (Not))))
                        {
                            results_list.Add(cond_property.Value);
                        }

                    }

                    return results_list.ToArray();
        }



        public IDictionary<BlueprintUnitFactReference, ComplexCustomProperty> m_ConditionalProperties = new Dictionary<BlueprintUnitFactReference, ComplexCustomProperty>();

        public bool Not = false;





    }
}

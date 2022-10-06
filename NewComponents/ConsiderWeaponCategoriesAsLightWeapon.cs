using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Parts;
using LegacyOfShadows.NewUnitParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewUnitParts;

namespace LegacyOfShadows.NewComponents
{
    public class ConsiderWeaponCategoriesAsLightWeapon: ConsiderWeaponCategoryAsLightWeapon
    {

        public WeaponCategory[] categories;

        public override void OnTurnOn()
        {
            base.OnTurnOn();
            
            var unit_part_damage_grace = base.Owner.Ensure<UnitPartDamageGrace>();

            for (int i = 0; i < categories.Length; i++)
            {
                Category = categories[i];

                if (!unit_part_damage_grace.HasEntry((WeaponCategory)Category))
                {
                    unit_part_damage_grace.AddEntry(Category, base.Fact);
                }

            }



        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();

            var unit_part_damage_grace = base.Owner.Ensure<UnitPartDamageGrace>();

            for (int i = 0; i < categories.Length; i++)
            {
                Category = categories[i];

                if (unit_part_damage_grace.HasEntry((WeaponCategory)Category))
                {
                    unit_part_damage_grace.RemoveEntry(base.Fact);
                }

            }


            base.Owner.Ensure<UnitPartDamageGrace>().RemoveEntry(base.Fact);
        }


    }
}

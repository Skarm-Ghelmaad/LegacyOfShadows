using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;

namespace LegacyOfShadows.NewComponents
{
    [AllowedOn(typeof(BlueprintAbility))]
    [AllowMultipleComponents]
    [TypeId("9DD22752846841CDB1A3AE6AC0DC21C3")]
    public class AbilityShowIfCasterKnowsSpell : BlueprintComponent, IAbilityVisibilityProvider
    {

        public bool IsAbilityVisible(AbilityData ability)
        {

            var spell_book = ability?.Caster?.GetSpellbook(this.Spellbook.Get());

            if (spell_book == null)
            {
                return this.Not;   

            }

            return spell_book.IsKnown(this.Spell) != this.Not;

        }


        [ValidateNotNull]
        [SerializeField]
        public BlueprintSpellbookReference Spellbook;
        public BlueprintAbilityReference Spell;
        public bool Not;
    }



}

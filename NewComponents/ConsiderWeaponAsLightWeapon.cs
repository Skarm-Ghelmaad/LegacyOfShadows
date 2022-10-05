using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core;
using TabletopTweaks.Core.NewUnitParts;
using static LegacyOfShadows.NewUnitParts.AdditionalUnitPartCustomMechanicsFeatures;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace LegacyOfShadows.NewComponents
{

    // This component is derived from Holic's ConsiderAsLightWeaponBase and Vek's DamageGraceTTT
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    abstract public class ConsiderWeaponAsLightWeapon : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        abstract public bool CanBeUsedOn(ItemEntityWeapon weapon);

        public override void OnTurnOn()
        {
            WeaponCategory? category = base.Param?.WeaponCategory ?? Category;
            base.Owner.CustomMechanicsFeature(CustomMechanicsFeature.UseWeaponAsLightWeapon);
        }

        public override void OnTurnOff()
        {
            base.Owner.Ensure<UnitPartDamageGrace>().RemoveEntry(base.Fact);
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Initiator.Body.SecondaryHand.HasShield)
            {
                ItemEntityShield maybeShield = evt.Initiator.Body.SecondaryHand.MaybeShield;
                if (maybeShield == null || maybeShield.Blueprint.Type.ProficiencyGroup != ArmorProficiencyGroup.Buckler)
                {
                    return;
                }
            }
            if (evt.Weapon.HoldInTwoHands) { return; }
            if (!evt.Initiator.Body.SecondaryHand.HasWeapon || evt.Initiator.Body.SecondaryHand.MaybeWeapon == evt.Initiator.Body.EmptyHandWeapon)
            {
                ModifiableValueAttributeStat dexterity = evt.Initiator.Descriptor.Stats.Dexterity;
                ModifiableValueAttributeStat modifiableValueAttributeStat = (evt.DamageBonusStat != null) ? (base.Owner.Descriptor.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat) : null;
                if (dexterity != null && (modifiableValueAttributeStat == null || dexterity.Bonus > modifiableValueAttributeStat.Bonus)
                    && evt.Weapon.Blueprint.Type.Category == (base.Param?.WeaponCategory ?? Category))
                {
                    evt.OverrideDamageBonusStat(StatType.Dexterity);
                }
                return;
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
        public WeaponCategory? Category;
    }




}

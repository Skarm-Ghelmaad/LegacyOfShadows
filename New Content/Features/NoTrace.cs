using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;


namespace LegacyOfShadows.New_Content.Features
{
    internal class NoTrace
    {
        public static void AddNoTrace() {

            
            private static readonly Sprite faststhSprie = BlueprintTools.GetBlueprint<BlueprintFeature>("97a6aa2b64dd21a4fac67658a91067d7").Icon;

        var NoTraceNinjaFeature = Helpers.CreateBlueprint<BlueprintFeature>(LoSContext, "NoTraceNinjaFeature", bp => {
            bp.SetName(LoSContext, "NoTraceNinjaFeature");
            bp.SetDescription(LoSContext, "You are skilled at the art of dispelling.\n" +
                "Whenever you attempt a dispel check based on your " +
                "caster level, you gain a +2 bonus on the check.");
            bp.m_Icon = faststhSprie;
            bp.IsClassFeature = true;
            bp.HideInUI = false;
            bp.Ranks = 6;
            bp.HideInCharacterSheetAndLevelUp = false;

        });





    }
    }
}

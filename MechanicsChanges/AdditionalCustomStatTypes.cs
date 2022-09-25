using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;

namespace LegacyOfShadows.MechanicsChanges
{
    public class AdditionalCustomStatTypes
    {
        // This class is used to add new custom stats that I need for my mod.


        public static class CustomStatTypes
        {
            //I have agreed with Vek17 to use only numbers over 11.000 for Stats and 1.001.000 for Attributes.
            public enum CustomStatType : int
            {
                Soul = 1_001_001,                           // This attribute is used with Universal Ki mechanics.
            }
  
        }


    }
}

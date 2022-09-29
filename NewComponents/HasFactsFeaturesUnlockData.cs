using System;
using Kingmaker.EntitySystem;
using Newtonsoft.Json;
using Kingmaker.Designers.Mechanics.Facts;

namespace LegacyOfShadows.NewComponents
{

    // This is the equivalent of UnitFactComponentDelegate<AddFeatureIfHasFactData> but works in blocks of facts (array) instead of individual facts.
    internal class HasFactsFeaturesUnlockData
    {
        public EntityFact[] AppliedFacts;
    }
}

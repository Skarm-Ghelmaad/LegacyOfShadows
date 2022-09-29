using System;
using Kingmaker.EntitySystem;
using Newtonsoft.Json;
using Kingmaker.Designers.Mechanics.Facts;


namespace LegacyOfShadows.NewComponents
{
    // This is the equivalent of UnitFactComponentDelegate<AddFeatureIfHasFactData>.
    public class ConditionalFactsFeaturesUnlockData
    {
        [JsonProperty]
        public EntityFact AppliedFact;
    }
}

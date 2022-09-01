using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Feature
    {
        public Feature()
        {
            SitterFeatures = new HashSet<SitterFeature>();
        }

        public int FeatureId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<SitterFeature> SitterFeatures { get; set; }
    }
}

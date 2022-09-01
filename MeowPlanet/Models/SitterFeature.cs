using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class SitterFeature
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; } = null!;
        public virtual Sitter Service { get; set; } = null!;
    }
}

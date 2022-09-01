using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Clue
    {
        public int ClueId { get; set; }
        public int MissingId { get; set; }
        public DateTime WitnessTime { get; set; }
        public decimal WitnessLat { get; set; }
        public decimal WitnessLng { get; set; }
        public int Status { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MemberId { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Missing Missing { get; set; } = null!;
    }
}

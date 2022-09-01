using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Adopt
    {
        public int MemberId { get; set; }
        public int CatId { get; set; }
        public int Owner { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateOver { get; set; }
        public int Status { get; set; }

        public virtual Cat Cat { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}

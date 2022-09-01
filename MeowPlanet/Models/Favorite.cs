using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Favorite
    {
        public int MemberId { get; set; }
        public int ServiceId { get; set; }
        public int Id { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Sitter Service { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Orderlist
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int ServiceId { get; set; }
        public int CatId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateOver { get; set; }
        public int Status { get; set; }
        public DateTime DateOrder { get; set; }
        public int Total { get; set; }
        public string? Comment { get; set; }
        public int? Star { get; set; }

        public virtual Cat Cat { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
        public virtual Sitter Service { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Sitter
    {
        public Sitter()
        {
            Favorites = new HashSet<Favorite>();
            Orderlists = new HashSet<Orderlist>();
            SitterFeatures = new HashSet<SitterFeature>();
        }

        public int ServiceId { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public int Pay { get; set; }
        public string Licence { get; set; } = null!;
        public string Cage { get; set; } = null!;
        public string Monitor { get; set; } = null!;
        public string Meal { get; set; } = null!;
        public string CatNumber { get; set; } = null!;
        public decimal PosLat { get; set; }
        public decimal PosLng { get; set; }
        public bool IsService { get; set; }
        public decimal AvgStar { get; set; }
        public string? Img01 { get; set; }
        public string? Img02 { get; set; }
        public string? Img03 { get; set; }
        public string? Img04 { get; set; }
        public string? Img05 { get; set; }
        public string? Area1 { get; set; }
        public string? Area2 { get; set; }
        public string? Area3 { get; set; }
        public string? FormattedAddress { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Orderlist> Orderlists { get; set; }
        public virtual ICollection<SitterFeature> SitterFeatures { get; set; }
    }
}

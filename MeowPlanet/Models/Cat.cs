using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Cat
    {
        public Cat()
        {
            Adopts = new HashSet<Adopt>();
            Missings = new HashSet<Missing>();
            Orderlists = new HashSet<Orderlist>();
        }

        public int CatId { get; set; }
        public int MemberId { get; set; }
        public int BreedId { get; set; }
        public bool IsSitting { get; set; }
        public bool IsMissing { get; set; }
        public bool IsAdoptable { get; set; }
        public string? Name { get; set; }
        public bool? Sex { get; set; }
        public int? Age { get; set; }
        public string? Introduce { get; set; }
        public string? Img01 { get; set; }
        public string? Img02 { get; set; }
        public string? Img03 { get; set; }
        public string? Img04 { get; set; }
        public string? Img05 { get; set; }
        public string? City { get; set; }

        public virtual CatBreed Breed { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<Adopt> Adopts { get; set; }
        public virtual ICollection<Missing> Missings { get; set; }
        public virtual ICollection<Orderlist> Orderlists { get; set; }
    }
}

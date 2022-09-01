using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class CatBreed
    {
        public CatBreed()
        {
            Cats = new HashSet<Cat>();
        }

        public int BreedId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Cat> Cats { get; set; }
    }
}

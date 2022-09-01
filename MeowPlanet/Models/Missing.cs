using System;
using System.Collections.Generic;

namespace MeowPlanet.Models
{
    public partial class Missing
    {
        public Missing()
        {
            Clues = new HashSet<Clue>();
        }

        public int MissingId { get; set; }
        public int CatId { get; set; }
        public DateTime Date { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Description { get; set; } = null!;
        public bool IsFound { get; set; }

        public virtual Cat Cat { get; set; } = null!;
        public virtual ICollection<Clue> Clues { get; set; }
    }
}

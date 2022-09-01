using System.ComponentModel.DataAnnotations;


namespace MeowPlanet.ViewModels.Missings
{
    public class ItemsViewModel
    {
        public int MissingId { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public bool? Sex { get; set; }
        public string? Breed { get; set; }
        public int ClueCount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime MissingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? UpdateDate { get; set; }

    }
}

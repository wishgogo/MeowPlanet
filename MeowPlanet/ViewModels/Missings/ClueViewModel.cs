using System.ComponentModel.DataAnnotations;

namespace MeowPlanet.ViewModels.Missings
{
    public class ClueViewModel
    {
        public int ClueId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime WitnessTime { get; set; }
        public int Status { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Distance { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = null!;
        public string ProviderPhoto { get; set; } = null!;

    }
}

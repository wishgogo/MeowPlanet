using System.ComponentModel.DataAnnotations;

namespace MeowPlanet.ViewModels.Missings
{
    public class DetailViewModel
    {
        public int? MissingId { get; set; }
        public string? Name { get; set; }
        public bool? Sex { get; set; }
        public int? Age { get; set; }
        public string? Breed { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public string? Img01 { get; set; }
        public string? Img02 { get; set; }
        public string? Img03 { get; set; }

        public string? Description { get; set; }
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? Photo { get; set; }

    }
}

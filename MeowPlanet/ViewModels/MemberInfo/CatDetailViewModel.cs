namespace MeowPlanet.ViewModels.MemberInfo
{
    public class CatDetailViewModel
    {
        public int MemberId { get; set; }
        public int CatId { get; set; }
        public int BreedId { get; set; }
        public string? Breed { get; set; }
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
    }
}

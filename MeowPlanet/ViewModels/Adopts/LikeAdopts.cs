namespace MeowPlanet.ViewModels
{
    public class LikeAdopts
    {
        public int CatId { get; set; }
        public int MemberId { get; set; }
        public string? CatName { get; set; }
        public bool? CatSex { get; set; }
        public int? CatAge { get; set; }
        public string? CatImg1 { get; set; }   
        public bool CatIsAdoptable { get; set; }
        public string BreedName { get; set; } = null!;
        public string? CatCity { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateOver { get; set; }
        public int Status { get; set; }
        public int Owner { get; set; }
        public string Name { get; set; } = null!;



    }
}

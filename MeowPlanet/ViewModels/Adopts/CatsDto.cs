namespace MeowPlanet.ViewModels.Adopts
{
    public class CatsDto
    {
        public int CatId { get; set; }
        public int BreedId { get; set; }
        public int MemberId { get; set; }
        public string? CatName { get; set; }
        public bool? CatSex { get; set; }
        public int? CatAge { get; set; }
        public string? CatIntroduce { get; set; }
        public string? CatImg1 { get; set; }
        public string? CatImg2 { get; set; }
        public string? CatImg3 { get; set; }
        public string? CatImg4 { get; set; }
        public string? CatImg5 { get; set; }
        public bool CatIsAdoptable { get; set; }
        public string BreedName { get; set; } = null!;
        public string? CatCity { get; set; }




    }
}

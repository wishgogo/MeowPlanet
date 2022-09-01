namespace MeowPlanet.ViewModels.Sitters
{
    public class SitterComfirmPayViewModels
    {
        //保姆本人資料
        public string SitterName { get; set; } = null!;
        public string? SitterPhoto { get; set; }
        //使用者本人資料
        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public string? UserPhoto { get; set; }
        //選擇貓咪資料
        public int CatId { get; set; }
        public bool? Sex { get; set; }
        public string? Introduce { get; set; }
        public string BreedName { get; set; } = null!;
        public string? CatName { get; set; }
        public string? CatImg01 { get; set; }
        //服務基本資料
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime DateStart { get; set; }
        //Wed Aug 03 2022 00:00:00 GMT+0800
        public DateTime DateOver { get; set; }
        public int Pay { get; set; }
        public int Night { get; set; }
        public int Total { get; set; }


    }
}

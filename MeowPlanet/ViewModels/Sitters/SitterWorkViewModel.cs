
namespace MeowPlanet.ViewModels.Sitters
{
    public class SitterWorkViewModel
    {

        //使用者本人資料
        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public string? UserPhoto { get; set; }
        //選擇貓咪資料
        public int OrderId { get; set; }
        public bool? Sex { get; set; }
        public string? Introduce { get; set; }
        public string BreedName { get; set; } = null!;
        public string? CatName { get; set; }
        public string? CatImg01 { get; set; }
        public DateTime DateStart { get; set; }
        //Wed Aug 03 2022 00:00:00 GMT+0800
        public DateTime DateOver { get; set; }
        public int Total { get; set; }
        public int Status { get; set; }







    }
}

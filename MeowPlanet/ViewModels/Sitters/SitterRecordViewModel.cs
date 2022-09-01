
namespace MeowPlanet.ViewModels.Sitters
{
    public class SitterRecordViewModel
    {

        //保姆本人資料

        //訂單ID
        public int OrderId { get; set; }
        //保姆姓名
        public string SitterName { get; set; } = null!;
        //保姆電話
        public string SitterPhone { get; set; } = null!;
        //保姆照片
        public string? SitterPhoto { get; set; }
        //保姆地址
        public string? FormattedAddress { get; set; }
        //訂單入宿開始日期
        public DateTime DateStart { get; set; }
        //Wed Aug 03 2022 00:00:00 GMT+0800
        //訂單入宿結束日期
        public DateTime DateOver { get; set; }
        //貓咪姓名
        public string? CatName { get; set; }
        //訂單費用
        public int Total { get; set; }
        //訂單狀態
        public int Status { get; set; }
    }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Mvc;
using MeowPlanet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using MeowPlanet.ViewModels.Sitters;


namespace MeowPlanet.Controllers
{
    public class SitterController : Controller
    {
        private readonly endtermContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int? _memberId;

        public SitterController(endtermContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;

            if (contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid) != null)
            {
                _memberId = Convert.ToInt32(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            }
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            //三個方法用中斷點看持續時間幾乎都差不多，大概2.5s~3.0s之間跳動
            //但單純看sql語法的秒數又差很多
            //方法一、二的sql 語法速度大概是70ms~90ms
            //方法三的sql 語法速度加總大概 280ms 以上
            //想要用forloop來測試 10圈的速度
            //發現第一圈花了2.X秒，其餘圈數都花<100ms左右而已
            //.AsNoTrackingWithIdentityResolution()
            //.Include(c => c.SitterFeatures)

            //方法一
            var result = await _context.Sitters
                .AsNoTracking()
                .Where(m => m.IsService == true)
                .Select(m => new SitterViewModels
                {
                    sitter = m,
                    memberPhoto = m.Member.Photo,
                    sitterfeatureList = m.SitterFeatures.Select(m => m.Feature.Name).ToList(),
                    OrderCommentList = m.Orderlists.ToList(),
                }).ToListAsync();

            //方法二
            //var result = new List<Sitter>();
            //result = await _context.Sitters
            //.Where(m => m.IsService == true).ToListAsync();

            //List<SitterViewModels> result2 = new List<SitterViewModels>();
            //foreach (var item in result)
            //{
            //    SitterViewModels x = new SitterViewModels()
            //    {
            //        sitter = item,
            //        memberPhoto = item.Member.Photo,
            //        sitterfeatureList = item.SitterFeatures.Select(c => c.Feature.Name).ToList(),
            //        OrderCommentList = item.Orderlists.ToList(),
            //    };
            //    result2.Add(x);
            //};

            //方法三
            //List<SitterViewModels> result = new List<SitterViewModels>();
            //var sitterList = _context.Sitters.Where(m => m.IsService == true).ToList();
            //foreach (var item in sitterList)
            //{
            //    SitterViewModels model = new SitterViewModels()
            //    {
            //        sitter = item,
            //        sitterfeatureList = _context.SitterFeatures.Include(m => m.Feature).Where(m => m.ServiceId == item.ServiceId).Select(x => x.Feature.Name).ToList(),
            //        OrderCommentList = _context.Orderlists.Where(m => m.ServiceId == item.ServiceId).ToList(),
            //        memberPhoto = _context.Members.FirstOrDefault(m => m.MemberId == item.MemberId).Photo,

            //    };
            //    result.Add(model);
            //}

            //TempData["controller"] = "Sitter";
            //TempData["action"] = "Index";

            return View(result);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var result = await _context.Sitters
                .AsNoTracking()
                //.AsNoTrackingWithIdentityResolution()
                .Where(m => m.ServiceId == id)
                .Select(m => new SitterViewModels
                {
                    sitter = m,
                    memberPhoto = m.Member.Photo,
                    sitterfeatureList = m.SitterFeatures.Select(m => m.Feature.Name).ToList(),
                    OrderCommentList = m.Orderlists.Where(m => m.Status == 2).OrderByDescending(m => m.DateOver).ToList(),
                    CommentOfUserList =m.Orderlists.Where(m => m.Status == 2).OrderByDescending(m => m.DateOver).Select(m => m.Member).ToList(),
                }).FirstOrDefaultAsync();


            var catList = await _context.Cats
                .Where(m => m.MemberId == _memberId)
                .Select(m => new Cat
                {
                    CatId = m.CatId,
                    BreedId = m.BreedId,
                    IsAdoptable = m.IsAdoptable,
                    IsMissing = m.IsMissing,
                    IsSitting = m.IsSitting,
                    Name = m.Name,
                    Sex = m.Sex,
                    Introduce = m.Introduce,
                    Img01 = m.Img01,
                }).ToListAsync();

            ViewBag.catList = catList;

            //TempData["controller"] = "Sitter";
            //TempData["action"] = "Detail";
            //TempData["ids"] = $"{id}";

            return View(result);
        }
        //方法一
        // 使用 變數 來接收submit資料
        [HttpPost]
        public ActionResult ComfirmPay(string startDate, string endDate, int night, int catId, int serviceId)
        {
            var user = _context.Members
                .Include(c => c.Cats.Where(c => c.CatId == catId))
                .ThenInclude(c => c.Breed)
                .Where(c => c.MemberId == _memberId)
                .FirstOrDefault();

            var sitter = _context.Sitters
                .Include(c => c.SitterFeatures)
                .Include(c => c.Member)
                .Where(c => c.ServiceId == serviceId).FirstOrDefault();

            SitterComfirmPayViewModels result = new SitterComfirmPayViewModels()
            {
                SitterName = sitter.Member.Name,
                SitterPhoto = sitter.Member.Photo,
                UserName = user.Name,
                UserPhone = user.Phone,
                UserPhoto = user.Photo,
                CatId = catId,
                Sex = user.Cats.FirstOrDefault().Sex,
                Introduce = user.Cats.FirstOrDefault().Introduce,
                BreedName = user.Cats.FirstOrDefault().Breed.Name,
                CatName = user.Cats.Where(c => c.CatId == catId).FirstOrDefault().Name,
                CatImg01 = user.Cats.Where(c => c.CatId == catId).FirstOrDefault().Img01,
                ServiceId = serviceId,
                ServiceName = sitter.Name,
                DateStart = DateTime.ParseExact(startDate, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture).Date,
                DateOver = DateTime.ParseExact(endDate, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture).Date,
                Pay = sitter.Pay,
                Night = night,
                Total = night * sitter.Pay,
            };
            return View(result);
        }
        //方法二
        // 使用 IFormCollection 來接收submit資料
        //[HttpPost]
        //public ActionResult ComfirmPay(IFormCollection form)
        //{
        //    string startDate = form["startDate"].ToString();
        //    string endDate = form["endDate"].ToString();
        //    //string endDate = form["__RequestVerificationToken"].ToString();

        //    ViewBag.start = startDate;
        //    ViewBag.end = endDate;
        //    return View();
        //}
        
        //送出訂單
        [HttpPost]
        public async Task<IActionResult> AddOrder(SitterComfirmPayViewModels sitterComfirmPay)
        {
            Orderlist orderlist = new Orderlist()
            {
                MemberId = (int)_memberId,
                ServiceId = sitterComfirmPay.ServiceId,
                CatId = sitterComfirmPay.CatId,
                DateStart = sitterComfirmPay.DateStart,
                DateOver = sitterComfirmPay.DateOver,
                DateOrder = DateTime.Now,
                Total = sitterComfirmPay.Total,
            };
            _context.Orderlists.Add(orderlist);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> SitterBox()
        {
            var sitter = await _context.Sitters
             .Where(x => x.MemberId == _memberId)
             .Select(x => new SitterViewModels
             {
                 sitter = x,
                 sitterfeatureList = x.SitterFeatures.Select(x => x.Feature.Name).ToList(),
             }).FirstOrDefaultAsync();
            return PartialView("_SitterBoxPartial",sitter);
        }

        [HttpGet]
        public  async Task<ActionResult> SitterViewMode()
        {
            var sitter = await _context.Sitters
            //要用theninclude 才看的到 feature
             //.Include(x => x.SitterFeatures)
             //.ThenInclude(x => x.Feature)
             //直接 include 看不到 feature
             //.Include(x => x.SitterFeatures.Feature)
             .Where(x => x.MemberId == _memberId)
             .Select( x => new SitterViewModels
             {
               sitter = x,
               //實際測試 我根本不用上面的include就可以取到我要資料了，這串語法自帶LEFT JOIN 跟 INNER JOIN
               sitterfeatureList = x.SitterFeatures.Select(x => x.Feature.Name).ToList(),
             }).FirstOrDefaultAsync();
 
            return PartialView("_SitterViewModePartial",sitter);
        }

        [HttpPost]
        public ActionResult SitterEditMode([FromBody]SitterViewModels j)
        {
            return PartialView("_SitterEditModePartial", j);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSitter(SitterViewModels j, List<string> check, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5)
        {
            var sitterDB = await _context.Sitters
             .Where(x => x.MemberId == _memberId)
             .FirstOrDefaultAsync();

            //sitter update
            Random random = new Random();
            string? uniqueFileName = null;

            if (file1 != null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + file1.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file1.CopyTo(fileStream);
                }
                sitterDB.Img01 = "/images/userUpload/" + uniqueFileName;
            }

            if (file2 != null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + file2.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file2.CopyTo(fileStream);
                }
                sitterDB.Img02 = "/images/userUpload/" + uniqueFileName;
            }

            if (file3 != null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + file3.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file3.CopyTo(fileStream);
                }
                sitterDB.Img03 = "/images/userUpload/" + uniqueFileName;
            }

            if (file4 != null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + file4.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file4.CopyTo(fileStream);
                }
                sitterDB.Img04 = "/images/userUpload/" + uniqueFileName;
            }

            if (file5 != null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + file5.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file5.CopyTo(fileStream);
                }
                sitterDB.Img05 = "/images/userUpload/" + uniqueFileName;
            }
            
            sitterDB.Name = j.sitter.Name;
            sitterDB.Summary = j.sitter.Summary;
            sitterDB.Pay = j.sitter.Pay;
            sitterDB.Licence = j.sitter.Licence;
            sitterDB.Cage = j.sitter.Cage;
            sitterDB.Monitor = j.sitter.Monitor;
            sitterDB.Meal = j.sitter.Meal;
            sitterDB.CatNumber = j.sitter.CatNumber;
            sitterDB.PosLat = j.sitter.PosLat;
            sitterDB.PosLng = j.sitter.PosLng;
            sitterDB.Area1 = j.sitter.Area1;
            sitterDB.Area2 = j.sitter.Area2;
            sitterDB.Area3 = j.sitter.Area3;
            sitterDB.FormattedAddress = j.sitter.FormattedAddress;
            sitterDB.IsService = j.sitter.IsService;


            //sitter feature update
            var sitterfeatureDB = await _context.SitterFeatures
             .Where(x => x.ServiceId == sitterDB.ServiceId)
             .ToListAsync();

            List<string> sitterFeaturesIdDB = new List<string>();
            for (int i = 0; i < sitterfeatureDB.Count; i++)
            {
                sitterFeaturesIdDB.Add(sitterfeatureDB[i].FeatureId.ToString());
            };

            var update = check.Except(sitterFeaturesIdDB).ToList();
            var delete = sitterFeaturesIdDB.Except(check).ToList();

            for (int i = 0; i < update.Count; i++)
            {
                Int32.TryParse(update[i] ,out int x);
                await _context.SitterFeatures.AddAsync(new SitterFeature() { ServiceId = sitterDB.ServiceId,FeatureId=x});
            }
            for (int i = 0; i < delete.Count; i++)
            {
                int.TryParse(delete[i], out int d);
                _context.SitterFeatures.Remove(sitterfeatureDB.FirstOrDefault(x => x.FeatureId == d));
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        public async Task<IActionResult> SitterWorkManage()
        {
            var result = await _context.Orderlists.Where(x => x.Service.MemberId == _memberId)
                .Select(x => new SitterWorkViewModel
                {
                    UserName = x.Member.Name,
                    UserPhone = x.Member.Phone,
                    UserPhoto = x.Member.Photo,
                    OrderId = x.OrderId,
                    Sex = x.Cat.Sex,
                    Introduce = x.Cat.Introduce,
                    BreedName = x.Cat.Breed.Name,
                    CatName = x.Cat.Name,
                    CatImg01 = x.Cat.Img01,
                    DateStart = x.DateStart,
                    DateOver = x.DateOver,
                    Total = x.Total,
                    Status = x.Status
                }).ToListAsync();

            return PartialView("_SitterWorkManagePartial", result);
        }

        public async Task<IActionResult> SitterReserveRecord()
        {
            var result1 = await _context.Orderlists.Where(x => x.Member.MemberId == _memberId)
                .Select(x => new SitterRecordViewModel
                {
                    OrderId = x.OrderId,
                    SitterName = x.Service.Name,
                    SitterPhone = x.Service.Member.Phone,
                    SitterPhoto = x.Service.Member.Photo,
                    FormattedAddress = x.Service.FormattedAddress,
                    DateStart = x.DateStart,
                    DateOver = x.DateOver,
                    CatName = x.Cat.Name,
                    Total = x.Total,
                    Status = x.Status
                }).ToListAsync();

            return PartialView("_SitterReserveRecordPartial", result1);
        }

        [HttpPost]
        public async Task<IActionResult> SetOrderStatus(int orderId, int status)
        {
            var order = await _context.Orderlists.FirstOrDefaultAsync(x => x.OrderId == orderId);
            order.Status = status;
            await _context.SaveChangesAsync();

            return Content("OK");
        }

        [HttpGet]
        public async Task<IActionResult> SetOrderStatus(int orderId, int status, int div12, string comment)
        {
            //訂單星星寫入
            var order = await _context.Orderlists.FirstOrDefaultAsync(x => x.OrderId == orderId);
            order.Status = status;
            order.Star = div12;
            order.Comment = comment;
            
            //星星平均計算
            var allOrder = _context.Orderlists.Where(x => x.ServiceId == order.ServiceId && x.Status == 2).ToList();
            var avg = Convert.ToDecimal(allOrder.Sum(x => x.Star)+order.Star)/Convert.ToDecimal(allOrder.Count()+1);
            var sitter = _context.Sitters.Where(x => x.ServiceId == order.ServiceId).FirstOrDefault();
            sitter.AvgStar = avg;
            await _context.SaveChangesAsync();

            return Content("OK");
        }

        

    }
}

using MeowPlanet.Data;
using MeowPlanet.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MeowPlanet.ViewModels.MemberInfo;
using System.Collections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MeowPlanet.Controllers
{
    public class MemberController : Controller
    {
        private readonly endtermContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MemberController(endtermContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.FindFirstValue(ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var LoginId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
                var LoginInfo = _context.Members.FirstOrDefault(p => p.MemberId == LoginId);
                return View(LoginInfo);
            }
        }

        public IActionResult CreateCat()
        {
            if (User.FindFirstValue(ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();

            }
        }

        public IActionResult CreateSitter()
        {
            if (User.FindFirstValue(ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();

            }
        }

        // 回應MyAccountPartial
        [HttpGet]
        public ActionResult GetAccountInfo(int MemberId)
        {
            var MemberInfo = _context.Members.FirstOrDefault(p => p.MemberId == MemberId);

            return PartialView("_MyAccountPartial", MemberInfo);
        }

        // 建立貓咪資料
        [HttpPost]
        public async Task<IActionResult> AddCat(Cat cat, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5)
        {
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
                cat.Img01 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img02 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img03 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img04 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img05 = "/images/userUpload/" + uniqueFileName;
            }

            _context.Cats.Add(cat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 建立保姆資料
        [HttpPost]
        public async Task<IActionResult> AddSitter(Sitter sitter, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5, string sitterfeature)
        {
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
                sitter.Img01 = "/images/userUpload/" + uniqueFileName;
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
                sitter.Img02 = "/images/userUpload/" + uniqueFileName;
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
                sitter.Img03 = "/images/userUpload/" + uniqueFileName;
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
                sitter.Img04 = "/images/userUpload/" + uniqueFileName;
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
                sitter.Img05 = "/images/userUpload/" + uniqueFileName;
            }
            _context.Sitters.Add(sitter);
            await _context.SaveChangesAsync();

            if (sitterfeature != null)
            {
                string[] sitterfeatureArray = sitterfeature.Split(',');

                var sitterInfo = _context.Sitters.Where(x => x.MemberId == sitter.MemberId).OrderBy(x => x.ServiceId).LastOrDefault();

                var serviceid = sitterInfo.ServiceId;


                for (int i = 0; i < sitterfeatureArray.Length; i++)
                {

                    SitterFeature sitter_feature = new SitterFeature();
                    sitter_feature.ServiceId = serviceid;
                    sitter_feature.FeatureId = Convert.ToInt32(sitterfeatureArray[i]);
                    _context.SitterFeatures.Add(sitter_feature);

                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Member");
        }

        // 儲存sitterfeature資料
        [HttpPost]
        public async Task<IActionResult> AddSitterFeature(string sitterfeature, int memberId)
        {
            if (sitterfeature != null)
            {
                string[] sitterfeatureArray = sitterfeature.Split(',');

                var sitterInfo = _context.Sitters.Where(x => x.MemberId == memberId).OrderBy(x => x.ServiceId).LastOrDefault();

                var serviceid = sitterInfo.ServiceId;


                for (int i = 0; i < sitterfeatureArray.Length; i++)
                {

                    SitterFeature sitter_feature = new SitterFeature();
                    sitter_feature.ServiceId = serviceid;
                    sitter_feature.FeatureId = Convert.ToInt32(sitterfeatureArray[i]);
                    _context.SitterFeatures.Add(sitter_feature);

                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Member");
        }

        // 取得選擇貓咪資料
        [HttpGet]
        public ActionResult GetCatInfo()
        {
            var LoginId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);

            var CatCount = _context.Cats.Count(x => x.MemberId == LoginId);

            if (CatCount > 0)
            {
                var CatInfo = _context.Cats.Where(x => x.MemberId == LoginId).Join(_context.CatBreeds,
                    c => c.BreedId,
                    s => s.BreedId,
                    (c, s) => new CatSelectViewModel
                    {
                        CatId = c.CatId,
                        Breed = s.Name,
                        Name = c.Name,
                        Sex = c.Sex,
                        Img01 = c.Img01

                    }).ToList();

                return PartialView("_CatSelectPartial", CatInfo);

            }
            else
            {
                return Content("nocat");
            }
        }

        // 取得貓咪詳細資料
        [HttpGet]
        public ActionResult GetCatDetail(int CatId)
        {
            var LoginId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);

            var CatDetail = _context.Cats.Where(x => x.CatId == CatId).Join(_context.CatBreeds,
                c => c.BreedId,
                s => s.BreedId,
                (c, s) => new CatDetailViewModel
                {
                    MemberId = LoginId,
                    CatId = c.CatId,
                    BreedId = c.BreedId,
                    Name = c.Name,
                    Breed = s.Name,
                    Sex = c.Sex,
                    Age = c.Age,
                    Introduce = c.Introduce,
                    Img01 = c.Img01,
                    Img02 = c.Img02,
                    Img03 = c.Img03,
                    Img04 = c.Img04,
                    Img05 = c.Img05,
                    City = c.City,
                    IsAdoptable = c.IsAdoptable,
                    IsMissing = c.IsMissing,
                    IsSitting = c.IsSitting

                }).FirstOrDefault();

            return PartialView("_CatDetailPartial", CatDetail);
        }

        // 修改貓咪資料
        [HttpPost]
        public async Task<IActionResult> UpdateCat(Cat cat, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5)
        {
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
                cat.Img01 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img02 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img03 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img04 = "/images/userUpload/" + uniqueFileName;
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
                cat.Img05 = "/images/userUpload/" + uniqueFileName;
            }

            var oldCat = _context.Cats.FirstOrDefault(p => p.CatId == cat.CatId);

            oldCat.Name = cat.Name;
            oldCat.Age = cat.Age;
            oldCat.BreedId = cat.BreedId;
            oldCat.Sex = cat.Sex;
            oldCat.City = cat.City;
            oldCat.Introduce = cat.Introduce;

            if ((oldCat.IsMissing != true) || (oldCat.IsSitting != true))
            {
                oldCat.IsAdoptable = cat.IsAdoptable;
            }

            if (cat.Img01 != null)
            {
                oldCat.Img01 = cat.Img01;
            }
            if (cat.Img02 != null)
            {
                oldCat.Img02 = cat.Img02;
            }
            if (cat.Img03 != null)
            {
                oldCat.Img03 = cat.Img03;
            }
            if (cat.Img04 != null)
            {
                oldCat.Img04 = cat.Img04;
            }
            if (cat.Img05 != null)
            {
                oldCat.Img05 = cat.Img05;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 刪除貓咪
        [HttpGet]
        public async Task<IActionResult> DeleteCat(int CatId)
        {
            var Cat = _context.Cats.FirstOrDefault(p => p.CatId == CatId);

            _context.Cats.Remove(Cat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 修改會員資料
        [HttpPost]
        public async Task<IActionResult> UpdateMember(Member member)
        {
            var oldMember = _context.Members.FirstOrDefault(p => p.MemberId == member.MemberId);

            oldMember.Password = member.Password;
            oldMember.Name = member.Name;
            oldMember.Phone = member.Phone;


            //更新暱稱
            var identity = new ClaimsIdentity(User.Identity);
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
            identity.AddClaim(new Claim(ClaimTypes.Name, member.Name));

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                        new ClaimsPrincipal(identity));

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 修改會員頭像
        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(IFormFile memberPhoto, int memberId)
        {
            var oldMember = _context.Members.FirstOrDefault(p => p.MemberId == memberId);

            Random random = new Random();
            string? uniqueFileName = null;

            if (memberPhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/userUpload");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + memberPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    memberPhoto.CopyTo(fileStream);
                }
                oldMember.Photo = "/images/userUpload/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

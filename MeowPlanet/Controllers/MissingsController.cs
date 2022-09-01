using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeowPlanet.ViewModels.Missings;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace MeowPlanet.Models
{
    public class MissingsController : Controller
    {
        private readonly endtermContext _context;
        private readonly int? _memberId;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MissingsController(endtermContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
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
            var itemList = await _context.ItemsViewModels.FromSqlRaw("SELECT missing.missing_id AS MissingId, sex AS Sex, img_01 AS Image, cat.name AS Name, date AS MissingDate, cat_breed.name AS Breed, COUNT(CASE WHEN clue.status = 1 THEN 1 ELSE NULL END) AS ClueCount, MAX(CASE WHEN status = 1 THEN witness_time ELSE NULL END) AS UpdateDate FROM missing INNER JOIN cat ON cat.cat_id = missing.cat_id INNER JOIN cat_breed ON cat.breed_id = cat_breed.breed_id LEFT JOIN clue ON missing.missing_id = clue.missing_id WHERE is_found = 0 GROUP BY missing.missing_id, sex, img_01, cat.name, date, cat_breed.name").ToListAsync();

            return View(itemList);
        }


        [HttpPost]
        public async Task<IActionResult> AddMissing(Missing missingCat)
        {
            _context.Missings.Add(missingCat);
            _context.Cats.FirstOrDefault(x => x.CatId == missingCat.CatId).IsMissing = true;
            _context.Cats.FirstOrDefault(x => x.CatId == missingCat.CatId).IsAdoptable = false;

            await _context.SaveChangesAsync();
            return Content("OK");
        }

        [HttpGet]
        public async Task<IActionResult> GetMissing()
        {

            var catList = await _context.Missings.Where(x => x.IsFound == false).ToListAsync();


            return Json(catList);
        }




        public async Task<IActionResult> GetDetail(int missingId)
        {
            var result = await _context.Missings
                .Where(x => x.MissingId == missingId)
                .Include(x => x.Cat)
                .Include(x => x.Cat.Member)
                .Include(x => x.Cat.Breed)
                .Select(x => new DetailViewModel()
                {
                    MissingId = missingId,
                    Name = x.Cat.Name,
                    Sex = x.Cat.Sex,
                    Age = x.Cat.Age,
                    Breed = x.Cat.Breed.Name,
                    Date = x.Date,
                    Img01 = x.Cat.Img01,
                    Img02 = x.Cat.Img02,
                    Img03 = x.Cat.Img03,
                    Description = x.Description,
                    MemberId = x.Cat.Member.MemberId,
                    MemberName = x.Cat.Member.Name,
                    Photo = x.Cat.Member.Photo
                })
                .FirstOrDefaultAsync();

            return PartialView("_MissingDetailPartial", result);
        }

        public async Task<IActionResult> GetPublish()
        {
            List<Cat> cats = await _context.Cats.Where(x => x.MemberId == _memberId).ToListAsync();
            ViewData["cats"] = cats;

            return PartialView("_MissingPublishPartial");
        }

        public async Task<IActionResult> PostClue(decimal WitnessLat, decimal WitnessLng, IFormFile Image, DateTime WitnessTime, string Description, int MissingId)
        {
            var clue = new Clue
            {
                MissingId = MissingId,
                WitnessLat = WitnessLat,
                WitnessLng = WitnessLng,
                WitnessTime = WitnessTime,
                Description = Description,
                MemberId = (int)_memberId
            };

            Random random = new Random();
            string? uniqueFileName;

            if (Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/clue");
                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
                clue.ImagePath = "/images/clue/" + uniqueFileName;
            }

            await _context.Clues.AddAsync(clue);
            await _context.SaveChangesAsync();

            return Content("OK");
        }

        [HttpGet]
        public async Task<IActionResult> GetClues()
        {
            var missingCat = await _context.Cats.FirstOrDefaultAsync(x => x.MemberId == _memberId && x.IsMissing == true);

            if (missingCat != null)
            {
                ViewBag.CatName = missingCat.Name;

                return PartialView("_MissingCluePartial");
            }
            else
            {
                return Content("False");
            }

        }

        public IActionResult GetCluesComponent(int memberId, int status)
        {
            return ViewComponent("MissingsManage", new { memberId, status });
        }

        [HttpPost]
        public async Task<IActionResult> EditClue(int clueId, int newStatus)
        {
            string respond = newStatus == 1 ? "已釘選此線索" : "已移除此線索";

            var clue = await _context.Clues.FirstOrDefaultAsync(x => x.ClueId == clueId);
            clue.Status = newStatus;
            await _context.SaveChangesAsync();

            return Content(respond);
        }

        [HttpGet]
        public async Task<IActionResult> CheckCats()
        {
            var catCount = await _context.Cats.CountAsync(x => x.MemberId == _memberId);
            var missingCount = await _context.Cats.CountAsync(x => x.MemberId == _memberId && x.IsMissing == true);

            if (catCount == 0)
            {
                return Content("0"); //名下沒有任何貓咪
            }
            else if (missingCount > 0)
            {
                return Content("1"); //名下已有走失貓咪
            }
            else
            {
                return Content("2"); //符合刊登條件
            }

        }

        [HttpPost]
        public async Task<IActionResult> CatFound()
        {
            var missingCat = _context.Cats.FirstOrDefault(x => x.MemberId == _memberId && x.IsMissing == true);
            var missingCatId = missingCat.CatId;

            missingCat.IsMissing = false;
            _context.Missings.FirstOrDefault(x => x.CatId == missingCatId && x.IsFound == false).IsFound = true;

            await _context.SaveChangesAsync();

            return Content("OK");
        }

        [HttpGet]
        public async Task<IActionResult> SetCluesMarker(int missingId)
        {
            var result = await _context.Clues.Where(x => x.MissingId == missingId && x.Status == 1).OrderBy(x => x.WitnessTime).ToListAsync();

            return Json(result);
        }

    }
}

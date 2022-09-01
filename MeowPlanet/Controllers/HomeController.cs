using MeowPlanet.Models;
using MeowPlanet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MeowPlanet.Controllers
{
    public class HomeController : Controller
    {
        private readonly endtermContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, endtermContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            var orderInfo = _context.Orderlists.Where(x => x.Star == 5).Join(_context.Members,
                o => o.MemberId,
                m => m.MemberId,
                (o,m) => new OrderCommentViewModel
                {
                    Comment = o.Comment,
                    Photo = m.Photo
                }).ToList();

            return View(orderInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
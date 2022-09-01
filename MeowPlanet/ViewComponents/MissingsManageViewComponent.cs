using Microsoft.AspNetCore.Mvc;
using MeowPlanet.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GeoCoordinatePortable;
using MeowPlanet.ViewModels.Missings;

namespace MeowPlanet.ViewComponents
{
    public class MissingsManageViewComponent : ViewComponent
    {
        private readonly endtermContext _context;

        public MissingsManageViewComponent(endtermContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int memberId, int status)
        {
            ViewBag.Status = status;

            var result = _context.Clues.Include(c => c.Missing)
                .Include(c => c.Missing.Cat)
                .Where(c => c.Missing.Cat.MemberId == memberId && c.Missing.Cat.IsMissing == true)
                .Where(c => c.Missing.CatId == c.Missing.Cat.CatId && c.Missing.IsFound == false)
                .Where(c => c.Missing.MissingId == c.MissingId && c.Status == status)
                .Select(c => new ClueViewModel
                {
                    ClueId = c.ClueId,
                    WitnessTime = c.WitnessTime,
                    Status = c.Status,
                    ImagePath = c.ImagePath,
                    Description = c.Description,
                    ProviderId = c.MemberId,
                    ProviderName = c.Member.Name,
                    ProviderPhoto = c.Member.Photo,
                    Distance = (int)new GeoCoordinate((double)c.WitnessLat, (double)c.WitnessLng).GetDistanceTo(new GeoCoordinate((double)c.Missing.Lat, (double)c.Missing.Lng))
                })
                .ToList();


            return View(result);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MeowPlanet.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GeoCoordinatePortable;
using MeowPlanet.ViewModels;

namespace MeowPlanet.ViewComponents
{
    public class PrivateMessageViewComponent : ViewComponent
    {

        private readonly endtermContext _context;

        public PrivateMessageViewComponent(endtermContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int memberId)
        {

            var contactMembers = await _context.Messages
                .Where(x => x.ReceivedId == memberId || x.SendId == memberId)
                .Select(x => new { x.Send, x.Received })
                .Select(x => new ContactMembers
                {
                    Id = x.Send.MemberId == memberId ? x.Received.MemberId : x.Send.MemberId,
                    Name = x.Send.MemberId == memberId ? x.Received.Name : x.Send.Name,
                    Photo = x.Send.MemberId == memberId ? x.Received.Photo : x.Send.Photo,
                    LastTime = _context.Messages.Where(m => (m.SendId == (x.Send.MemberId == memberId ? x.Received.MemberId : x.Send.MemberId) && m.ReceivedId == memberId) || (m.SendId == memberId && m.ReceivedId == (x.Send.MemberId == memberId ? x.Received.MemberId : x.Send.MemberId))).Max(m => m.Time),
                    UnreadCount = _context.Messages.Count(m => m.IsRead == false && (m.SendId == (x.Send.MemberId == memberId ? x.Received.MemberId : x.Send.MemberId) && m.ReceivedId == memberId))
                })
                .Distinct()
                .OrderByDescending(x=> x.LastTime)
                .ToListAsync();
                

            var result = await _context.Members.Where(m => m.MemberId == memberId).Select( x => new MessageBoxModel
            {
                UserName = x.Name,
                UserPhoto = x.Photo,
                HasUnread = _context.Messages.Where(x => x.ReceivedId == memberId).Select(x => x.IsRead).Contains(false),
                ContactMembers = contactMembers

            }).FirstOrDefaultAsync();



            return View(result);
        }        

    }
}

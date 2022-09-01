using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeowPlanet.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        internal IEnumerable<object> AspNetUsers;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
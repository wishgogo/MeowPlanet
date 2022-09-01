using MeowPlanet.ViewModels.Missings;
using Microsoft.EntityFrameworkCore;

namespace MeowPlanet.Models
{
    public partial class endtermContext : DbContext
    {

        public virtual DbSet<ItemsViewModel> ItemsViewModels { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemsViewModel>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}

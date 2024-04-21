using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BoardingHouseApp.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<KostData> KostData { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<BookingData> BookingDates { get; set; }
    }
}

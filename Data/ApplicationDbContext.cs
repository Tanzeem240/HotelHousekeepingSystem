using Microsoft.EntityFrameworkCore;
using HotelHousekeepingSystem.Models;

namespace HotelHousekeepingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CleaningTask> CleaningTasks { get; set; }
    }
}
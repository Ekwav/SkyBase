using Microsoft.EntityFrameworkCore;

namespace Coflnet.Sky.SkyAuctionTracker.Models
{
    /// <summary>
    /// <see cref="DbContext"/> For flip tracking
    /// </summary>
    public class TrackerDbContext : DbContext
    {
        public DbSet<FlipEvent> FlipEvents { get; set; }
        public DbSet<Flip> Flips { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="TrackerDbContext"/>
        /// </summary>
        /// <param name="options"></param>
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
        : base(options)
        {
        }
    }
}
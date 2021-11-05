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

        /// <summary>
        /// Configures additional relations and indexes
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FlipEvent>(entity =>
            {
                entity.HasIndex(e => new { e.AuctionUUID, e.FlipEventType });
                entity.HasIndex(e => e.PlayerUUID);
            });

            modelBuilder.Entity<Flip>(entity =>
            {
                entity.HasIndex(e => new { e.AuctionUUID });
            });
        }
    }
}
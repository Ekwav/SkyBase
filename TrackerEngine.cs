using System.Threading;
using System.Threading.Tasks;
using Coflnet.Sky.SkyAuctionTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coflnet.Sky.SkyAuctionTracker
{

    public class TrackerEngine : BackgroundService
    {
        private IServiceScopeFactory scopeFactory;

        public TrackerEngine(
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrackerDbContext>();
            // make sure all migrations are applied
            await context.Database.MigrateAsync();
        }
    }
}
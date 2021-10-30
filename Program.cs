using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Coflnet.Sky.SkyAuctionTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //SubscribeEngine.Instance.LoadFromDb();
            //RunIsolatedForever(SubscribeEngine.Instance.ProcessQueues, "SubscribeEngine");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

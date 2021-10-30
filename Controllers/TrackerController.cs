using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coflnet.Sky.SkyAuctionTracker.Models;
using System;

namespace Coflnet.Sky.SkyAuctionTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly TrackerDbContext db;

        public TrackerController(TrackerDbContext context)
        {
            db = context;
        }

        //TODO: Gleiche Klasse für hier und in den Command-Klassen verwenden (NewFlipTrackingModel & TrackFliipEventModel)

        [HttpPost]
        [Route("newFlip")]
        public async Task<Flip> trackFlip([FromBody] Flip flip)
        {
            db.Flips.Add(flip);
            await db.SaveChangesAsync();
            return flip;
        }

        [HttpPost]
        [Route("trackFlipEvent")]
        public async Task<FlipEvent> trackFlipEvent(FlipEvent flipEvent)
        {
            db.FlipEvents.Add(flipEvent);
            await db.SaveChangesAsync();
            return flipEvent;
        }
    }
}

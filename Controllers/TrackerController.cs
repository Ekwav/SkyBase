using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coflnet.Sky.SkyAuctionTracker.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        [Route("newFlip/{auctionUUID}")]
        public async Task<Flip> trackFlip([FromBody] Flip flip, string auctionUUID)
        {

            flip.AuctionUUID = GetId(auctionUUID);

            var flipAlreadyExists = await db.Flips.Where(f => f.AuctionUUID == flip.AuctionUUID && f.FinderType == flip.FinderType).AnyAsync();
            if (flipAlreadyExists)
            {
                return flip;
            }
            db.Flips.Add(flip);
            await db.SaveChangesAsync();
            return flip;
        }

        [HttpPost]
        [Route("trackFlipEvent/{auctionUUID}")]
        public async Task<FlipEvent> trackFlipEvent(FlipEvent flipEvent, string auctionUUID)
        {

            flipEvent.AuctionUUID = GetId(auctionUUID);

            var flipEventAlreadyExists = await db.FlipEvents.Where(f => f.AuctionUUID == flipEvent.AuctionUUID && f.FlipEventType == flipEvent.FlipEventType && f.PlayerUUID == flipEvent.PlayerUUID).AnyAsync();
            if (flipEventAlreadyExists)
            {
                return flipEvent;
            }
            db.FlipEvents.Add(flipEvent);
            await db.SaveChangesAsync();
            return flipEvent;
        }

        public long GetId(string uuid)
        {
            if (uuid.Length > 17)
                uuid = uuid.Substring(0, 17);
            var builder = new System.Text.StringBuilder(uuid);
            builder.Remove(12, 1);
            builder.Remove(16, uuid.Length - 17);
            var id = Convert.ToInt64(builder.ToString(), 16);
            if (id == 0)
                id = 1; // allow uId == 0 to be false if not calculated
            return id;
        }
    }
}

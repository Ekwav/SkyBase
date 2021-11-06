using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coflnet.Sky.SkyAuctionTracker.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

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
            if (flip.Timestamp == default)
            {
                flip.Timestamp = DateTime.Now;
            }
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
        [Route("flipEvent/{auctionUUID}")]
        public async Task<FlipEvent> trackFlipEvent(FlipEvent flipEvent, string auctionUUID)
        {
            if (flipEvent.Timestamp == default)
            {
                flipEvent.Timestamp = DateTime.Now;
            }
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

        [HttpGet]
        [Route("lastFlipTime")]
        public async Task<DateTime> getLastFlipTime()
        {

            var flipEventAlreadyExists = await db.FlipEvents.OrderByDescending(f => f.Id).FirstOrDefaultAsync();
            if (flipEventAlreadyExists == null)
            {
                return DateTime.UnixEpoch;
            }
            return flipEventAlreadyExists.Timestamp;
        }

        [HttpGet]
        [Route("flipRecieveTimes")]
        public async Task<double> getFlipRecieveTimes(int number = 100)
        {

            var flips = await db.Flips.OrderByDescending(f => f.Id).Take(number).ToListAsync();

            double sum = 0;
            flips.ForEach(flip =>
            {
                sum += db.FlipEvents.Where(f => f.AuctionUUID == flip.AuctionUUID && f.FlipEventType == FlipEventType.FLIP_RECEIVE).Average(f => (f.Timestamp - flip.Timestamp).TotalSeconds);
            });
            return sum / flips.Count();
        }

        [HttpGet]
        [Route("flipBuyingTimeForPlayer")]
        public async Task<double> getFlipBuyingTimeForPlayer(long playerUUID, int number = 50)
        {

            var purchaseConfirmEvents = await db.FlipEvents.Where(flipEvent => flipEvent.PlayerUUID == playerUUID && flipEvent.FlipEventType == FlipEventType.PURCHASE_CONFIRM).OrderByDescending(f => f.Id).Take(number).ToListAsync();

            double sum = 0;
            purchaseConfirmEvents.ForEach(purchaseEvent =>
            {
                var recieveEvent = db.FlipEvents.Where(f => f.AuctionUUID == purchaseEvent.AuctionUUID && f.PlayerUUID == purchaseEvent.PlayerUUID && f.FlipEventType == FlipEventType.FLIP_RECEIVE).SingleOrDefault();
                sum += (purchaseEvent.Timestamp - recieveEvent.Timestamp).TotalSeconds;
            });
            return sum / purchaseConfirmEvents.Count();
        }

        [HttpGet]
        [Route("flipsBoughtBeforeFound")]
        public async Task<List<Flip>> getFlipsBoughtBeforeFound(long playerUUID)
        {

            var purchaseConfirmEvents = await db.FlipEvents.Where(flipEvent => flipEvent.PlayerUUID == playerUUID && flipEvent.FlipEventType == FlipEventType.AUCTION_SOLD).ToListAsync();

            var flips = new List<Flip>();
            purchaseConfirmEvents.ForEach(purchaseEvent =>
            {
                var flip = db.Flips.SingleOrDefault(f => f.AuctionUUID == purchaseEvent.AuctionUUID);
                if (flip != null && flip.Timestamp > purchaseEvent.Timestamp)
                {
                    flips.Add(flip);
                }
            });
            return flips;
        }

        [HttpGet]
        [Route("flipsForAuction")]
        public async Task<List<Flip>> getFlipsOfAuction(long auctionUUid)
        {

            var flips = await db.Flips.Where(flip => flip.AuctionUUID == auctionUUid).ToListAsync();
            return flips;
        }

        [HttpGet]
        [Route("numberOfActiveFlipperUsers")]
        public async Task<int> getNumberOfActiveFlipperUsers()
        {

            return await db.FlipEvents.Where(flipEvent => flipEvent.FlipEventType == FlipEventType.FLIP_RECEIVE && DateTime.Now.Subtract(flipEvent.Timestamp).TotalMinutes > 3).GroupBy(flipEvent => flipEvent.PlayerUUID).CountAsync();
        }

        [HttpGet]
        [Route("getOutspeedTime")]
        public async Task<ValueTuple<long, double>> getOutspeedTime(long auctionUUID, long playerUUID)
        {
            var flipClickEvent = db.FlipEvents.Where(flip => flip.AuctionUUID == auctionUUID && flip.FlipEventType == FlipEventType.FLIP_CLICK && flip.PlayerUUID == playerUUID).SingleOrDefault();
            var flipSoldEvent = db.FlipEvents.Where(flip => flip.AuctionUUID == auctionUUID && flip.FlipEventType == FlipEventType.AUCTION_SOLD).SingleOrDefault();

            return new ValueTuple<long, double>(flipSoldEvent.PlayerUUID, (flipSoldEvent.Timestamp - flipClickEvent.Timestamp).TotalSeconds);
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

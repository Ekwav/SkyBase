
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Coflnet.Sky.SkyAuctionTracker.Models
{
    [DataContract]
    public class FlipEvent
    {
        [IgnoreDataMember]
        [JsonIgnore]
        public int Id { get; set; }
        [DataMember(Name = "playerUUID")]
        public string PlayerUUID { get; set; }
        [DataMember(Name = "auctionUUID")]
        public int AuctionUUID { get; set; }
        [DataMember(Name = "flipTrackerEvent")]
        public FlipTrackerEvent FlipTrackerEvent { get; set; }
        [System.ComponentModel.DataAnnotations.Timestamp]
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public enum FlipTrackerEvent
    {
        PURCHASE_START = 1,
        PURCHASE_CONFIRM = 2,
        FLIP_RECEIVE = 4,
        FLIP_CLICK = 8,
        AUCTION_SOLD = 16
    }
}
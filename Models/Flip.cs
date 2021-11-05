
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Coflnet.Sky.SkyAuctionTracker.Models
{
    [DataContract]
    public class Flip
    {
        [IgnoreDataMember]
        [JsonIgnore]
        public int Id { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public long AuctionUUID { get; set; }
        [DataMember(Name = "targetPrice")]
        public int TargetPrice { get; set; }
        [DataMember(Name = "finderType")]
        public FinderType FinderType { get; set; }
        [System.ComponentModel.DataAnnotations.Timestamp]
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public enum FinderType
    {
        FLIPPER = 1,
        LOWEST_BIN = 2,
        SNIPER = 4,
        AI = 8
    }
}

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

        [DataMember(Name = "auctionUUID")]
        public string AuctionUUID { get; set; }
        [DataMember(Name = "targetPrice")]
        public int TargetPrice { get; set; }
        [DataMember(Name = "finderType")]
        public string FinderType { get; set; }
        [System.ComponentModel.DataAnnotations.Timestamp]
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
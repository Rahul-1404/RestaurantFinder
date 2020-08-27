using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantFinder.Models
{

    public class RestaurantRecord
    {
        [JsonProperty("restaurant")]
        public Restaurant Restaurant { get; set; }
    }
    public class Restaurant
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("cuisines")]
        public string Cuisines { get; set; }

        [JsonProperty("average_cost_for_two")]
        public int AverageCostForTwo { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("reviews")]

        public string Reviews { get; set; }

        [JsonProperty("phonenumber")]

        public string PhoneNumber { get; set; }

        public double DistanceFromUser { get; set; }

    }
}

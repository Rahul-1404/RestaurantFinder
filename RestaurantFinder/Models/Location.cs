using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantFinder.Models
{
    public class Location
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city_id")]
        public string City_id { get; set; }

        [JsonProperty("locality_verbose")]
        public string LocalityVerbose { get; set; }

        [JsonProperty("latitude")]
        public string latitude { get; set; }

        [JsonProperty("longitude")]
        public string longitude { get; set; }

        [JsonProperty("location_suggestions")]
        public List<location_suggestions> location_suggestions { get; set; }



    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantFinder.Models
{
    public class location_suggestions
    {
        [JsonProperty("entity_type")]
        public string entity_type { get; set; }

        [JsonProperty("entity_id")]
        public string entity_id { get; set; }
    }
}

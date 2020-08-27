using Nest;
using RestaurantFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantFinder.ViewModels
{
    public class SearchViewModel
    {
        public RestaurantRecord[] restaurants { get; set; }

        public string SearchQuery { get; set; }

        public int Radius { get; set; }

        public string Sort { get; set; }

        public string Order { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
    }
}

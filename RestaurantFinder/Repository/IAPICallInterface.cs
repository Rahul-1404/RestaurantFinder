using RestaurantFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestaurantFinder.Repository
{
   public interface IAPICallInterface
    {
        public  HttpClient GetHttpClient(string url);

        public Task<T> GetAsync<T>(string url, string urlParameters);

        public Task<T> RunAsync<T>(string url, string urlParameters);

        public RestaurantRecord [] ShowRestaurants(string entity_id, string entity_type, string SearchQuery);

        public RestaurantRecord[] ShowRelevantRestaurants(string SearchQuery);

        public Restaurant GetRestaurantDetails(int id);

        public string GetAddress(string latitude, string longitude);

        public location_suggestions GetLocationIndex(string state, string country, string lat, string lon);

        public double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude);

    }
}

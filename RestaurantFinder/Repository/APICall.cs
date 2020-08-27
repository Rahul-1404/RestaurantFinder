using Newtonsoft.Json;
using RestaurantFinder.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RestaurantFinder.Repository
{
    public class APICall : IAPICallInterface
    {
        const string url = "https://developers.zomato.com/api/v2.1/";
        const string apiKey = "52085c3e5ec7c74684c5a8aae4bc1d86";
        public static string lati = "";
        public static string longi = "";
        public static string ntity_id = "";
        public static string ntity_type = "";

        private readonly List<string> UserlocationDeatils;

        public APICall()
        {
            UserlocationDeatils = new List<string>();
        }

        public HttpClient GetHttpClient(string url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<T> GetAsync<T>(string url, string urlParameters)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    HttpResponseMessage response = await client.GetAsync(urlParameters);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public async Task<T> RunAsync<T>(string url, string urlParameters)
        {
            return await GetAsync<T>(url, urlParameters);
        }

        public RestaurantList GetRestaurants(string entity_id , string entity_type , int start , string SearchQuery)
        {
            string urlParameters = "";
            if (urlParameters == "")
            {
                urlParameters = $"search?start={start}&entity_id={entity_id}&entity_type={entity_type}&apikey={apiKey}";
            }
            else
                urlParameters = $"search?q={SearchQuery}&start={start}&entity_id={entity_id}&entity_type={entity_type}&apikey={apiKey}";
            var response = RunAsync<RestaurantList>(url, urlParameters).GetAwaiter().GetResult();
            return response;
        }

        public RestaurantRecord[] ShowRestaurants(string entity_id , string entity_type , string SearchQuery)
        {
            ntity_id = entity_id;
            ntity_type = entity_type;
            UserlocationDeatils.Add(entity_id);
            UserlocationDeatils.Add(entity_type);
            int[] Starts = { 0, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400 };
            var myList = new List<RestaurantRecord>();
            foreach (var i in Starts)
            {
                var response = GetRestaurants(entity_id, entity_type , i , SearchQuery);
                if (response != null)
                {
                    var restaurants = response.Restaurants;

                    myList.AddRange(restaurants);
                }

            }

            return myList.ToArray();
           
        }

        public RestaurantRecord[] ShowRelevantRestaurants(string SearchQuery)
        {           
            var list = ShowRestaurants(ntity_id,ntity_type, SearchQuery);
            if (SearchQuery != null && SearchQuery != " ")
            {
                List<RestaurantRecord> restos = new List<RestaurantRecord>();

                foreach (var i in list)
                {
                    if (i.Restaurant.Name.ToLower().Contains(SearchQuery.ToLower()))

                    {
                        restos.Add(i);
                    }
                }

                return restos.ToArray();
            }

            return list;
        }


        public Restaurant GetRestaurantDetails(int id)
        {
            string urlParameters = $"restaurant?res_id={id}&apikey={apiKey}";
            var response = RunAsync<Restaurant>(url, urlParameters).GetAwaiter().GetResult();
            return response;
        }

        public string GetAddress(string latitude, string longitude)
        {
            if(latitude == null || longitude == null)
            {
                //Setting the default value to New York , if the user does'nt grant permission to access their location
                latitude = "40.634825299999996";
                longitude = "-74.04175130000002";
            }
            lati = latitude;
            longi = longitude;
            UserlocationDeatils.Add(latitude);
            UserlocationDeatils.Add(longitude);
            string locationName = "";
            string key = "AIzaSyDlGs-ygBzx6RN6YcAOpKYb0Lw7V2c4nkg";
            string url = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key={2}", latitude, longitude,key);
            XElement xml = XElement.Load(url);
            if (xml.Element("status").Value == "OK")
            {
                locationName = string.Format("{0}",
                    xml.Element("result").Element("formatted_address").Value);
            }
            return locationName;

        }
        public location_suggestions GetLocationIndex(string state, string country , string lat , string lon)
        {
            if (lat == null || lon == null)
            {
                //Setting the default value to New York , if the user does'nt grant permission to access their location
                lat = "40.634825299999996";
                lon = "-74.04175130000002";
            }
            string urlParameters = $"locations?radius=1000&count=100&query={state}%20{country}&lat={lat}&lon={lon}&apikey={"52085c3e5ec7c74684c5a8aae4bc1d86"}"; 
             var response = RunAsync<Location>(url, urlParameters).GetAwaiter().GetResult();
            return response.location_suggestions[0];
             
        }

        public double Calculate(double lat1, double lon1, double lat2, double lon2)
        {
            lat1 = double.Parse(lati, System.Globalization.CultureInfo.InvariantCulture);
            lon1 = double.Parse(longi, System.Globalization.CultureInfo.InvariantCulture);
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344;
        }
    }
}

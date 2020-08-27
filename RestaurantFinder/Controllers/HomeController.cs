using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestaurantFinder.Models;
using RestaurantFinder.Repository;
using RestaurantFinder.ViewModels;

namespace RestaurantFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAPICallInterface _api;

        private RestaurantRecord[] restaurantsList = { };

        public string lati = "";
        public string longi = "";

        public HomeController(ILogger<HomeController> logger , IAPICallInterface api)
        {
            _logger = logger;
            _api = api;
        }

        public IActionResult Index()
        {        
          // var restaurants =  _api.ShowRestaurants();
           var data = _api.GetRestaurantDetails(18812348);
           return View();
        }

        [HttpPost]
        public IActionResult Index(string latitude , string longitude)
        {

           lati = latitude;
           longi = longitude;

           return RedirectToAction("ShowRestaurants", new { lat = latitude, longi = longitude });

        }

        [HttpGet]
        public IActionResult ShowRestaurants(string lat, string longi)
        {
            NumberFormatInfo formatProvider = new NumberFormatInfo();
            formatProvider.NumberDecimalSeparator = ", ";
            formatProvider.NumberGroupSeparator = ".";
            formatProvider.NumberGroupSizes = new int[] { 2 };
            var location = _api.GetAddress(lat, longi);
            var loc = location.Split(',');
            var State = loc[loc.Length - 2];
            var country = loc[loc.Length - 1];
            var state_zip = State.Split(' ');
            var locationindex = _api.GetLocationIndex(state_zip[1], country, lat, longi);
            var restaurants = _api.ShowRestaurants(locationindex.entity_id, locationindex.entity_type , " ");
            restaurantsList = restaurants;
            List<Location> points = new List<Location>();
            foreach(var i in restaurants)
            {
                points.Add(i.Restaurant.Location);
            }
            ViewData["points"] = points;

            SearchViewModel view = new SearchViewModel();
            view.restaurants = restaurants;
            return View(view);
        }

        public IActionResult RestaurantDetails(int ID)  
        {
            //double.Parse(data.Location.latitude, System.Globalization.CultureInfo.InvariantCulture), double.Parse(data.Location.longitude, System.Globalization.CultureInfo.InvariantCulture)
            var data = _api.GetRestaurantDetails(ID);
            var distance = _api.Calculate(0, 0, double.Parse(data.Location.latitude, System.Globalization.CultureInfo.InvariantCulture), double.Parse(data.Location.longitude, System.Globalization.CultureInfo.InvariantCulture));
            data.DistanceFromUser = distance;
            return View(data);
        }

        [HttpPost]
        public IActionResult ShowRestaurants(string SearchQuery , string latitude , string longitude)
        {
            var restaurants = _api.ShowRelevantRestaurants(SearchQuery);
            restaurantsList = restaurants;
            List<Location> points = new List<Location>();
            ViewData["points"] = points;
            SearchViewModel view = new SearchViewModel();
            view.restaurants = restaurants;
            return View(view);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

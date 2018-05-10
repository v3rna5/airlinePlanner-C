using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AirplanePlanner.Models;

namespace AirplanePlanner.Controllers
{
    public class CitiesController : Controller
    {

      [HttpGet("/cities")]
       public ActionResult Index()
       {
           List<City> allCities = City.GetAll();
           return View(allCities);
       }
       [HttpGet("/cities/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/cities")]
        public ActionResult Create()
        {
            City newCity = new City(Request.Form["city-name"]);
            newCity.Save();
            return RedirectToAction("Success", "Home");
        }


       // 
       // [HttpPost("/cities/new")]
       // public ActionResult Create()
       // {
       //     City newCity = new City(Request.Form["city-name"]);
       //     newCity.Save();
       //     List<City> allCities = City.GetAll();
       //     return View("Index", allCities);
       // }

      [HttpGet("/cities/{id}")]
      public ActionResult Details(int id)
      {
         Dictionary<string, object> model = new Dictionary<string, object>();
         City selectedCity = City.Find(id);
         List<Flight> cityFlights = selectedCity.GetFlights();
         List<Flight> allFlights = Flight.GetAll();
         model.Add("selectedCity", selectedCity);
         model.Add("cityFlights", cityFlights);
         model.Add("allFlights", allFlights);
         return View(model);
      }

      //  [HttpPost("/items")]
      //  public ActionResult CreateItem()
      //  {
      //    Dictionary<string, object> model = new Dictionary<string, object>();
      //    Category foundCategory = Category.Find(Int32.Parse(Request.Form["category-id"]));
      //    Item newItem = new Item(Request.Form["item-description"]);
      //   //  newItem.SetDate(Request.Form["item-due"]);
      //    newItem.Save();
      //    foundCategory.AddItem(newItem);
      //    List<Item> categoryItems = foundCategory.GetItems();
      //    model.Add("items", categoryItems);
      //    model.Add("category", foundCategory);
      //    return View("Details", model);
      //  }
      //

       [HttpPost("/cities/{cityId}/flights/new")]
        public ActionResult AddFlight(int cityId)
        {
            City city = City.Find(cityId);
            Flight flight = Flight.Find(Int32.Parse(Request.Form["flight-id"]));
            city.AddFlight(flight);
            return RedirectToAction("Details",  new { id = cityId });
        }
      //  [HttpPost("/items/{id}/update")]
      //  public ActionResult Update(int id)
      //  {
      //      Item thisItem = Item.Find(id);
      //      thisItem.Edit(Request.Form["newname"]);
      //      return RedirectToAction("Details");
      //  }
    }
}

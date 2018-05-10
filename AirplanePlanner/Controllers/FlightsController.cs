using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AirplanePlanner.Models;
using System;

namespace AirplanePlanner.Controllers
{
    public class FlightsController : Controller
    {
       [HttpGet("/flights")]
       public ActionResult Index()
       {
           List<Flight> allFlights = Flight.GetAll();
           return View(allFlights);
       }

       [HttpGet("/flights/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/fights")]
        public ActionResult Create()
        {
            Flight newFlight = new Flight(Request.Form["flight-description"]);
            newFlight.Save();
            return RedirectToAction("Success", "Home");
        }

       [HttpGet("/cities/{cityId}/flights/new")]
       public ActionResult CreateForm(int cityId)
       {
          Dictionary<string, object> model = new Dictionary<string, object>();
          City city = City.Find(cityId);
          return View(city);
       }

       [HttpGet("/flights/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Flight selectedFlight = Flight.Find(id);
            List<City> flightCities = selectedFlight.GetCities();
            List<City> allCities = City.GetAll();
            model.Add("selectedFlight", selectedFlight);
            model.Add("flightCities", flightCities);
            model.Add("allCities", allCities);
            return View(model);

        }

       [HttpGet("/cities/{cityId}/flights/{GetFlightsId}")]
       public ActionResult Details(int cityId, int flightId)
       {
          Flight flight = Flight.Find(flightId);
          Dictionary<string, object> model = new Dictionary<string, object>();
          City city = City.Find(cityId);
          model.Add("flight", flight);
          model.Add("city", city);
          return View(flight);
       }

      [HttpPost("/flights/{flightId}/cities/new")]
        public ActionResult AddCity(int flightId)
        {
            Flight flight = Flight.Find(flightId);
            City city = City.Find(Int32.Parse(Request.Form["AddCity-id"]));
            flight.AddCity(city);
            return RedirectToAction("Details",  new { id = flightId });
        }
       [HttpGet("/flights/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Flight thisFlight = Flight.Find(id);
            return View(thisFlight);
        }

        [HttpPost("/cities/{id}/delete")]
         public ActionResult DeleteFlight(int id)
         {
             Flight.Delete(id);
             return RedirectToAction("Details", "Cities", new { id = id });
         }
    }
}

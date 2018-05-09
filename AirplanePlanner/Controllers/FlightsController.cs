using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [HttpPost("/flights")]
        public ActionResult Create()
        {
            Flight newFlight = new Flight(Request.Form["flight-city"], int.Parse(Request.Form["flight-deptTime"]), Request.Form["flight-deptCity"], Request.Form["flight-arrCity"], Request.Form["flight-status"]);
            newFlight.Save();
            return RedirectToAction("Success", "Home");
        }
        
       [HttpGet("/cities/{cityId}/flights/new")]
       public ActionResult CreateForm(int cityId)
       {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Cities city = Cities.Find(cityId);
          return View(city);
       }

       [HttpGet("/flights/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Flight selectedFlight = Flight.Find(id);
            List<Cities> flightCities = selectedFlight.GetCities();
            List<Cities> allCities = Cities.GetAll();
            model.Add("selectedFlight", selectedFlight);
            model.Add("flightCities", flightCities);
            model.Add("allCities", allCities);
            return View(model);

        }

       [HttpGet("/cities/{cityId}/items/{flightId}")]
       public ActionResult Details(int cityId, int flightId)
       {
          Flight flight = Flight.Find(flightId);
          Dictionary<string, object> model = new Dictionary<string, object>();
          Cities city = Cities.Find(cityId);
          model.Add("flight", flight);
          model.Add("city", city);
          return View(city);
       }

      [HttpPost("/flights/{flightId}/cities/new")]
        public ActionResult AddCity(int flightId)
        {
            Flight flight = flight.Find(flightId);
            Cities city = Cities.Find(Int32.Parse(Request.Form["city-id"]));
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
         public ActionResult DeleteItem(int id)
         {
             Flight.Delete(id);
             return RedirectToAction("Details", "Cities", new { id = id });
         }
    }
}

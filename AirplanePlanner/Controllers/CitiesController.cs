using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AirplanePlanner.Models;
using System;

namespace AirplanePlanner.Controllers
{
  public class CitiesController : Controller
  {
    [Route("/")]
    public ActionResult Index()
    {
      List<Cities> allCities = Cities.GetAll();
      return View("Index", allCities);
    }

    [HttpGet("cities/new")]
    public ActionResult CreateCityForm()
    {
      return View();
    }
    [HttpPost("/cities")]
    public ActionResult Create()
    {
      Cities newCity = new Cities(Request.Form["city-name"]);
      newCity.Save();
      return RedirectToAction("Success", "Home");
    }
    [HttpGet("/cities/{id}")]
      public ActionResult Details(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Cities selectedCity = Cities.Find(id);
        List<Flight> cityFlights = selectedCity.GetFlights();
        List<Flight> allFlights = Flight.GetAll();
        model.Add("selectedCity", selectedCity);
        model.Add("cityFlights", cityFlights);
        model.Add("allFlights", allFlights);
        return View(model);
      }
      [HttpPost("/cities/{cityId}/flights/new")]
      public ActionResult AddFlight(int cityId)
      {
          Cities city = Cities.Find(cityId);
          Flight flight = Flight.Find(Int32.Parse(Request.Form["flight-id"]));
          city.AddFlight(flight);
          return RedirectToAction("Details",  new { id = cityId });
      }
  }
}

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
      List<Cities> allCities = Cities.GetAllCities();
      return View("Index", allCities);
    }

    [HttpGet("cities/new")]
    public ActionResult CreateCityForm()
    {
      return View();
    }
    
  }
}

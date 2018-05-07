using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirplanePlanner.Models
{
  public class Flights
  {
    private int _id;
    private int _cityId;
    private int _departureTime;
    private string _departureCity;
    private string _arrivalCity;
    private string _status;

    public Flights(int id, int cityId, int departureTime, string departureCity, string arrivalCity, string Status)
    {
      _id = id;
      _cityId = cityId;
      _departureTime = departureTime;
      _departureCity = departureCity;
      _arrivalCity = arrivalCity;
      _status = Status;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCityId()
    {
      return _cityId;
    }
    public void SetCityId(int id)
    {
      _cityId = id
    }
    public int GetDepartureTime()
    {
      return _departureTime;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetStatus()
    {
      return _status;
    }
  }
}

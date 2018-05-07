using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirplanePlanner.Models
{
  public class Cities
  {
    private string _name;
    private int _id;

    public Cities(string name, int Id = 0)
    {
      _name = name;
      _id = Id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public static List<Cities> GetAllCities()
    {
      List<Cities> allCities = new List<Cities> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        Cities newCity = new Cities(cityId, cityName);
        allCities.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public List<Flight> GetFlights()
    {
      List<Flight> allCityFlights = new List<Flight> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `flights` WHERE `city_id` = @city_id;";

      MySqlParameter cityId = new MySqlParameter();
      cityId.ParameterName = "@city_id";
      cityId.Value = this._id;
      cmd.Parameters.Add(cityId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        int flightDeparture = rdr.GetInt32(1);
        string flightCityDep = rdr.GetString(2);
        string flightCityArriv = rdr.GetString(3);
        string flightStatus = rdr.GetString(4);
        Flight newFlight = new Flight(flightId, flightDeparture, flightCityDep, flightCityArriv, flightStatus);
        // newClient.SetAppt();
        allCityFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return allCityFlights;
    }
    public static Cities Find(int id)
  {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from `cities` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);


      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";

      while (rdr.Read())
      {
          cityId = rdr.GetInt32(0);
          cityName = rdr.GetString(1);

      }

      Cities foundCity = new Cities(cityId, cityName);

      conn.Close();
      if (conn != null)
      {
      conn.Dispose();
      }

      return foundCity;
    }
  }
}

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
    public List<Flight> GetFlights()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT flights.* FROM cities
              JOIN cities_flights ON (cities.id = cities_flights.city_id)
              JOIN flights ON (cities_flights.flight_id = flights.id)
              WHERE cities.id = @CityId;";

          MySqlParameter cityIdParameter = new MySqlParameter();
          cityIdParameter.ParameterName = "@CityId";
          cityIdParameter.Value = _id;
          cmd.Parameters.Add(cityIdParameter);

          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Flight> flights = new List<Flight>{};

          while(rdr.Read())
          {
            int flightId = rdr.GetInt32(0);
            string flightDepartureTime = rdr.GetString(1);
            string flightDepartureCity = rdr.GetString(2);
            string flightArrivalCity = rdr.GetString(3);
            string flightStatus = rdr.GetString(4);
            //string city_id = rdr.GetInt32(5)
            Flight newFlight = new Flight(flightId, flightDepartureTime, flightDepartureCity, flightArrivalCity, flightStatus);
            flights.Add(newFlight);
          }
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
          return flights;
        }
      public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = _id;
            cmd.Parameters.Add(city_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = newFlight.GetId();
            cmd.Parameters.Add(flight_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Save()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

          MySqlParameter name = new MySqlParameter();
          name.ParameterName = "@name";
          name.Value = this._name;
          cmd.Parameters.Add(name);

          // Code to declare, set, and add values to a cityId SQL parameters has also been removed.

          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }
        public static List<Cities> GetAll()
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
          Cities newCity = new Cities(cityName);
          newCity.SetId(cityId);
          allCities.Add(newCity);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allCities;
      }
      public static Cities Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";
      //string itemDueDate = "";
      // We remove the line setting a itemCategoryId value here.

      while(rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);
    //    itemDueDate = rdr.GetString(2);
        // We no longer read the itemCategoryId here, either.
      }
    }
  } 

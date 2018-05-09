using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirplanePlanner.Models
{
  public class Flight
  {
    private int _id;
    private int _cityId;
    private int _departureTime;
    private string _departureCity;
    private string _arrivalCity;
    private string _status;

    public Flight(int id, int cityId, int departureTime, string departureCity, string arrivalCity, string Status)
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
      _cityId = id;
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
    public void AddCity(Cities newCity)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

        MySqlParameter city_id = new MySqlParameter();
        city_id.ParameterName = "@CityId";
        city_id.Value = newCategory.GetId();
        cmd.Parameters.Add(city_id);

        MySqlParameter flight_id = new MySqlParameter();
        flight_id.ParameterName = "@FlightId";
        flight_id.Value = _id;
        cmd.Parameters.Add(flight_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }

      public List<Cities> GetCities()
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT city_id FROM cities_flights WHERE flight_id = @flightId;";

        MySqlParameter flightIdParameter = new MySqlParameter();
        flightIdParameter.ParameterName = "@flightId";
        flightIdParameter.Value = _id;
        cmd.Parameters.Add(flightIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> cityIds = new List<int> {};
        while(rdr.Read())
        {
            int cityId = rdr.GetInt32(0);
            cityIds.Add(cityId);
        }
        rdr.Dispose();

      List<Cities> cities = new List<Cities> {};
        foreach (int cityId in cityIds)
        {
            var cityQuery = conn.CreateCommand() as MySqlCommand;
            cityQuery.CommandText = @"SELECT * FROM cities WHERE id = @CityId;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = cityId;
            cityQuery.Parameters.Add(cityIdParameter);

            var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
            while(cityQueryRdr.Read())
            {
                int thisCityId = cityQueryRdr.GetInt32(0);
                string cityName = cityQueryRdr.GetString(1);
                Cities foundCity = new Cities(cityName, thisCityId);
                cities.Add(foundCity);
            }
            cityQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return cities;
      }
      public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (arrival_city) VALUES (@arrivalCity);";

            MySqlParameter arrivalInfo = new MySqlParameter();
            arrivalInfo.ParameterName = "@arrivalCity";
            arrivalInfo.Value = this._arrivalCity;
            cmd.Parameters.Add(arrivalInfo);

            // MySqlParameter dueDate = new MySqlParameter();
            // dueDate.ParameterName = "@dueDate";
            // dueDate.Value = this._dueDate;
            // cmd.Parameters.Add(dueDate);

            // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Flight> GetAll()
      {
          List<Flight> allFlights = new List<Flight> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM flights;";
          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          while(rdr.Read())
          {
            int flightId = rdr.GetInt32(0);
            string flightDepartureTime = rdr.GetString(1);
            string flightDepartureCity = rdr.GetString(2);
            string flightArrivalCity = rdr.GetString(3);
            string flightStatus = rdr.GetString(4);

            Flight newFlight = new Flight(flightDepartureTime, flightDepartureCity, flightArrivalCity, flightStatus, flightId);
            //newItem.SetDate(itemDueDate);
            allFlights.Add(newFlight);
          }
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
          return allFlights;
      }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int flightId = 0;
            int deptTime = 0;
            string deptCity = "";
            string arrCity = "";
            string flightStatus = "";

            //string itemDueDate = "";
            // We remove the line setting a itemCategoryId value here.

            while(rdr.Read())
            {
              flightId = rdr.GetInt32(0);
              deptTime = rdr.GetInt32(1);
              deptCity = rdr.GetString(2);
              arrCity = rdr.GetString(3);
              flightStatus = rdr.GetString(4);
          //    itemDueDate = rdr.GetString(2);
              // We no longer read the itemCategoryId here, either.
            }

            // Constructor below no longer includes a itemCategoryId parameter:
            Flight newFlight = new Flight(deptTime, deptCity, arrCity, flightStatus, flightId);
          //  newItem.SetDate(ItemDueDate);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newFlight;
        }

        // public void UpdateDescription(string newDescription)
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
        //
        //     MySqlParameter searchId = new MySqlParameter();
        //     searchId.ParameterName = "@searchId";
        //     searchId.Value = _id;
        //     cmd.Parameters.Add(searchId);
        //
        //     MySqlParameter description = new MySqlParameter();
        //     description.ParameterName = "@newDescription";
        //     description.Value = newDescription;
        //     cmd.Parameters.Add(description);
        //
        //     cmd.ExecuteNonQuery();
        //     _description = newDescription;
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }

        }

    }
  }

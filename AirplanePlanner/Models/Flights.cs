using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirplanePlanner.Models;
using System;

namespace AirplanePlanner.Models
{
  public class Flight
    {
        private string _description;
        //private string _dueDate;
        private bool _isDone;
        private int _id;

        // We no longer declare _categoryId here

        public Flight(string description, int id = 0, bool done = false)
        {
            _description = description;
            _id = id;
           // categoryId is removed from the constructor
        }

        public override bool Equals(System.Object otherFlight)
        {
          if (!(otherFlight is Flight))
          {
            return false;
          }
          else
          {
             Flight newFlight = (Flight) otherFlight;
             bool idEquality = this.GetId() == newFlight.GetId();
             bool descriptionEquality = this.GetDescription() == newFlight.GetDescription();
            //  bool dueDateEquality = this.GetDate() == newItem.GetDate();
             // We no longer compare Items' categoryIds in a categoryEquality bool here.
             return (idEquality && descriptionEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetDescription().GetHashCode();
        }

        public string GetDescription()
        {
            return _description;
        }

        public int GetId()
        {
            return _id;
        }

        // public string GetDate()
        // {
        //     return _dueDate;
        // }

        public bool GetDone()
        {
            return _isDone;
        }

        public void SetDone(bool maybeDone)
        {
            _isDone = maybeDone;
        }

        // public void SetDate(string newDate)
        // {
        //     _dueDate = newDate;
        // }

        public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = newCity.GetId();
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

        public List<City> GetCities()
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

            List<City> cities = new List<City> {};
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
                    City foundCity = new City(cityName, thisCityId);
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

        // We've removed the GetCategoryId() method entirely.

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (description) VALUES (@description);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);

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
            //   string itemDueDate = rdr.GetString(2);
              string flightDescription = rdr.GetString(1);
              // We no longer need to read categoryIds from our items table here.
              // Constructor below no longer includes a itemCategoryId parameter:
              Flight newFlight = new Flight(flightDescription, flightId);
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
            string flightName = "";
            //string itemDueDate = "";
            // We remove the line setting a itemCategoryId value here.

            while(rdr.Read())
            {
              flightId = rdr.GetInt32(0);
              flightName = rdr.GetString(1);
          //    itemDueDate = rdr.GetString(2);
              // We no longer read the itemCategoryId here, either.
            }

            // Constructor below no longer includes a itemCategoryId parameter:
            Flight newFlight = new Flight(flightName, flightId);
          //  newItem.SetDate(ItemDueDate);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newFlight;
        }

        public void UpdateDescription(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _description = newDescription;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        public static void Delete(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM cities_flights WHERE city_id = @FlightId;";

        MySqlParameter flightIdParameter = new MySqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = id;
        cmd.Parameters.Add(flightIdParameter);

        cmd.ExecuteNonQuery();
        if (conn != null)
        {
          conn.Close();
        }
      }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}

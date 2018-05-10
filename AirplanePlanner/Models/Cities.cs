using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirplanePlanner.Models;
using System;

namespace AirplanePlanner.Models
{
  public class City
  {
    private string _name;
    private int _id;
    private List<Flight> _flights;

    public City(string cityName, int city_id = 0)
    {
      _name = cityName;
      _id = city_id;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = this.GetId() == newCity.GetId();
        bool nameEquality = this.GetName() == newCity.GetName();
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
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
                string flightDescription = rdr.GetString(1);
                Flight newFlight = new Flight(flightDescription, flightId);
                flights.Add(newFlight);
              }
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
              return flights;
          }
          //Old Get Items without Join
//           public List<Item> GetItems()
// {
//     MySqlConnection conn = DB.Connection();
//     conn.Open();
//     var cmd = conn.CreateCommand() as MySqlCommand;
//     cmd.CommandText = @"SELECT item_id FROM categories_items WHERE category_id = @CategoryId;";
//
//     MySqlParameter categoryIdParameter = new MySqlParameter();
//     categoryIdParameter.ParameterName = "@CategoryId";
//     categoryIdParameter.Value = _id;
//     cmd.Parameters.Add(categoryIdParameter);
//
//     var rdr = cmd.ExecuteReader() as MySqlDataReader;
//
//     List<int> itemIds = new List<int> {};
//     while(rdr.Read())
//     {
//         int itemId = rdr.GetInt32(0);
//         itemIds.Add(itemId);
//     }
//     rdr.Dispose();
//
//     List<Item> items = new List<Item> {};
//     foreach (int itemId in itemIds)
//     {
//         var itemQuery = conn.CreateCommand() as MySqlCommand;
//         itemQuery.CommandText = @"SELECT * FROM items WHERE id = @ItemId;";
//
//         MySqlParameter itemIdParameter = new MySqlParameter();
//         itemIdParameter.ParameterName = "@ItemId";
//         itemIdParameter.Value = itemId;
//         itemQuery.Parameters.Add(itemIdParameter);
//
//         var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
//         while(itemQueryRdr.Read())
//         {
//             int thisItemId = itemQueryRdr.GetInt32(0);
//             string itemDescription = itemQueryRdr.GetString(1);
//             Item foundItem = new Item(itemDescription, thisItemId);
//             items.Add(foundItem);
//         }
//         itemQueryRdr.Dispose();
//     }
//     conn.Close();
//     if (conn != null)
//     {
//         conn.Dispose();
//     }
//     return items;
// }




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

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }

    public void SetId(int newId)
    {
      _id = newId;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public void SetList(List<Flight> flights)
    {
      _flights = flights;
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

      // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName);
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

    public static City Find(int id)
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

      // Constructor below no longer includes a itemCategoryId parameter:
      City newCity = new City(cityName, cityId);
    //  newCategory.SetDate(ItemDueDate);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }

      return newCity;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_flights WHERE city_id = @CityId;", conn);
      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cityIdParameter);
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
      cmd.CommandText = @"DELETE FROM cities;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}

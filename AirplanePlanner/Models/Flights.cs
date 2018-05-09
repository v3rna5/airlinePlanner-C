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
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `flights` (`departure_time`, `departure_city`, `arrival_city`, `status`, `city_id`) VALUES (@DepartureTime, @DepartureCity, @ArrivalCity, @Status, @CityId);";

      MySqlParameter departureTime = new MySqlParameter();
      departureTime.ParameterName = "@DepartureTime";
      departureTime.Value = this._departureTime;

      MySqlParameter departureCity = new MySqlParameter();
      departureCity.ParameterName = "@DepartureCity";
      departureCity.Value = this._departureCity;

      // MySqlParameter formattedAppt = new MySqlParameter();
      // formattedAppt.ParameterName = "@FormattedAppt";
      // formattedAppt.Value = this._formattedAppt;

      MySqlParameter arrivalCity = new MySqlParameter();
      arrivalCity.ParameterName = "@ArrivalCity";
      arrivalCity.Value = this._arrivalCity;

      MySqlParameter Status = new MySqlParameter();
      Status.ParameterName = "@Status";
      Status.Value = this._status;

      MySqlParameter cityId = new MySqlParameter();
      cityId.ParameterName = "@CityId";
      cityId.Value = this._cityId;


      cmd.Parameters.Add(departureTime);
      cmd.Parameters.Add(departureCity);
      cmd.Parameters.Add(arrivalCity);
      cmd.Parameters.Add(Status);
      cmd.Parameters.Add(cityId);

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
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        int flightDeparture = rdr.GetInt32(1);
        string flightCityDep = rdr.GetString(2);
        string flightCityArriv = rdr.GetString(3);
        string flightStatus = rdr.GetString(4);
        Flight newFlight = new Flight(flightId, flightDeparture, flightCityDep, flightCityArriv, flightStatus);
        // newClient.SetAppt();
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
      List<Flight> allFlights = Flight.GetAll();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from `flights` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      int flightTime = 0;
      string flightArrival = "";
      string flightDeparture = "";
      string flightStatus = "";
      int flightCityId = 0;

      while (rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightTime = rdr.GetInt32(1);
        flightDeparture = rdr.GetString(2);
        flightArrival = rdr.GetString(3);
        flightStatus = rdr.GetString(4);
        flightCityId = rdr.GetInt32(5);
      }

      Flight foundFlight = new Flight(flightId, flightTime, flightDeparture, flightArrival, flightStatus, flightCityId);

      conn.Close();
      if (conn != null)
      {
      conn.Dispose();
      }

      return foundFlight;
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
          bool idEquality = (this.GetId() == newFlight.GetId());
          bool nameEquality = (this.GetName() == newFlight.GetDepartureTime());
          bool cityEquality = (this.GetStylistId() == newFlight.GetCityId());
          return (idEquality && nameEquality && cityEquality);
        }
      }

      public override int GetHashCode()
     {
       return this.GetId().GetHashCode();
     }

  }
}

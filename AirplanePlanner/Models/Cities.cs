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
  }
}

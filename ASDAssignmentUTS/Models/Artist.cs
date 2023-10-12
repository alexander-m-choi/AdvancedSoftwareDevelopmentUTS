using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Models;

public class Artist
{
    public int id { get; set; }
    public string name { get; set; }
    public string genre { get; set; }
    public string country { get; set; }
    public string description { get; set; }

    public Artist(int id, string name, string genre, string country, string description)
    {
        this.id = id;
        this.name = name;
        this.genre = genre;
        this.country = country;
        this.description = description;
    }

    public Artist()
    {

    }

}

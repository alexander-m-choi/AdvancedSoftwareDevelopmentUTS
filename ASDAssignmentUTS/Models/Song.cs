using System;
using System.Collections.Generic;
using System.Linq;
using ASDAssignmentUTS.Services;

namespace ASDAssignmentUTS.Models
{
    public class Song
    {
        public int id { get; set; }
        public string name { get; set; }
        public int artistId { get; set; }
        public string genre { get; set; }
        public string description { get; set; }
        
        public Song()
        {

        }

        public String ArtistName { get => SongDBManager.GetArtistName(artistId); }
      //  public String SearchQuery { get; set; }
    }
}

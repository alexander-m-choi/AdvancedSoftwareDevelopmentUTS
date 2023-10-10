using System.ComponentModel.DataAnnotations;
using ASDAssignmentUTS.Services;

namespace ASDAssignmentUTS.Models
{
    public class Playlist
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the playlist.")]
        [StringLength(50, ErrorMessage = "The name must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string name { get; set; }

        [StringLength(500, ErrorMessage = "The description must be less than {1} characters long.")]
        public string description { get; set; }

        [Required(ErrorMessage = "Please enter an owner ID for the playlist.")]
        public int ownerId { get; set; }

        public Playlist(int id, string name, string description, int ownerId)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.ownerId = ownerId;
        }

        public Playlist()
        {

        }

        //get playlist by id

    }
}

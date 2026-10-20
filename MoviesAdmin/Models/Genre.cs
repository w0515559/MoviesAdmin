using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; } // Unique identifier for the genre (auto-generated)

        [Display(Name = "Genre Name")]
        [StringLength(25)] // Longest common genre is 15 characters
        [Required]
        public string Title { get; set; } = string.Empty; // Genre title (e.g., Action, Comedy, Drama)
    }
}

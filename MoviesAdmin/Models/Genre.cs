using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; } // Unique identifier for the genre (auto-generated)

        // navigation property
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();

        [Display(Name = "Genre Name")]
        [StringLength(25)] // Longest common genre is 15 characters
        [Required]
        public string Title { get; set; } = string.Empty; // Genre title (e.g., Action, Comedy, Drama)

    }
}

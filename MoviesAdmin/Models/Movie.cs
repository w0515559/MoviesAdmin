using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; } // Unique identifier for the movie (auto-generated)

        [Required]
        [StringLength(250)] // Longest movie title is 201 characters long
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(25)] // Longest common genre is 15 characters
        public string Genre { get; set; } = string.Empty;

        [Required]
        [StringLength(10)] // 
        public string Rating { get; set; } = string.Empty; // Format: G, PG, PG-13, R

        [StringLength(5000)] // Some movies have a 10 page synopsis
        [Required]
        public string Synopsis {  get; set; } = string.Empty;

        [Required]
        public DateOnly ReleaseDate { get; set; } = DateOnly.MinValue; // (defaults to jan 1st 0001 if no date given)

        [Range(1, 52000)] // longest movie is 51420 minutes
        [Required]
        public int Runtime { get; set; } // Runtime in minutes 
    }
}

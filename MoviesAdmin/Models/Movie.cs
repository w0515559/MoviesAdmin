using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        // Primary key
        [Required]
        public int Id { get; set; } // Unique identifier for the movie (auto-generated)

        // foreign key for genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        // navigation property for genre
        [ValidateNever]
        public Genre Genre { get; set; } = null!;

        // foreign key for Rating
        [Display(Name = "Age Rating")]
        public int AudienceRatingId { get; set; }

        // navigation property for rating
        [ValidateNever]
        public AudienceRating AudienceRating { get; set; } = null!;

        // navigation property for critic reviews
        public List<CriticReview> CriticReviews { get; set; } = new List<CriticReview>();




        [Required]
        [StringLength(250)] // Longest movie title is 201 characters long
        public string Title { get; set; } = string.Empty;

        [StringLength(5000)] // Some movies have a 10 page synopsis
        [Required]
        public string Synopsis {  get; set; } = string.Empty;

        [Display(Name = "Release Date")]
        [Required]
        public DateOnly ReleaseDate { get; set; } = DateOnly.MinValue; // (defaults to jan 1st 0001 if no date given)

        [Range(1, 52000)] // longest movie is 51420 minutes
        [Required]
        public int Runtime { get; set; } // Runtime in minutes 

        // URL for image stored in azure storage
        public string ImageUrl { get; set; } = string.Empty;
    }
}

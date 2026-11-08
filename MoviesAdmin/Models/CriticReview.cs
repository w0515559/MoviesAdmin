using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    // Each movie only allows 1 review per user https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=data-annotations
    [Index(nameof(MovieId), nameof(CreatedBy), IsUnique = true)]
    public class CriticReview
    {

        // primary key
        [Required]
        public int Id { get; set; } // Unique identifier for the review (auto-generated)


        // foreign key to Movie
        public int MovieId { get; set; }

        // nav property
        [ValidateNever]
        public Movie Movie { get; set; } = null!;



        [StringLength(1000)] // Maximum length for the review description
        [Required]
        public string Description { get; set; } = string.Empty; // Review description

        [Range(0, 5)] // Rating must be between 0 and 5
        [Required]
        public int Rating { get; set; } // Rating (0 - 5 stars)

        [DisplayName("Published")]
        [Required]
        public bool IsPublished { get; set; } // Indicates if the review is published

        [DisplayName("Created By")] 
        [StringLength(40)] // Maximum length for the username of the reviewer
        public string CreatedBy { get; set; } = string.Empty; // User who created the review

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Date when the review was created

    }
}

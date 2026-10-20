using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {

        [Required]
        public int Id { get; set; } // Unique identifier for the review (auto-generated)

        [StringLength(2500)] // Maximum length for the review description
        [Required]
        public string Description { get; set; } = string.Empty; // Review description

        [Range(1, 5)] // Rating must be between 1 and 5
        [Required]
        public int Rating { get; set; } // Rating (1 - 5 stars)

        [Required]
        public bool IsPublished { get; set; } // Indicates if the review is published

        [DisplayName("Created By")] 
        [StringLength(40)] // Maximum length for the username of the reviewer
        [Required]
        public string CreatedBy { get; set; } = string.Empty; // User who created the review

        [Display(Name = "Date Created")]
        [Required]
        public DateTime CreatedDate { get; set; } // Date when the review was created

    }
}

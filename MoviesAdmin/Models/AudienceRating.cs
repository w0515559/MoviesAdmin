using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        [Required]
        public int Id { get; set; }

        // navigation property
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();

        [Display(Name = "Age Rating")]
        [StringLength(10)]  
        [Required]
        public string Title { get; set; } = string.Empty; // Audience rating title (PG, PG-13, R, Unrated)



    }
}
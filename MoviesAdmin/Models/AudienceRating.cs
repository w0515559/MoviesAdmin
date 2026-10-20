using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Age Rating")]
        [StringLength(10)]  
        [Required]
        public string Title { get; set; } = string.Empty; // Audience rating title (PG, PG-13, R, Unrated)
    }
}
            
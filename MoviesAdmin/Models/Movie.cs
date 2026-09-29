namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; } // Unique identifier for the movie

        public string Title { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty; 

        public string Rating { get; set; } = string.Empty; // Format: G, PG, PG-13, R

        public string Synopsis {  get; set; } = string.Empty;

        public DateOnly ReleaseDate { get; set; } = DateOnly.MinValue; // (defaults to jan 1st 0001 if no date given)

        public int Runtime { get; set; } // Runtime in minutes 


    }
}

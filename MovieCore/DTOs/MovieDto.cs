namespace MovieCore.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int Duration { get; set; }
        public MovieDetailsDto? MovieDetails { get; set; }
    }
}
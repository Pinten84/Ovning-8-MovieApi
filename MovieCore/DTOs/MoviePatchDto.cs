public class MoviePatchDto
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public int? Duration { get; set; }
    public int? GenreId { get; set; }
    public MovieDetailsPatchDto? MovieDetails { get; set; }
}
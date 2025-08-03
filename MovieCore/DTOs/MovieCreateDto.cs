using System.ComponentModel.DataAnnotations;

public class MovieCreateDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(1888, 2100)]
    public int Year { get; set; }

    [Required]
    public int GenreId { get; set; }

    [Required]
    [Range(1, 1000)]
    public int Duration { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Budget must be non-negative.")]
    public int Budget { get; set; }

    public List<int>? ActorIds { get; set; } // Om du vill ange skådespelare direkt
}
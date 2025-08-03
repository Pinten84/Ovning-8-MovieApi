using System.ComponentModel.DataAnnotations;

public class ReviewCreateDto
{
    [Required]
    [StringLength(100)]
    public string ReviewerName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Comment { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    public int MovieId { get; set; }
}
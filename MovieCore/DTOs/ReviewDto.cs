public class ReviewDto
{
    public int Id { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty; // Ny property
}
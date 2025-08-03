public class GenreNotFoundException : Exception
{
    public int GenreId { get; }
    public GenreNotFoundException(int genreId)
        : base($"Genre with id {genreId} does not exist.")
    {
        GenreId = genreId;
    }
}
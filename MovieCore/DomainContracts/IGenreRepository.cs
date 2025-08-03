using MovieCore.Entities;

public interface IGenreRepository
{
    Task<bool> AnyAsync(int id);
    Task<Genre?> GetByIdAsync(int id);
}
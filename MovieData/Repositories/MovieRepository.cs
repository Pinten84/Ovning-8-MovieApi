using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Entities;

namespace MovieData.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie?> GetAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Movies.AnyAsync(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public void Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
        }
    }
}
using MovieCore.DomainContracts;
using MovieCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieData.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MovieDbContext _context;
        public GenreRepository(MovieDbContext context) => _context = context;

        public async Task<bool> AnyAsync(int id)
            => await _context.Genres.AnyAsync(g => g.Id == id);

        public async Task<Genre?> GetByIdAsync(int id)
            => await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
    }
}
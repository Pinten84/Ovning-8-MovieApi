using Microsoft.EntityFrameworkCore;
using MovieCore.DomainContracts;
using MovieCore.Entities;

namespace MovieData.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieDbContext _context;

        public ReviewRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .ToListAsync();
        }

        public async Task<Review?> GetAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }

        public void Remove(Review review)
        {
            _context.Reviews.Remove(review);
        }
    }
}
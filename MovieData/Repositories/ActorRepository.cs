using MovieCore.DomainContracts;
using MovieCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieData.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovieDbContext _context;

        public ActorRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Actor?> GetAsync(int id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Actors.AnyAsync(a => a.Id == id);
        }

        public void Add(Actor actor)
        {
            _context.Actors.Add(actor);
        }

        public void Update(Actor actor)
        {
            _context.Actors.Update(actor);
        }

        public void Remove(Actor actor)
        {
            _context.Actors.Remove(actor);
        }
    }
}
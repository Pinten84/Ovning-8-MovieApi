using System.Collections.Generic;
using System.Threading.Tasks;
using MovieCore.Entities;
using MovieCore.DomainContracts;

namespace MovieCore.DomainContracts
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Review review);
        void Update(Review review);
        void Remove(Review review);
    }
}
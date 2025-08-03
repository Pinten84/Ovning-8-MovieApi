using MovieCore.DTOs;
using MovieCore.Entities;

namespace Movie.Service.Contracts
{
    public interface IActorService
    {
        Task<PagedResult<Actor>> GetPagedAsync(PageQuery query);
        Task<IEnumerable<Actor>> GetAllAsync();
        Task<Actor?> GetByIdAsync(int id);
        Task<Actor> CreateAsync(Actor actor);
        Task<bool> UpdateAsync(int id, Actor actor);
        Task<bool> DeleteAsync(int id);
    }
}
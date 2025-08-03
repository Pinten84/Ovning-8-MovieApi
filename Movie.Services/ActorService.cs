using MovieCore.DomainContracts;
using MovieCore.Entities;
using Movie.Service.Contracts;
using MovieCore.DTOs;

namespace Movie.Services
{
    public class ActorService : IActorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Actor>> GetPagedAsync(PageQuery query)
        {
            var allActors = await _unitOfWork.Actors.GetAllAsync();
            var totalItems = allActors.Count();
            var pageSize = query.PageSize;
            var currentPage = query.Page;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var actors = allActors
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Actor>
            {
                Data = actors,
                Meta = new MetaData
                {
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    PageSize = pageSize
                }
            };
        }

        public async Task<IEnumerable<Actor>> GetAllAsync() => await _unitOfWork.Actors.GetAllAsync();
        public async Task<Actor?> GetByIdAsync(int id) => await _unitOfWork.Actors.GetAsync(id);
        public async Task<Actor> CreateAsync(Actor actor)
        {
            _unitOfWork.Actors.Add(actor);
            await _unitOfWork.CompleteAsync();
            return actor;
        }
        public async Task<bool> UpdateAsync(int id, Actor actor)
        {
            var existing = await _unitOfWork.Actors.GetAsync(id);
            if (existing == null) return false;
            existing.Name = actor.Name;
            existing.BirthYear = actor.BirthYear;
            _unitOfWork.Actors.Update(existing);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var actor = await _unitOfWork.Actors.GetAsync(id);
            if (actor == null) return false;
            _unitOfWork.Actors.Remove(actor);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
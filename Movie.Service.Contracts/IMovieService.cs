using MovieCore.DTOs;

namespace Movie.Service.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<MovieDto?> GetByIdAsync(int id);
        Task<MovieDto> CreateAsync(MovieCreateDto dto);
        Task<bool> UpdateAsync(int id, MovieUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<MovieDto>> GetPagedAsync(PageQuery query);
        Task<bool> PatchAsync(int id, MoviePatchDto patchDto);
        Task<ServiceResult<MovieDto>> GetByIdWithResultAsync(int id);
        Task<ServiceResult<MovieDto>> CreateWithResultAsync(MovieCreateDto dto);
        // ...lägg till övriga metoder enligt behov
    }
}
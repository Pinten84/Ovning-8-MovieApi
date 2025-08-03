namespace Movie.Service.Contracts
{
    public interface IReviewService
    {
        Task<PagedResult<ReviewDto>> GetPagedAsync(PageQuery query);
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<ReviewDto?> GetByIdAsync(int id);
        Task<ReviewDto> CreateAsync(ReviewCreateDto dto);
        Task<bool> UpdateAsync(int id, ReviewCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
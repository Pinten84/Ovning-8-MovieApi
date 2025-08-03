using MovieCore.DomainContracts;
using MovieCore.Entities;
using Movie.Service.Contracts;
using AutoMapper;
using MovieCore.DTOs;

namespace Movie.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ReviewDto>> GetPagedAsync(PageQuery query)
        {
            var allReviews = await _unitOfWork.Reviews.GetAllAsync();
            var totalItems = allReviews.Count();
            var pageSize = query.PageSize;
            var currentPage = query.Page;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var reviews = allReviews
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ReviewDto>
            {
                Data = _mapper.Map<IEnumerable<ReviewDto>>(reviews),
                Meta = new MetaData
                {
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    PageSize = pageSize
                }
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto?> GetByIdAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetAsync(id);
            return review == null ? null : _mapper.Map<ReviewDto>(review);
        }

        public async Task<ReviewDto> CreateAsync(ReviewCreateDto dto)
        {
            var movie = await _unitOfWork.Movies.GetAsync(dto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found.");

            // Regel: Max 10 recensioner per film
            if (movie.Reviews.Count() >= 10)
                throw new InvalidOperationException("A movie can have a maximum of 10 reviews.");

            // Regel: Max 5 recensioner om filmen är äldre än 20 år
            if (movie.Year <= DateTime.Now.Year - 20 && movie.Reviews.Count() >= 5)
                throw new InvalidOperationException("Movies older than 20 years can have max 5 reviews.");

            var review = _mapper.Map<Review>(dto);
            _unitOfWork.Reviews.Add(review);
            await _unitOfWork.CompleteAsync();

            var reviewWithMovie = await _unitOfWork.Reviews.GetAsync(review.Id);
            return _mapper.Map<ReviewDto>(reviewWithMovie);
        }

        public async Task<bool> UpdateAsync(int id, ReviewCreateDto dto)
        {
            var review = await _unitOfWork.Reviews.GetAsync(id);
            if (review == null) return false;
            _mapper.Map(dto, review);
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetAsync(id);
            if (review == null) return false;
            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
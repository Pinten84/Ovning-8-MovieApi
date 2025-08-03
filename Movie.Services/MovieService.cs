using Movie.Service.Contracts;
using MovieCore.DTOs;
using MovieCore.DomainContracts;
using MovieCore.Entities;
using AutoMapper;

namespace Movie.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
            => _mapper.Map<IEnumerable<MovieDto>>(await _unitOfWork.Movies.GetAllAsync());

        public async Task<MovieDto?> GetByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetAsync(id);
            return movie == null ? null : _mapper.Map<MovieDto>(movie);
        }

        public async Task<ServiceResult<MovieDto>> GetByIdWithResultAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetAsync(id);
            if (movie == null)
                return ServiceResult<MovieDto>.NotFound($"Movie with id {id} not found.");
            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
        }

        public async Task<MovieDto> CreateAsync(MovieCreateDto dto)
        {
            if (dto.Budget < 0)
                throw new InvalidOperationException("Budget must be non-negative.");

            var allMovies = await _unitOfWork.Movies.GetAllAsync();
            if (allMovies.Any(m => string.Equals(m.Title, dto.Title, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A movie with this title already exists.");

            var genreExists = await _unitOfWork.Genres.AnyAsync(dto.GenreId);
            if (!genreExists)
                throw new GenreNotFoundException(dto.GenreId);

            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.GenreId);
            if (genre != null && genre.Name == "Documentary")
            {
                if (dto.Budget > 1_000_000)
                    throw new InvalidOperationException("A documentary cannot have a budget over 1 million.");
                if (dto.ActorIds != null && dto.ActorIds.Count > 10)
                    throw new InvalidOperationException("A documentary cannot have more than 10 actors.");
            }

            var movie = _mapper.Map<MovieCore.Entities.Movie>(dto);
            _unitOfWork.Movies.Add(movie);
            await _unitOfWork.CompleteAsync();

            var movieWithGenre = await _unitOfWork.Movies.GetAsync(movie.Id);
            return _mapper.Map<MovieDto>(movieWithGenre);
        }

        public async Task<ServiceResult<MovieDto>> CreateWithResultAsync(MovieCreateDto dto)
        {
            if (dto.Budget < 0)
                return ServiceResult<MovieDto>.BadRequest("Budget must be non-negative.");

            var allMovies = await _unitOfWork.Movies.GetAllAsync();
            if (allMovies.Any(m => string.Equals(m.Title, dto.Title, StringComparison.OrdinalIgnoreCase)))
                return ServiceResult<MovieDto>.Conflict("A movie with this title already exists.");

            var genreExists = await _unitOfWork.Genres.AnyAsync(dto.GenreId);
            if (!genreExists)
                return ServiceResult<MovieDto>.NotFound($"Genre with id {dto.GenreId} not found.");

            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.GenreId);
            if (genre != null && genre.Name == "Documentary")
            {
                if (dto.Budget > 1_000_000)
                    return ServiceResult<MovieDto>.BadRequest("A documentary cannot have a budget over 1 million.");
                if (dto.ActorIds != null && dto.ActorIds.Count > 10)
                    return ServiceResult<MovieDto>.BadRequest("A documentary cannot have more than 10 actors.");
            }

            var movie = _mapper.Map<MovieCore.Entities.Movie>(dto);
            _unitOfWork.Movies.Add(movie);
            await _unitOfWork.CompleteAsync();

            var movieWithGenre = await _unitOfWork.Movies.GetAsync(movie.Id);
            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movieWithGenre));
        }

        public async Task<bool> UpdateAsync(int id, MovieUpdateDto dto)
        {
            var movie = await _unitOfWork.Movies.GetAsync(id);
            if (movie == null) return false;

            if (dto.Budget < 0)
                throw new InvalidOperationException("Budget must be non-negative.");

            var allMovies = await _unitOfWork.Movies.GetAllAsync();
            if (allMovies.Any(m => string.Equals(m.Title, dto.Title, StringComparison.OrdinalIgnoreCase) && m.Id != id))
                throw new InvalidOperationException("A movie with this title already exists.");

            _mapper.Map(dto, movie);
            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetAsync(id);
            if (movie == null) return false;
            _unitOfWork.Movies.Remove(movie);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PagedResult<MovieDto>> GetPagedAsync(PageQuery query)
        {
            var allMovies = await _unitOfWork.Movies.GetAllAsync();
            var totalItems = allMovies.Count();
            var pageSize = query.PageSize;
            var currentPage = query.Page;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var movies = allMovies
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<MovieDto>
            {
                Data = _mapper.Map<IEnumerable<MovieDto>>(movies),
                Meta = new MetaData
                {
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    PageSize = pageSize
                }
            };
        }

        public async Task<bool> PatchAsync(int id, MoviePatchDto patchDto)
        {
            var movie = await _unitOfWork.Movies.GetAsync(id);
            if (movie == null) return false;

            if (patchDto.Title != null) movie.Title = patchDto.Title;
            if (patchDto.Year.HasValue) movie.Year = patchDto.Year.Value;
            if (patchDto.Duration.HasValue) movie.Duration = patchDto.Duration.Value;
            if (patchDto.GenreId.HasValue) movie.GenreId = patchDto.GenreId.Value;

            if (patchDto.MovieDetails != null)
            {
                if (movie.MovieDetails == null)
                {
                    movie.MovieDetails = new MovieDetails
                    {
                        Language = "Unknown",
                        Synopsis = patchDto.MovieDetails.Synopsis ?? string.Empty,
                        Budget = patchDto.MovieDetails.Budget ?? 0
                    };
                }
                else
                {
                    if (patchDto.MovieDetails.Budget.HasValue)
                        movie.MovieDetails.Budget = patchDto.MovieDetails.Budget.Value;
                    if (patchDto.MovieDetails.Synopsis != null)
                        movie.MovieDetails.Synopsis = patchDto.MovieDetails.Synopsis;
                }
            }

            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
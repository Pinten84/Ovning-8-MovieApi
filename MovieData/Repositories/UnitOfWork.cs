using MovieCore.DomainContracts;

namespace MovieData.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;
        public IMovieRepository Movies { get; }
        public IReviewRepository Reviews { get; }
        public IActorRepository Actors { get; }
        public IGenreRepository Genres { get; }

        public UnitOfWork(
            MovieDbContext context,
            IMovieRepository movieRepository,
            IReviewRepository reviewRepository,
            IActorRepository actorRepository,
            IGenreRepository genreRepository)
        {
            _context = context;
            Movies = movieRepository;
            Reviews = reviewRepository;
            Actors = actorRepository;
            Genres = genreRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
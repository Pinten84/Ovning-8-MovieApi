using MovieCore.DomainContracts;

namespace MovieData
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;
        public IMovieRepository Movies { get; }
        public IActorRepository Actors { get; }
        public IReviewRepository Reviews { get; }
        public IGenreRepository Genres { get; }

        public UnitOfWork(
            MovieDbContext context,
            IMovieRepository movieRepository,
            IActorRepository actorRepository,
            IReviewRepository reviewRepository,
            IGenreRepository genreRepository)
        {
            _context = context;
            Movies = movieRepository;
            Actors = actorRepository;
            Reviews = reviewRepository;
            Genres = genreRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
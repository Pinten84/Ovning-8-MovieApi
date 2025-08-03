using Movie.Service.Contracts;

namespace Movie.Services
{
    public class ServiceManager : IServiceManager
    {
        public IMovieService Movies { get; }
        public IReviewService Reviews { get; }
        public IActorService Actors { get; }

        public ServiceManager(IMovieService movies, IReviewService reviews, IActorService actors)
        {
            Movies = movies;
            Reviews = reviews;
            Actors = actors;
        }
    }
}
namespace Movie.Service.Contracts
{
    public interface IServiceManager
    {
        IMovieService Movies { get; }
        IReviewService Reviews { get; }
        IActorService Actors { get; }
    }
}
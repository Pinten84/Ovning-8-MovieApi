using System.Threading.Tasks;

namespace MovieCore.DomainContracts
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IReviewRepository Reviews { get; }
        IActorRepository Actors { get; }
        IGenreRepository Genres { get; }
        Task CompleteAsync();
    }
}
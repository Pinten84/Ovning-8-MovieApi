using Microsoft.EntityFrameworkCore;
using MovieCore.Entities;
using MovieData;

public class MovieActorService
{
    private readonly MovieDbContext _context;
    public MovieActorService(MovieDbContext context) => _context = context;

    public async Task AddActorToMovie(int movieId, int actorId, string role)
    {
        var exists = await _context.MovieActors.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
        if (exists)
            throw new InvalidOperationException("Actor is already assigned to this movie.");

        var movieActor = new MovieActor { MovieId = movieId, ActorId = actorId, Role = role };
        _context.MovieActors.Add(movieActor);
        await _context.SaveChangesAsync();
    }
}
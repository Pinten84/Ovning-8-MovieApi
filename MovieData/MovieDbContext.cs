using Microsoft.EntityFrameworkCore;
using MovieCore.Entities;

namespace MovieData
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Sci-Fi" },
                new Genre { Id = 5, Name = "Documentary" }
            );

            // Seed Actors
            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Keanu Reeves", BirthYear = 1964 },
                new Actor { Id = 2, Name = "Carrie-Anne Moss", BirthYear = 1967 },
                new Actor { Id = 3, Name = "Laurence Fishburne", BirthYear = 1961 },
                new Actor { Id = 4, Name = "Will Smith", BirthYear = 1968 }
            );

            // Seed Movies
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "The Matrix", Year = 1999, Duration = 136, GenreId = 4 },
                new Movie { Id = 2, Title = "Men in Black", Year = 1997, Duration = 98, GenreId = 1 },
                new Movie { Id = 3, Title = "John Wick", Year = 2014, Duration = 101, GenreId = 1 }
            );

            // Seed MovieActors (many-to-many)
            modelBuilder.Entity<MovieActor>().HasData(
                new MovieActor { MovieId = 1, ActorId = 1, Role = "Neo" },
                new MovieActor { MovieId = 1, ActorId = 2, Role = "Trinity" },
                new MovieActor { MovieId = 1, ActorId = 3, Role = "Morpheus" },
                new MovieActor { MovieId = 2, ActorId = 4, Role = "Agent J" },
                new MovieActor { MovieId = 3, ActorId = 1, Role = "John Wick" }
            );

            // Seed Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, ReviewerName = "Alice", Comment = "En fantastisk film!", Rating = 5, MovieId = 1 },
                new Review { Id = 2, ReviewerName = "Bob", Comment = "Spännande och nyskapande.", Rating = 4, MovieId = 1 },
                new Review { Id = 3, ReviewerName = "Charlie", Comment = "Rolig och underhållande.", Rating = 4, MovieId = 2 },
                new Review { Id = 4, ReviewerName = "Diana", Comment = "Intensiv action!", Rating = 5, MovieId = 3 }
            );

            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });
        }
    }
}
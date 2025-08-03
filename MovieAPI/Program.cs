using Microsoft.EntityFrameworkCore;
using MovieData;
using Movie.Service.Contracts;
using Movie.Services;
using MovieCore.DomainContracts;
using MovieData.Repositories;

namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IUnitOfWork, MovieData.Repositories.UnitOfWork>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IActorService, ActorService>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddControllers()
                   .AddApplicationPart(typeof(Movie.Presentation.Controllers.ActorsController).Assembly)
                   .AddNewtonsoftJson();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<ProblemDetailsMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}

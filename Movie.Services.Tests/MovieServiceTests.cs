using Xunit;
using Moq;
using AutoMapper;
using Movie.Services;
using MovieCore.DomainContracts;
using MovieCore.DTOs;
using MovieCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Movie.Services.Tests;

public class MovieServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MovieService _service;

    public MovieServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new MovieService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedMovies()
    {
        // Arrange
        var movies = new List<MovieCore.Entities.Movie> { new MovieCore.Entities.Movie { Id = 1, Title = "Test" } };
        var movieDtos = new List<MovieDto> { new MovieDto { Id = 1, Title = "Test" } };
        _unitOfWorkMock.Setup(u => u.Movies.GetAllAsync()).ReturnsAsync(movies);
        _mapperMock.Setup(m => m.Map<IEnumerable<MovieDto>>(movies)).Returns(movieDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Test", ((List<MovieDto>)result)[0].Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMovieNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Movies.GetAsync(It.IsAny<int>())).ReturnsAsync((MovieCore.Entities.Movie?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenBudgetIsNegative()
    {
        var dto = new MovieCreateDto { Title = "Test", Budget = -1, GenreId = 1 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }
}

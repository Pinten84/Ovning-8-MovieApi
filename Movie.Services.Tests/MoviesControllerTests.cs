using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Controllers;
using Movie.Service.Contracts;
using MovieCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MoviesControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly MoviesController _controller;

    public MoviesControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new MoviesController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithPagedMovies()
    {
        // Arrange
        var pagedResult = new PagedResult<MovieDto>
        {
            Data = new List<MovieDto> { new MovieDto { Id = 1, Title = "Test" } },
            Meta = new MetaData { TotalItems = 1, CurrentPage = 1, TotalPages = 1, PageSize = 10 }
        };
        _serviceManagerMock.Setup(s => s.Movies.GetPagedAsync(It.IsAny<PageQuery>())).ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetAll(new PageQuery());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<PagedResult<MovieDto>>(okResult.Value);
        Assert.Single(returned.Data);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        _serviceManagerMock.Setup(s => s.Movies.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((MovieDto?)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
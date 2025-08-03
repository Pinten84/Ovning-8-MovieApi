using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Movie.Service.Contracts;
using MovieCore.DTOs;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MoviesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<PagedResult<MovieDto>>> GetAll([FromQuery] PageQuery query)
        {
            var result = await _serviceManager.Movies.GetPagedAsync(query);
            return Ok(result);
        }

        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _serviceManager.Movies.GetByIdAsync(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        // GET: api/movies/5/withresult (exempel på ServiceResult)
        [HttpGet("{id}/withresult")]
        public async Task<IActionResult> GetWithResult(int id)
        {
            var result = await _serviceManager.Movies.GetByIdWithResultAsync(id);
            if (result.Problem != null)
            {
                var problem = new ProblemDetails
                {
                    Title = result.Problem.Title,
                    Detail = result.Problem.Detail,
                    Status = result.Problem.Status
                };
                return StatusCode(result.StatusCode, problem);
            }
            return Ok(result.Value);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateDto dto)
        {
            try
            {
                var created = await _serviceManager.Movies.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (GenreNotFoundException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Genre not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Business rule violation",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // POST: api/movies/withresult (exempel på ServiceResult)
        [HttpPost("withresult")]
        public async Task<IActionResult> CreateWithResult(MovieCreateDto dto)
        {
            var result = await _serviceManager.Movies.CreateWithResultAsync(dto);
            if (result.Problem != null)
            {
                var problem = new ProblemDetails
                {
                    Title = result.Problem.Title,
                    Detail = result.Problem.Detail,
                    Status = result.Problem.Status
                };
                return StatusCode(result.StatusCode, problem);
            }
            return CreatedAtAction(nameof(GetWithResult), new { id = result.Value!.Id }, result.Value);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MovieUpdateDto dto)
        {
            var updated = await _serviceManager.Movies.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.Movies.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        // PATCH: api/movies/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var movie = await _serviceManager.Movies.GetByIdAsync(id);
            if (movie == null)
                return NotFound();

            var patchDto = new MoviePatchDto
            {
                Title = movie.Title,
                Year = movie.Year,
                Duration = movie.Duration,
                GenreId = movie.GenreId,
                MovieDetails = movie.MovieDetails != null
                    ? new MovieDetailsPatchDto
                    {
                        Budget = movie.MovieDetails.Budget,
                        Synopsis = movie.MovieDetails.Synopsis
                    }
                    : new MovieDetailsPatchDto()
            };

            if (patchDto.MovieDetails == null)
                patchDto.MovieDetails = new MovieDetailsPatchDto();

            patchDoc.ApplyTo(patchDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _serviceManager.Movies.PatchAsync(id, patchDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
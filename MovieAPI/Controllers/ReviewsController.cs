using Microsoft.AspNetCore.Mvc;
using MovieCore.DTOs;
using Movie.Service.Contracts;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ReviewsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ReviewDto>>> GetAll([FromQuery] PageQuery query)
        {
            var result = await _serviceManager.Reviews.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            var review = await _serviceManager.Reviews.GetByIdAsync(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(ReviewCreateDto dto)
        {
            var review = await _serviceManager.Reviews.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ReviewCreateDto dto)
        {
            var result = await _serviceManager.Reviews.UpdateAsync(id, dto);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceManager.Reviews.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
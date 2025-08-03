using Microsoft.AspNetCore.Mvc;
using Movie.Service.Contracts;
using MovieCore.DTOs;
using MovieCore.Entities;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ActorsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Actor>>> GetAll([FromQuery] PageQuery query)
        {
            var result = await _serviceManager.Actors.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var actor = await _serviceManager.Actors.GetByIdAsync(id);
            if (actor == null)
                return NotFound();
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Actor actor)
        {
            var created = await _serviceManager.Actors.CreateAsync(actor);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Actor actor)
        {
            var updated = await _serviceManager.Actors.UpdateAsync(id, actor);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.Actors.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
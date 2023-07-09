
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurantDtos = await _service.GetAll();

            return Ok(restaurantDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var restaurant = await _service.GetById(id);

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody]  CreateRestaurantDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _service.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
             var isDeleted = await _service.Delete(id);

            if(isDeleted) return NoContent();

            return NotFound();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            var isUpdated = await _service.Update(id, dto);

            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurantDtos = await _service.GetAll();

            return Ok(restaurantDtos);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById([FromRoute] int id)
        {
            var restaurant = await _service.GetById(id);

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurant([FromBody]  CreateRestaurantDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _service.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }
    }
}

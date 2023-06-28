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
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRestaurantService _service;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper, IRestaurantService service)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await _dbContext.Restaurants
                .Include(x=>x.Address)
                .Include(x=>x.Dishes)
                .ToListAsync();

            var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return Ok(restaurantDtos);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById([FromRoute] int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefaultAsync(i => i.Id == id);

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurant([FromBody]  CreateRestaurantDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created($"/api/restaurant/{restaurant.Id}", null);
        }
    }
}

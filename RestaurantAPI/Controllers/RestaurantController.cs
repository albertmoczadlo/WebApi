using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await _dbContext.Restaurants
                .Include(x=>x.Address)
                .Include(x=>x.Dishes)
                .ToListAsync();

            var restaurantDtos = _mapper.Map<RestaurantDto>(restaurants);

            return Ok(restaurantDtos);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById([FromRoute] int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurant);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll()
        {
            return await _dbContext.Restaurants.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById([FromRoute]int id)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(i=>i.Id == id);

            if(restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }
    }
}

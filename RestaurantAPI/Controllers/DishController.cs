using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController:ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute]int restaurantId,[FromBody] CreateDishDto dto)
        {
            var newDishId = _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}",null);
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> Get([FromRoute] int restaurantId, [FromRoute]int dishId)
        {
            var dish = await _dishService.GetDishById(restaurantId,dishId);

            return Ok(dish);
        }

        [HttpGet]
        public async Task<ActionResult<List<DishDto>>> Get([FromRoute] int restaurantId)
        {
            var result = await _dishService.GetAll(restaurantId);

            return Ok(result);
        }
    }
}

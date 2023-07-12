using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/{restaurantId}/dish")]
    [ApiController]
    public class DishController:ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute]int restaurantId, CreateDishDto dto)
        {

        }
    }
}

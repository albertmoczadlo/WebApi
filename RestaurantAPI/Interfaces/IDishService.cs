using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IDishService
    {
        Task<int> Create(int restaurantId, CreateDishDto dto);
        Task<DishDto> GetDishById(int restaurantId, int dishId);
        Task<List<DishDto>> GetAll(int restaurantId);
    }
}

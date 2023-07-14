using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IDishService
    {
        Task<int> Create(int restaurantId, CreateDishDto dto);
    }
}

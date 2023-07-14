using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        Task<int> Create(CreateRestaurantDto dto);
        Task<IEnumerable<RestaurantDto>> GetAll();
        Task<RestaurantDto> GetById(int id);
        Task Delete(int id);
        Task Update(int id, UpdateRestaurantDto dto);
    }
}
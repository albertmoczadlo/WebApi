using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        Task<int> Create(CreateRestaurantDto dto);
        Task<IEnumerable<RestaurantDto>> GetAll();
        Task<RestaurantDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, UpdateRestaurantDto dto);
    }
}
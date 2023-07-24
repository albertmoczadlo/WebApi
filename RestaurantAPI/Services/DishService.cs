using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DishDto> GetDishById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(x=>x.Id == dishId);

            if(dish == null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }

        public async Task<List<DishDto>> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDto;
        }

        public async Task<int> Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dishEntity);
            await _dbContext.SaveChangesAsync();

            return dishEntity.Id;
        }

        public async Task RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            await _dbContext.SaveChangesAsync();
        }

        private Restaurant GetRestaurantById(int id)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(i => i.Id == id);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }
}

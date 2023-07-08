using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper,
            ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAll()
        {

            var restaurants = await _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .ToListAsync();

            var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantDtos;
        }

        public async Task<RestaurantDto> GetById(int id)
        {
            var restaurant = await _dbContext.Restaurants
              .Include(x => x.Address)
              .Include(x => x.Dishes)
              .FirstOrDefaultAsync(i => i.Id == id);

            if (restaurant == null) return null;

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public async Task<int> Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            await _dbContext.Restaurants.AddAsync(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public async Task<bool> Delete(int id)
        {
            _logger.LogError($"Restaurant whith id: {id} delete action invoked");

            var result = await _dbContext.Restaurants
                .FirstOrDefaultAsync(i => i.Id == id);

            if (result == null) return false;

            _dbContext.Restaurants.Remove(result);
            _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(int id, UpdateRestaurantDto dto)
        {
            var restaurant = await _dbContext.Restaurants
                .FirstOrDefaultAsync(i => i.Id == id);

            if(restaurant == null) return false;

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
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

            if (restaurant == null) throw new NotFoundException("Restaurant not found");

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

        public async Task Delete(int id)
        {

            var result = await _dbContext.Restaurants
                .FirstOrDefaultAsync(i => i.Id == id);

            if (result == null)
                throw new NotFoundException("Restaurant not found");

            _dbContext.Restaurants.Remove(result);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, UpdateRestaurantDto dto)
        {
            var restaurant = await _dbContext.Restaurants
                .FirstOrDefaultAsync(i => i.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            await _dbContext.SaveChangesAsync();
        }
    }
}

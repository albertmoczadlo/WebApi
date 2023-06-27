﻿using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.ProfileMap;

public class RestaurantMappingProfile : Profile
{
    public RestaurantMappingProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(m => m.City, c => c.MapFrom(m => m.Address.City))
            .ForMember(m=>m.Street, c => c.MapFrom(m=>m.Address.Street))
            .ForMember(m=>m.PostalCode, c=>c.MapFrom(m=>m.Address.PostalCode));

        CreateMap<Dish, DishDto>();

        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(r=>r.Address, c=>c.MapFrom(dto=>new Address()
            { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));
    }
}

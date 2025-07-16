using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Context;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly DataContext dataContext;
    private readonly IMapper mapper;

    public RestaurantRepository(DataContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }
    public async Task<Restaurant> CreateAsync(AddRestaurantDTO addRestaurantDTO)
    {
        var restaurantDM = mapper.Map<Restaurant>(addRestaurantDTO);
            await dataContext.Restaurants.AddAsync(restaurantDM);
            return restaurantDM;
    }

    public async Task<Restaurant> DeleteAsync(Restaurant restaurant)
    {
        await Task.Delay(0);
        dataContext.Restaurants.Remove(restaurant);
        return restaurant;
    }

    public async Task<PagedList<Restaurant>> GetAllAsync(UserParams userParams)
    {
        var query = dataContext.Restaurants;

        return await PagedList<Restaurant>.CreateAsync(query,userParams.PageNumber, userParams.PageSize);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await dataContext.Restaurants
            .Include(x => x.MenuItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedList<Restaurant>> GetPagedByIdAsync(UserParams userParams, int id)
    {
        var query = dataContext.Restaurants
                .Include(x => x.MenuItems)
                .Where(x => x.Id == id);
        // var query = dataContext.Orders.AsQueryable();
        //Total Price filtering
        if (userParams.MinPrice > 0)
        {
            query = query.Where(x => x.MenuItems.Any(m => m.Price >= userParams.MinPrice));
        }
        if (userParams.MaxPrice < decimal.MaxValue)
        {
            query = query.Where(x => x.MenuItems.Any(m => m.Price <= userParams.MaxPrice));
        }
        //Item name filtering
        if (!userParams.MenuItemName.IsNullOrEmpty())
        {
            query = query.Where(x => x.MenuItems.Any(m => m.Name.Contains(userParams.MenuItemName)));
        }
        //Item description filtering
        if (!userParams.MenuItemDescription.IsNullOrEmpty())
        {
            query = query.Where(x => x.MenuItems.Any(m => m.Description.ToLower().Contains(userParams.MenuItemDescription.ToLower())));
        }
        
        return await PagedList<Restaurant>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<bool> RestaurantExistsAsync(AddRestaurantDTO addRestaurantDTO)
    {
         return await dataContext.Restaurants.AnyAsync(
            x => x.Name.ToLower() == addRestaurantDTO.Name.ToLower()
            && x.LocationDescription.ToLower() == addRestaurantDTO.LocationDescription.ToLower()
            );
    }

    public async Task<bool> SaveAllAsync()
    {
         return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
    {
        await Task.Delay(0);
        dataContext.Restaurants.Update(restaurant);
        return restaurant;
    }
}

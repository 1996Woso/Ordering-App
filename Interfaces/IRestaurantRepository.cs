using System;
using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Interfaces;

public interface IRestaurantRepository
{
        Task<Restaurant> CreateAsync(AddRestaurantDTO addRestaurantDTO );
        Task<Restaurant> UpdateAsync(Restaurant restaurant);
        Task<Restaurant> DeleteAsync(Restaurant restaurant);
        Task<Restaurant?> GetByIdAsync(int id);
        Task<PagedList<Restaurant>> GetAllAsync(UserParams userParams);
        Task<bool> SaveAllAsync();
        Task<bool> RestaurantExistsAsync(AddRestaurantDTO addRestaurantDTO);
}

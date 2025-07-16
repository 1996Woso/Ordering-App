using System;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<PagedList<Order>> GetByEmployeeId(UserParams userParams, int id);
    Task<PagedList<Order>> GetAllAsync(UserParams userParams);
    Task<bool> SaveAllAsync();
    Task<List<string>> OrderStatusesAsync();
    Task<List<string>> PossibleStatusesAsync();


}

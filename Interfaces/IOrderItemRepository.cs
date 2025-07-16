using System;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem> CreateAsync(OrderItem orderItem);
    Task<OrderItem> UpdateAsync(OrderItem orderItem);
    Task<OrderItem?> GetByIdAsync(int id);
    Task<PagedList<OrderItem>> GetAllAsync(UserParams userParams);
    Task<bool> SaveAllAsync();
}

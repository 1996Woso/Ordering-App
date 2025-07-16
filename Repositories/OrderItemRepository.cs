using System;
using Ordering_App.Context;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly DataContext dataContext;

    public OrderItemRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        await dataContext.OrderItems.AddAsync(orderItem);
        await SaveAllAsync();
        return orderItem;
    }

    public Task<PagedList<OrderItem>> GetAllAsync(UserParams userParams)
    {
        throw new NotImplementedException();
    }

    public Task<OrderItem?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    public Task<OrderItem> UpdateAsync(OrderItem orderItem)
    {
        throw new NotImplementedException();
    }
}

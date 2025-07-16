using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Context;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext dataContext;

    public OrderRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    public async Task<Order> CreateAsync(Order order)
    {
        await dataContext.Orders.AddAsync(order);
        return order;
    }

    public async Task<PagedList<Order>> GetAllAsync(UserParams userParams)
    {
        var query = dataContext.Orders.AsQueryable();
        //Total Price filtering
        if (userParams.MinTotalPrice > 0) query = query.Where(x => x.TotalAmount>= userParams.MinTotalPrice);
        if (userParams.MaxTotalPrice < decimal.MaxValue) query = query.Where(x => x.TotalAmount <= userParams.MaxTotalPrice);
        //Last deposit month
        if (!userParams.OrderStatus.IsNullOrEmpty())
        {
            query = query.Where(x => x.Status == userParams.OrderStatus);
        }
        if (!userParams.OrderDate.IsNullOrEmpty())
        {
            DateTime orderDate =  DateTime.Parse(userParams.OrderDate!);
            query = query.Where(x => x.OrderDate.Date == orderDate.Date);
        }
     
        return await PagedList<Order>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public Task<PagedList<Order>> GetByEmployeeId(UserParams userParams, int id)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>> OrderStatusesAsync()
    {
        return await dataContext.Orders
            .Select(x => x.Status)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    public Task<Order> UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }
}

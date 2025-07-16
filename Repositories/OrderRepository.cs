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

    public async Task<PagedList<Order>> GetByEmployeeId(UserParams userParams, int id)
    {
       var query = dataContext.Orders.Where(x => x.EmployeeId == id).AsQueryable();
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

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await dataContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
    }

    public async Task<List<string>> OrderStatusesAsync()
    {
        return await dataContext.Orders
            .Select(x => x.Status)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<string>> PossibleStatusesAsync()
    {
        await Task.Delay(0);
        var list = new List<string>()
        {
            "Pending", "Preparing", "Delivering", "Delivered"
        };
        return list;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        await Task.Delay(0);
        dataContext.Orders.Update(order);
        return order;
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Context;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering_App.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public MenuItemRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }
        public async Task<MenuItem> CreateAsync(AddMenuItemDTO addMenuItemDTO)
        {
            var menuIdemDM = mapper.Map<MenuItem>(addMenuItemDTO);
            await dataContext.MenuItems.AddAsync(menuIdemDM);
            return menuIdemDM;
        }

        public Task<MenuItem> DeleteAsync(MenuItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<MenuItem>> GetAllAsync(UserParams userParams)
        {
            var query = dataContext.MenuItems.AsQueryable();
            //Price filtering
            if (userParams.MinPrice > 0) query = query.Where(x => x.Price >= userParams.MinPrice);
            if (userParams.MaxPrice < decimal.MaxValue) query = query.Where(x => x.Price <= userParams.MaxPrice);
            
            //Restaurant filtering
            if (!userParams.RestaurantName.IsNullOrEmpty())
            {
                int restaurantId = await dataContext.Restaurants
                    .Where(x => x.Name == userParams.RestaurantName)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                if (restaurantId > 0)
                {
                    query = query.Where(x => x.RestaurantId == restaurantId);
                }
            }
            //Item name filtering
            if (!userParams.MenuItemName.IsNullOrEmpty())
            {
                query = query.Where(x => x.Name.Contains(userParams.MenuItemName!));
            }
            // query = userParams.OrderBy switch
            // {
            //     "created" => query.OrderByDescending(x => x.CreatedAt),
            //     _ => query.OrderByDescending(x => x.LastActive)
            // };

            return await PagedList<MenuItem>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await dataContext.MenuItems
            .Include(x => x.Restaurant)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MenuItem>> GetSelectedItems(List<int> menuItemsIds)
        {
            var selectedItems = new List<MenuItem>();
            if (menuItemsIds.Count() > 0)
            {
                foreach (var menuItemId in menuItemsIds)
                {
                    var item = await GetByIdAsync(menuItemId);
                    if (item != null) selectedItems.Add(item);
                }
            }
            return selectedItems;
        }

        public async Task<bool> ItemExistsAsync(MenuItem menuItem)
        {
            return await dataContext.MenuItems.AnyAsync(
            x => x.Name.ToLower() == menuItem.Name.ToLower()
            && x.Description.ToLower() == menuItem.Description.ToLower()
            && x.RestaurantId == menuItem.RestaurantId
            );
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public Task<MenuItem> UpdateAsync(MenuItem item)
        {
            throw new NotImplementedException();
        }
    }
}

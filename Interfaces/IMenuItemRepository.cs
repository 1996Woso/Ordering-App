using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem> CreateAsync(AddMenuItemDTO addMenuItemDTO);
        Task<MenuItem> UpdateAsync(MenuItem item);
        Task<MenuItem> DeleteAsync(MenuItem item);
        Task<MenuItem?> GetByIdAsync(int id);
        Task<PagedList<MenuItem>> GetAllAsync(UserParams userParams);
        Task<bool> SaveAllAsync();
        Task<bool> ItemExistsAsync(MenuItem menuItem);
        Task<List<MenuItem>> GetSelectedItems(List<int> menuItemsIds);
    }
}

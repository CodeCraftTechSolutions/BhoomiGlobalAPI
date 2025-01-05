using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;


namespace BhoomiGlobalAPI.Service.Infrastructure
{
    public interface IMenuItemService
    {
        IEnumerable<MenuItemDTO> GetAll();
        Task<QueryResult<MenuItemDTO>> MenuItemList(QueryObject queryObject);
        Task<MenuItemDTO> GetMenuItemById(int id);
        MenuItemDTO GetMenuItemById(int Id, bool isAuthorized);
        MenuItemDTO GetMenuItemById(int Id, Expression<Func<MenuItem, bool>> where = null, params Expression<Func<MenuItem, object>>[] includeExpressions);
        Task<List<MenuItemDTO>> GetMenuItemByMenuCategoryId(int Id);
        Task<int> Create(MenuItemDTO model, Int64 userId);
        Task<int> Update(MenuItemDTO model, Int64 userId);
        Task Delete(int id);
        Task<bool> ChangeStatus(int id);
        Task<List<FooterMenuDTO>> FooterMenu();
        Task<List<HeaderMenuDTO>> HeaderMenuNew();
        Task<WebSettings> HeaderMenu();
        Task<AdditionFooterInformation> GetDefaultData();
        void SaveChanges();
    }
}

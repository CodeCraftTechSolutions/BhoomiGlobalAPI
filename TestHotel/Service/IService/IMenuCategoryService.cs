using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.Infrastructure
{
    public interface IMenuCategoryService
    {
        IEnumerable<MenuCategoryDTO> GetAll();
        Task<QueryResult<MenuCategoryDTO>> MenuCategoryList(QueryObject queryObject);
        Task<MenuCategoryDTO> GetMenuCategoryById(int id);
        Task<IEnumerable<SelectListItem>> GetParent();
        List<MenuCategoryDTO> menuCategory { get; }
        MenuCategoryDTO GetMenuCategoryById(int Id, bool isAuthorized);
        string GeneratePdfTemplateString(QueryResult<MenuCategoryDTO> menucategory);
        MenuCategoryDTO GetMenuCategoryById(int Id, Expression<Func<MenuCategory, bool>> where = null, params Expression<Func<MenuCategory, object>>[] includeExpressions);
        Task<int> Create(MenuCategoryDTO model,Int64 userId);
        Task<int> Update(MenuCategoryDTO model,Int64 userId);
        Task Delete(int id);
        Task<bool> ChangeStatus(int id);
        void SaveChanges();
    }
}

using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.Infrastructure
{
    public interface IPageCategoryService
    {
        IEnumerable<PageCategoryDTO> GetAll();

        
        Task<QueryResult<PageCategoryDTO>> PageCategoryList(QueryObject queryObject);
        Task<PageCategoryDTO> GetPageCategoryById(int id);
        PageCategoryDTO GetPageCategoryById(int Id, bool isAuthorized);
        PageCategoryDTO GetPageCategoryById(int Id, Expression<Func<PageCategory, bool>> where = null, params Expression<Func<PageCategory, object>>[] includeExpressions);
        Task<int> Create(PageCategoryDTO model);
        Task<int> Update(PageCategoryDTO model);
        Task Delete(int id);
        bool IsExisting(int pageCategoryCode);
        string GeneratePdfTemplateString(QueryResult<PageCategoryDTO> pagecategory);
        void SaveChanges();
    }
}

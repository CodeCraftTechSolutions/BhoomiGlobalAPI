using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;
namespace BhoomiGlobal.Service.Infrastructure
{
    public interface IPageService
    {
        PageBundle GetAllForWeb();
        PageDTO GetByIdForWeb(int id);
        IEnumerable<PageDTO> GetAll();
        Task<PageDTO> GetPageById(int id);
        Task<PageDTO> GetPageByCode(string code);
        Task<QueryResult<PageDTO>> PageList(QueryObjectPage queryObject);
        PageDTO GetPageById(int Id, bool isAuthorized);
        PageDTO GetPageById(int Id, Expression<Func<Page, bool>> where = null, params Expression<Func<Page, object>>[] includeExpressions);
        Task<int> Create(PageDTO model);
        Task<int> Update(PageDTO model);
        Task Delete(int id);
        Task PatchPageImage(PageImageCheckPrimaryDTO llImageDTO);
        Task UploadImage(int brandId, List<string> filepath);
        List<PageImage> GetPageImages(int PageId);
        Task<PageImageDTO> GetPageImage(int id);
        Task DeleteImages(int PageId);
        Task DeleteImage(int Id);/*Id::=Page Image's Id*/
        List<PageDTO> GetPageByPageCategoryId(int pageCategoryId);
        Task<List<PageDTO>> GetFAQ();
        Task<List<PageDTO>> Support();
        Task<PageDTO> AboutUs();
        Task<PageDTO> TermsAndConditionsWeb();
        string GeneratePdfTemplateString(QueryResult<PageDTO> pagecategory);
    }
}

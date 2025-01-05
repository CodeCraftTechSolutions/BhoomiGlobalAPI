using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.IService
{

    public interface INewsletterService
    {
        IEnumerable<NewsletterDTO> GetAll();
        Task<NewsletterDTO> GetNewsletterById(Int64 id);
        Task<NewsletterDTO> GetNewsletterByEmail(string Email);
        Task<QueryResult<NewsletterDTO>> NewsletterList(NewsletterQueryObject queryObject);
        NewsletterDTO GetNewsletterById(Int64 Id, bool isAuthorized);
        NewsletterDTO GetNewsletterById(Int64 Id, Expression<Func<Newsletter, bool>> where = null, params Expression<Func<Newsletter, object>>[] includeExpressions);
        Task<Int64> Create(NewsletterDTO model);
        Task<Int64> Update(NewsletterDTO model);
        Task<bool> Delete(Int64 id);
        Task<NewsletterAPIDTO> GetNewletterForAPI(Int64 id);
        string GeneratePdfTemplateString(QueryResult<NewsletterDTO> newsletters);
    }
}

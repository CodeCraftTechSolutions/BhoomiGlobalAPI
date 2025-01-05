using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface INewsletterSubscriberService
    {
        IEnumerable<NewsletterSubscriberDTO> GetAll();
        Task<NewsletterSubscriberDTO> GetNewsletterSubscriberById(Int64 id);
        Task<NewsletterSubscriberDTO> GetNewsletterSubscriberByEmail(string Email);
        Task<QueryResult<NewsletterSubscriberDTO>> NewsletterSubscriberList(NewsletterSubscriberQueryObject queryObject);
        NewsletterSubscriberDTO GetNewsletterSubscriberById(Int64 Id, bool isAuthorized);
        string GeneratePdfTemplateString(QueryResult<NewsletterSubscriberDTO> newlettersubscribers);
        NewsletterSubscriberDTO GetNewsletterSubscriberById(Int64 Id, Expression<Func<NewsletterSubscriber, bool>> where = null, params Expression<Func<NewsletterSubscriber, object>>[] includeExpressions);
        Task<Int64> Create(NewsletterSubscriberDTO model);
        Task<Int64> Update(NewsletterSubscriberDTO model);
        Task<bool> UpdateStatus(Int64 id);
        Task<bool> Delete(Int64 id);
        Task<bool> Delete(string email);
    }
}

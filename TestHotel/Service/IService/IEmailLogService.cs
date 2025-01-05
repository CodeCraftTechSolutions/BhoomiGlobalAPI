using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IEmailLogService
    {
        IEnumerable<EmailLogDTO> GetAll();
        Task<QueryResult<EmailLogDTO>> EmailLogList(EmailLogSearchQueryObject queryObject);
        Task<EmailLogDTO> GetEmailLogById(Int64 id);
        EmailLogDTO GetEmailLogById(Int64 Id, bool isAuthorized);
        string GeneratePdfTemplateString(QueryResult<EmailLogDTO> emailqueues);
        EmailLogDTO GetEmailLogById(Int64 Id, Expression<Func<EmailLog, bool>> where = null, params Expression<Func<EmailLog, object>>[] includeExpressions);
        Task<Int64> Create(EmailLogDTO model);
        Task<Int64> Update(EmailLogDTO model);
        Task Delete(Int64 id);
        void SaveChanges();
    }
}

using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IEmailQueueService
    {
        IEnumerable<EmailQueueDTO> GetAll();
        Task<QueryResult<EmailQueueDTO>> EmailQueueList(EmailQuerySearchQueryObject queryObject);
        string GeneratePdfTemplateString(QueryResult<EmailQueueDTO> emailqueues);
        Task<EmailQueueDTO> GetEmailQueueById(Int64 id);
        EmailQueueDTO GetEmailQueueById(Int64 Id, bool isAuthorized);
        EmailQueueDTO GetEmailQueueById(Int64 Id, Expression<Func<EmailQueue, bool>> where = null, params Expression<Func<EmailQueue, object>>[] includeExpressions);
        Task<Int64> Create(EmailQueueDTO model);
        Task<Int64> Update(EmailQueueDTO model);
        Task Delete(Int64 id);
        Task ProcessEmailQueue(EmailServerSetting emailSetting);
        Task<bool> CancelEmailQueue(Int64 id);
        void SaveChanges();
        Task<bool> ChangeStatus(Int64 id);
        EmailTemplate GetEmailTemplateByTemplateName(string templateName);
        Task InsertoEmailQueue(UserDetailsDTO userModel, string template
                               , string callbackurl = null, string data = "");
    }
}

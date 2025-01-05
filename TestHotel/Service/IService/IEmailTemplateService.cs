using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IEmailTemplateService
    {
        IEnumerable<EmailTemplateDTO> GetAll();
        Task<QueryResult<EmailTemplateDTO>> EmailTemplateList(EmailTemplateSearchQueryObject queryObject);
        EmailTemplateDTO GetEmailTemplateByName(string name);
        Task<EmailTemplateDTO> GetEmailTemplateById(int id);
        EmailTemplateDTO GetEmailTemplateById(int Id, bool isAuthorized);
        string GeneratePdfTemplateString(QueryResult<EmailTemplateDTO> emailqueues);
        EmailTemplateDTO GetEmailTemplateById(int Id, Expression<Func<EmailTemplate, bool>> where = null, params Expression<Func<EmailTemplate, object>>[] includeExpressions);
        Task<int> Create(EmailTemplateDTO model);
        Task<int> Update(EmailTemplateDTO model);
        Task Delete(int id);
        string UpdateEmailContent(List<KeyNamePair> keyNamePairs, string content);
        Task<bool> ChangeStatus(int id);
        void SaveChanges();
    }
}

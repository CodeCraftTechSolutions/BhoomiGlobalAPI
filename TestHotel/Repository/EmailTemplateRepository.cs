using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class EmailTemplateRepository : RepositoryBase<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}

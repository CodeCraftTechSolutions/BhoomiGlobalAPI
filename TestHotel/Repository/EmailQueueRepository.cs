using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class EmailQueueRepository : RepositoryBase<EmailQueue>, IEmailQueueRepository
    {
        public EmailQueueRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}

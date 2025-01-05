using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class NewsletterRepository : RepositoryBase<Newsletter>, INewsletterRepository
    {
        public NewsletterRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}

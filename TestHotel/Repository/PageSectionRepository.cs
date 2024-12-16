using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class PageSectionRepository : RepositoryBase<PageSection>,IPageSectionRepository
    {
        public PageSectionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}

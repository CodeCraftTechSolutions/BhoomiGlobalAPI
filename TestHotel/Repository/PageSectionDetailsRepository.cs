using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class PageSectionDetailsRepository: RepositoryBase<PageSectionDetails>, IPageSectionDetailsRepository
    {
        public PageSectionDetailsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}

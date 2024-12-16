using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;


namespace BhoomiGlobalAPI.Repository
{
    public class PageCategoryRepository : RepositoryBase<PageCategory>, IPageCategoryRepository
    {
        public PageCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}

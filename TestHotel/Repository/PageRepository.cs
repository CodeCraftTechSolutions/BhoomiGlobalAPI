using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class PageRepository:RepositoryBase<Page>,IPageRepository
    {
        public PageRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}

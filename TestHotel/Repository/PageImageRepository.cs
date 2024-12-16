using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class PageImageRepository:RepositoryBase<PageImage>,IPageImageRepository
    {
        public PageImageRepository(IDbFactory dbFactory): base(dbFactory)
        {

        }
    }
}

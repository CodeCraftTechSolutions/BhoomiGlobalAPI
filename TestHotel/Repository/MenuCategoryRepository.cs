using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class MenuCategoryRepository : RepositoryBase<MenuCategory>,IMenuCategoryRepository
    {
        public MenuCategoryRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}

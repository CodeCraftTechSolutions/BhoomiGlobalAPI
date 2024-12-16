using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;
namespace BhoomiGlobalAPI.Repository
{
    public class MenuItemRepository:RepositoryBase<MenuItem>,IMenuItemRepository
    {
        public MenuItemRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}

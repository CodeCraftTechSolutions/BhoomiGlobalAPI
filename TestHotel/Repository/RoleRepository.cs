using BhoomiGlobaAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;

namespace BhoomiGlobaAPI.Repository.Repository
{
    public class RoleRepository: RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }

    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }


}

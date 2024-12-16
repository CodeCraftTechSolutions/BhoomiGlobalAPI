using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Common;

namespace BhoomiGlobaAPI.Repository.Infrastructure
{
    public interface IRoleRepository : IRepository<Role>
    {
    }

    public interface IUserRoleRepository : IRepository<UserRole>
    {

    }
}

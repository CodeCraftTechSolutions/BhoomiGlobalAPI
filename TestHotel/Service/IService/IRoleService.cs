using BhoomiGlobalAPI.DTO;


namespace BhoomiGlobalAPI.Service.IService
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetAll();
        Task<long> Update(RoleDTO model);
        Task<RoleDTO> GetRoleById(Int64 id);
        Task<Int64> Create(RoleDTO model);
        Task Delete(Int64 id);
        void SaveChanges();
    }
}

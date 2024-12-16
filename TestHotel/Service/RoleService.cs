using AutoMapper;
using BhoomiGlobaAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTO;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Service.IService;

namespace BhoomiGlobalAPI.Service
{
    public class RoleService:IRoleService
    {
        IRoleRepository _roleRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IUserRoleRepository _userRoleRepository;
        public RoleService(IRoleRepository roleRepository,IMapper mapper, IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRoleRepository = userRoleRepository;
        }

        public IEnumerable<RoleDTO> GetAll()
        {
            IEnumerable<Role> roles = _roleRepository.GetAll();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }


        public async Task<RoleDTO> GetRoleById(Int64 id)
        {
            var roles = await _roleRepository.GetSingle(id);
            if (roles == null)
                return new RoleDTO();
            return _mapper.Map<RoleDTO>(roles);
        }


        public async Task<Int64> Create(RoleDTO model)
        {
            Role obj = _mapper.Map<Role>(model);
            await _roleRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }


        public async Task<long> Update(RoleDTO model)
        {

            var roles = await _roleRepository.GetSingle(model.Id);
            if (roles != null)
            {
                roles.Id = model.Id;
                roles.Name = model.Name;
                roles.RoleId = model.RoleId;
                roles.DisplayName = model.DisplayName;
                roles.GroupNo = model.GroupNo ?? 0;
                roles.ShowStoreDropDown = model.ShowStoreDropDown;
                roles.IsUniqueRole = model.IsUniqueRole;
                await _unitOfWork.Commit();
            }
            return roles.Id;
        }


        public async Task Delete(Int64 id)
        {
            var role = await _roleRepository.GetSingle(id);
            _roleRepository.Delete(role);
            await _unitOfWork.Commit();
        }


        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
    }
}

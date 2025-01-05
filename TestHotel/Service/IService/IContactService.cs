using BhoomiGlobalAPI.DTOs;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IContactService
    {
        List<ContactDTO> GetAll();
        Task<bool> Delete(long id);
        Task<long> Add(ContactDTO model);
        ContactDTO GetById(long id);
    }
}

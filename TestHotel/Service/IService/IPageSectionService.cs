using BhoomiGlobalAPI.DTOs;

namespace BhoomiGlobalAPI.Service.Infrastructure
{
    public interface IPageSectionService
    {
        Task<bool> Create(PageSectionUpsertDTO model);
        Task<bool> Update(PageSectionUpsertDTO model);
    }
}
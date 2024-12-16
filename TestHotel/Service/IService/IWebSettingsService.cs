using BhoomiGlobalAPI.DTO;
namespace BhoomiGlobalAPI.Service.Infrastructure
{

    public interface IWebSettingsService
    {
        Task<int> Create(WebSettingsDTO model);
        Task<int> Update(WebSettingsDTO model);
        WebSettingsDTO GetFirstOrDefault();
        Task UploadImagePlayStoreImage(string filepath);
        WebSettingsDTO GetPlayStoreImage();
        WebSettingsDTO GetSiteLogoImage();
        WebSettingsDTO GetSiteFaviconImage();
        void SaveChanges();
    }
}

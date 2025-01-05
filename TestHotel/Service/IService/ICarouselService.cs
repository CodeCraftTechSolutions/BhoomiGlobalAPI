using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface ICarouselService
    {
        Task<QueryResult<CarouselModifiedDTO>> GetQueryCarousels(CarouselQueryObject query);
        Task<bool> PatchOrders(List<PatchOrderDTO> patchorders);
        Task<int> Create(CarouselFewerItemsDTO model);
        Task<int> Update(CarouselFewerItemsDTO model);
        Task UploadImage(int carouselId, string filepath);
        Task UploadHomeBannerSmall(int carouselId, string filepath);
        CarouselImageUrlDTO GetCarouselImage(int carouselId);
        CarouselImageUrlDTO GetCarouselHomeBannerSmallImage(int carouselId);
        CarouselImagePathDTO GetCarouselImagePath(int carouselId);
        CarouselImagePathDTO GetCarouselHomeBannerSmallImagePath(int carouselId);
        Task Delete(int Id);
        string GetImageName(int Id);
        string GetHomeBannerSmallImageName(int Id);
        List<CarouselStringifiedDTO> GetAllByType(int typeId);
        Task<CarouselModifiedDTO> GetCarouselById(int id);

        Task PatchCarousel(CarouselMobileDTO carmob);
        Task ArrangeOrder(List<SliderOrder> SliderOrder);
        List<CarouselStringifiedDTO> GetAll(int storeId, long brandId, int affiliateId, int homepageId);
        void SaveChanges();
    }
}

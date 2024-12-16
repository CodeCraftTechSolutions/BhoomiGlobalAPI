using AutoMapper;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTO;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Service.Infrastructure;

namespace BhoomiGlobalAPI.Service
{
    public class WebSettingsService:IWebSettingsService
    {
        IWebSettingsRepository _webSettingsRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public WebSettingsService(IWebSettingsRepository webSettingsRepository,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _webSettingsRepository = webSettingsRepository; 
            _mapper = mapper;
            
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Create(WebSettingsDTO model)
        {

            WebSettings obj = _mapper.Map<WebSettings>(model);
            obj.AboutUs = model.AboutUs;
            obj.FacebookUrl = model.FacebookUrl;
            obj.TwitterUrl = model.TwitterUrl;
            obj.InstagramUrl = model.InstagramUrl;
            obj.AddressDetail = model.AddressDetail;
            obj.OurServiceCategoryId = model.OurServiceCategoryId;
            obj.FooterMenuCategoryId = model.FooterMenuCategoryId;
            obj.HeaderMenuCategoryId = model.HeaderMenuCategoryId;
            obj.PlayStoreUrl = model.PlayStoreUrl;
            obj.AppStoreUrl = model.AppStoreUrl;
            obj.FaqCategoryId = model.FaqCategoryId;
            obj.SupportPageId = model.SupportPageId;
            obj.CustomerSupport = model.CustomerSupport;
            obj.AboutUsPageId = model.AboutUsPageId;
            obj.ContactUsPageId = model.ContactUsPageId;
            obj.ServicePageId = model.ServicePageId;
            obj.SiteLogoImageUrl = model.SiteLogoImageUrl;
            obj.SiteFaviconImageUrl = model.SiteFaviconImageUrl;
            await _webSettingsRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }
        public async Task<int> Update(WebSettingsDTO model)
        {

            var obj = _webSettingsRepository.All.FirstOrDefault();
            if (obj != null)
            {
                obj.AboutUs = model.AboutUs;
                obj.FacebookUrl = model.FacebookUrl;
                obj.TwitterUrl = model.TwitterUrl;
                obj.InstagramUrl = model.InstagramUrl;
                obj.AddressDetail = model.AddressDetail;
                obj.OurServiceCategoryId = model.OurServiceCategoryId;
                obj.FooterMenuCategoryId = model.FooterMenuCategoryId;
                obj.HeaderMenuCategoryId = model.HeaderMenuCategoryId;
                obj.PlayStoreUrl = model.PlayStoreUrl;
                obj.AppStoreUrl = model.AppStoreUrl;
                obj.FaqCategoryId = model.FaqCategoryId;
                obj.SupportPageId = model.SupportPageId;
                obj.CustomerSupport = model.CustomerSupport;
                obj.PlayStoreImageUrl = model.PlayStoreImageUrl;
                obj.AppleStoreImageUrl = model.AppleStoreImageUrl;
                obj.CustomerSupportEmail = model.CustomerSupportEmail;
                obj.AboutUsPageId = model.AboutUsPageId;
                obj.ContactUsPageId = model.ContactUsPageId;
                obj.ServicePageId = model.ServicePageId;
                obj.SiteLogoImageUrl = model.SiteLogoImageUrl;
                obj.SiteFaviconImageUrl = model.SiteFaviconImageUrl;
                obj.OurClientSaysCategoryId = model.OurClientSaysCategoryId;
                await _unitOfWork.Commit();
            }
            return obj.Id;
        }
        public WebSettingsDTO GetFirstOrDefault()
        {
            var result = _webSettingsRepository.All.FirstOrDefault();
            if(result == null) { return new WebSettingsDTO(); }
            return _mapper.Map<WebSettingsDTO>(result);
        }
        public WebSettingsDTO GetPlayStoreImage()
        {
            var wb = _webSettingsRepository.All.FirstOrDefault();
            var wb_ = new WebSettingsDTO()
            {

                PlayStoreImageUrl = wb.PlayStoreImageUrl == null ? null : "UploadsPlayStoreImage" + "\\" + wb.PlayStoreImageUrl
            };
            return wb_;
        }
        public WebSettingsDTO GetSiteLogoImage()
        {
            var wb = _webSettingsRepository.All.FirstOrDefault();
            var wb_ = new WebSettingsDTO()
            {

                SiteLogoImageUrl = "UploadsPlayStoreImage/" + wb.SiteLogoImageUrl
            };
            return wb_;
        }
        public WebSettingsDTO GetSiteFaviconImage()
        {
            var wb = _webSettingsRepository.All.FirstOrDefault();
            var wb_ = new WebSettingsDTO()
            {

                SiteFaviconImageUrl = "UploadsPlayStoreImage/" + wb.SiteFaviconImageUrl
            };
            return wb_;
        }

        public async Task UploadImagePlayStoreImage(string filepath)
        {
            var websettings = _webSettingsRepository.All.FirstOrDefault() ;
            websettings.PlayStoreImageUrl = filepath;
             await _unitOfWork.Commit();
        }
        public async Task<WebSettingsDTO> GetWebSettingsById(int id)
        {
            var websettings = await _webSettingsRepository.GetSingle(id);
            if (websettings == null) return new WebSettingsDTO();
            return _mapper.Map<WebSettingsDTO>(websettings);
        }
        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
    }
   
}

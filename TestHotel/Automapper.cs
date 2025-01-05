using AutoMapper;
using BhoomiGlobalAPI.DTO;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;

namespace BhoomiGlobalAPI
{
    public class AutoMapping: Profile
    {
        public static IMapper Automapperinitializer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDetailsDTO, UserDetails>().ReverseMap();
                cfg.CreateMap<UserRegistrationDTO, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email))
                .ReverseMap();
                cfg.CreateMap<UserRegistrationDTO, UserDetails>().ReverseMap();
                cfg.CreateMap<Role, RoleDTO>().ReverseMap();
                cfg.CreateMap <MenuCategory, MenuCategoryDTO>().ReverseMap();
                cfg.CreateMap<MenuItem, MenuItemDTO>().ReverseMap();
                cfg.CreateMap<Page, PageDTO>().ReverseMap();
                cfg.CreateMap<PageSection, PageSectionInputDTO>().ReverseMap();
                cfg.CreateMap<PageCategory, PageCategoryDTO>().ReverseMap();
                cfg.CreateMap<PageSection, PageSectionDTO>().ReverseMap();
                cfg.CreateMap<PageSectionDetails, PageSectionDetailsDTO>().ReverseMap();
                cfg.CreateMap<WebSettings, WebSettingsDTO>().ReverseMap();

                cfg.CreateMap<EmailQueueDTO, EmailQueue>().ReverseMap();
                cfg.CreateMap<EmailLogDTO, EmailLog>().ReverseMap();
                cfg.CreateMap<EmailTemplateDTO, EmailTemplate>().ReverseMap();

                cfg.CreateMap<NewsletterSubscriberDTO, NewsletterSubscriber>().ReverseMap();
                cfg.CreateMap<NewsletterDTO, Newsletter>().ReverseMap();
                cfg.CreateMap<MenuCategoryDTO, MenuCategory>().ReverseMap();
                cfg.CreateMap<CarouselDTO, Carousel>().ReverseMap();
                cfg.CreateMap<ContactDTO, Contact>().ReverseMap();
            });



            return config.CreateMapper();
        }

        private static void CreateMap<T1, T2>()
        {
            throw new NotImplementedException();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BhoomiGlobalAPI.Entities
{
    public class WebSettings
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string AboutUs { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string AddressDetail { get; set; }
        public string PlayStoreUrl { get; set; }
        public string AppStoreUrl { get; set; }
        [ForeignKey("OurServiceCategoryId")]
        public int? OurServiceCategoryId { get; set; }
        [ForeignKey("FooterMenuCategoryId")]
        public int? FooterMenuCategoryId { get; set; }

        [ForeignKey("HeaderMenuCategoryId")]
        public int? HeaderMenuCategoryId { get; set; }
        public virtual PageCategory OurServiceCategory { get; set; }
        [ForeignKey("FaqCategoryId")]
        public int? FaqCategoryId { get; set; }
        public int? WebTCId { get; set; }
        [ForeignKey("SupportPageId")]
        public int? SupportPageId { get; set; }
        public string CustomerSupport { get; set; }
        public string PlayStoreImageUrl { get; set; }
        public string AppleStoreImageUrl { get; set; }
        public string CustomerSupportEmail  { get; set; }
        public int AboutUsPageId { get; set; }
        public int ContactUsPageId { get; set; }
        public int? OurClientSaysCategoryId { get; set; }
        public int ServicePageId { get; set; }
        public string SiteLogoImageUrl { get; set; }
        public string SiteFaviconImageUrl { get; set; }

        public virtual Page SupportPage { get; set; }
        public virtual PageCategory FaqCategory { get; set; }
        public virtual MenuCategory FooterMenuCategory { get; set; }
        public virtual MenuCategory HeaderMenuCategory { get; set; }

    }
}

      
      
      
      
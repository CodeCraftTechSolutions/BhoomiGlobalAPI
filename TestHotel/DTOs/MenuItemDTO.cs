namespace BhoomiGlobalAPI.DTOs
{
    public class MenuItemDTO
    {
        
        public int Id { get; set; }
        public int MenuCategoryId { get; set; }
        public string? EntityCategoryName { get; set; }
        public int EntityId { get; set; }
        public string? EntityName { get; set; }
        public int Status { get; set; }
        public int MenuTypeId { get; set; }
        public string? MenuTypeName { get; set; }
        public int EntityCategoryId { get; set; }
        public string Url { get; set; }
        public string? CodeUrl { get; set; }
        public Int64 CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 ModifiedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual UserDetailsDTO? UserDetails { get; set; }
        public virtual ICollection<MenuItemDTO>? MenuItems { get; set; }
    }

    public class FooterMenuDTO
    {
        public FooterMenuDTO()
        {
            AdditionData = new AdditionFooterInformation();
        }
        public string Name { get; set; }
        public bool IsParent { get; set; }
        public string Url { get; set; }
        public string CodeUrl { get; set; }
        public List<FooterMenuDTO> FooterMenu { get; set; }
        public AdditionFooterInformation AdditionData { get; set; }
    }    
    public class HeaderMenuDTO
    {
        public HeaderMenuDTO()
        {
            AdditionData = new AdditionFooterInformation();
        }
        public string Name { get; set; }
        public bool IsParent { get; set; }
        public string Url { get; set; }
        public string CodeUrl { get; set; }
        public List<HeaderMenuDTO> HeaderMenu { get; set; }
        public AdditionFooterInformation AdditionData { get; set; }
    }
    public class AdditionFooterInformation
    {
        public string SiteLogoUrl { get; set; }
        public string AboutUs { get; set; }
        public string Address { get; set; }
        public string AboutOffice { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public int TermsOfServiceId { get; set; }
        public int PrivacyPolicyId { get; set; }
        public int ContactUsId { get; set; }
        public int AboutUsId { get; set; }
        public string GoogleMapUrl { get; set; }
       
    }
    
}

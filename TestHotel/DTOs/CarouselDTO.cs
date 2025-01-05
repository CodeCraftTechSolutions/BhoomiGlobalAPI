using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.DTOs
{
    public class CarouselDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OrderBy { get; set; }
        public string ImagePath { get; set; }
        public int CarouselType { get; set; }
        public string? TargetUrl { get; set; }
        public string? HomeBannerSmall { get; set; }
        public string? Slider { get; set; }
    }
    public class CarouselFewerItemsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CarouselType { get; set; }
        public string? TargetUrl { get; set; }
        public string? Slider { get; set; }
        public int StoreId { get; set; }
        public long BrandId { get; set; }
    }
    public class CarouselModifiedDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OrderBy { get; set; }
        public string ImageUrl { get; set; }
        public int CarouselType { get; set; }
        public string CarouselType_ { get; set; }
        public string TargetUrl { get; set; }
        public long BrandId { get; set; }
        public int StoreId { get; set; }
        public string? HomeBannerSmall { get; set; }
        public string? Slider { get; set; }
    }
    public class CarouselStringifiedDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OrderBy { get; set; }
        public string ImageUrl { get; set; }
        public string CarouselType { get; set; }
        public string TargetUrl { get; set; }
        public int? TargetModule { get; set; }
        public int? EntityId { get; set; }
        public int? CategoryId { get; set; }
        public string? HomeBannerSmall { get; set; }
        public string? Slider { get; set; }
    }
    public class CarouselImageUrlDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
    public class CarouselImagePathDTO
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
    public class CarouselMobileDTO
    {
        public int Id { get; set; }
        public int TargetModule { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
    }

    public class SliderImage
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
    }

    public class SliderOrder
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int StoreId { get; set; }
        public int BrandId { get; set; }
    }
}

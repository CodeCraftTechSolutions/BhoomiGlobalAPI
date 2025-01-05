using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhoomiGlobalAPI.Entities
{
    public class Carousel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int OrderBy { get; set; }
        [StringLength(1000)]
        public string? ImagePath { get; set; }
        public string? TargetUrl { get; set; }
        public int? CarouselType { get; set; }
        #region mobile-navigator
        public int? TargetModule { get; set; }
        /*TargetModule Enum: Store=1, Product=2,Page=3*/
        public int? CategoryId { get; set; }
        public int? EntityId { get; set; }
        #endregion mobile-navigator
        public string? homeBannerSmall { get; set; }
        public string? Slider { get; set; }
    }
}

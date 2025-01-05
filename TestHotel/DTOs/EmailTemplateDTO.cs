using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.DTOs
{
    public class EmailTemplateDTO
    {
        public int EmailTemplateId { get; set; }
        [Required]
        public string TemplateName { get; set; }
        public string EmailFrom { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string HTMLWrapper { get; set; }
        public int Active { get; set; }
    }
}

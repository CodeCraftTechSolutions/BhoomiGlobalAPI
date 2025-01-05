using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.Entities
{
    public class EmailTemplate
    {
        public int EmailTemplateId { get; set; }
        public string TemplateName { get; set; }
        [Display(Name = "Email From")]
        public string EmailFrom { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string HTMLWrapper { get; set; }
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Is Active")]
        public int Active { get; set; }
    }
}

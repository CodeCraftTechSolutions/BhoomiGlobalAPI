namespace BhoomiGlobalAPI.DTOs
{
    public class EmailMessageDTO
    {
        public string TemplateName { get; set; }
        public string Message { get; set; }
        public List<EmailNameDTO> ToEmail { get; set; }
        public List<EmailNameDTO> CcEmail { get; set; }
        public List<EmailNameDTO> BccEmail { get; set; }
        public string CallBackUrl { get; set; }
        public String Attachements { get; set; }
    }

    public class EmailNameDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}

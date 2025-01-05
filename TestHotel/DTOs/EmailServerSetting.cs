namespace BhoomiGlobalAPI.DTOs
{
    public class EmailServerSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AttachmentPath { get; set; }
        public string AirlineLogoPath { get; set; }
    }
}

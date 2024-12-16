namespace BhoomiGlobalAPI.DTOs
{
  

    public class PageCategoryCodeSettings
    {
        public int OurServices { get; set; }
    }

    public class PhotoSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
    }

    public class VideoSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
    }

    public class AuthenticationSettings
    {
        public string ClientId { get; set; }
        public string Issuer { get; set; }
    }
}

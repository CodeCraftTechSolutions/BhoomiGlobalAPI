namespace BhoomiGlobalAPI.DTOs
{
    public class APIResponseDTO<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

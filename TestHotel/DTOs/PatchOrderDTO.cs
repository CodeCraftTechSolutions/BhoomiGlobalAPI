namespace BhoomiGlobalAPI.DTOs
{
    public class PatchOrderDTO
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
    public class PatchOrderInt64DTO
    {
        public Int64 Id { get; set; }
        public int Order { get; set; }
    }

    public class PatchOrderBundle
    {
        public int ProductId { get; set; }
        public List<PatchOrderDTO> PatchOrderList { get; set; }
    }
}

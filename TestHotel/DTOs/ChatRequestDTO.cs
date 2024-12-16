using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.DTOs
{
    public class ChatRequestDTO
    {
        [Required(ErrorMessage = "Prompt is required.")]
        [StringLength(1000, ErrorMessage = "Prompt cannot exceed 1000 characters.")]
        public string Prompt { get; set; }

    }
}

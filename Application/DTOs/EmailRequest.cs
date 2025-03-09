using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class EmailRequest
    {
        [Required(ErrorMessage = "Recipient email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message body is required.")]
        public string Body { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class EmailConfiguration
    {
        [Required(ErrorMessage = "IMAP Server is required.")]
        public string ImapServer { get; set; } = string.Empty;

        [Required(ErrorMessage = "IMAP Port is required.")]
        [Range(1, 65535, ErrorMessage = "IMAP Port must be between 1 and 65535.")]
        public int ImapPort { get; set; } = 993;

        [Required(ErrorMessage = "SMTP Server is required.")]
        public string SmtpServer { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP Port is required.")]
        [Range(1, 65535, ErrorMessage = "SMTP Port must be between 1 and 65535.")]
        public int SmtpPort { get; set; } = 587;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

}

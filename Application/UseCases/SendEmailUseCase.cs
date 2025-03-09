using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.UseCases
{
    public class SendEmailUseCase
    {
        private readonly ISmtpEmailService _smtpEmailService;

        public SendEmailUseCase(ISmtpEmailService smtpEmailService)
        {
            _smtpEmailService = smtpEmailService;
        }

        public async Task<bool> InvokeAsync(EmailRequest request)
        {
            try
            {
                await _smtpEmailService.SendEmailAsync(request.To, request.Subject, request.Body);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}

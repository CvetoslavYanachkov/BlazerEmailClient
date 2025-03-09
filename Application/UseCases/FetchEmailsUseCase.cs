using Infrastructure.Interfaces;

namespace Application.UseCases
{
    public class FetchEmailsUseCase
    {
        private readonly IImapEmailService _imapEmailService;

        public FetchEmailsUseCase(IImapEmailService imapEmailService)
        {
            _imapEmailService = imapEmailService;
        }

        public async Task<string> InvokeAsync()
        {
            return await _imapEmailService.GetEmailsAsync();
        }
    }
}

namespace Infrastructure.Interfaces
{
    public interface IImapEmailService
    {
        Task<string> GetEmailsAsync();
    }
}

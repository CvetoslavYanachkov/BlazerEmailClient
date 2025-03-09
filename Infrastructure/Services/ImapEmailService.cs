using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Shared.Models;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

public class ImapEmailService : IImapEmailService
{
    private readonly EmailConfiguration _config;

    public ImapEmailService(IOptions<EmailConfiguration> options)
    {
        _config = options.Value;
    }

    public async Task<string> GetEmailsAsync()
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(_config.ImapServer, _config.ImapPort);

            using var stream = new SslStream(client.GetStream());
            await stream.AuthenticateAsClientAsync(_config.ImapServer);

            using var reader = new StreamReader(stream, Encoding.ASCII);
            using var writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

            // Read initial server response
            string response = await reader.ReadLineAsync() ?? string.Empty;

            // Authenticate
            await writer.WriteLineAsync($"A01 LOGIN \"{_config.Email}\" \"{_config.Password}\"");
            response = await ReadResponseAsync(reader);
            if (response.Contains("NO") || response.Contains("BAD"))
                throw new Exception("IMAP Authentication failed.");

            // Select inbox
            await writer.WriteLineAsync("A02 SELECT INBOX");
            response = await ReadResponseAsync(reader);
            if (response.Contains("NO") || response.Contains("BAD"))
                throw new Exception("Failed to select INBOX.");

            // Fetch emails
            await writer.WriteLineAsync("A03 FETCH 6:1 (RFC822)");
            response = await ReadResponseAsync(reader);

            // Logout
            await writer.WriteLineAsync("A04 LOGOUT");
            await ReadResponseAsync(reader);

            return ParseMultipleEmails(response);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Network error: {ex.Message}");
            return "IMAP connection failed.";
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");
            return "IMAP read/write error.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General IMAP error: {ex.Message}");
            return "An error occurred while fetching emails.";
        }
    }


    private async Task<string> ReadResponseAsync(StreamReader reader)
    {
        StringBuilder response = new StringBuilder();
        string? line;
        do
        {
            line = await reader.ReadLineAsync();
            if (line == null) break;
            response.AppendLine(line);
        }
        while (line != null && !line.StartsWith("A0"));

        return response.ToString();
    }

    private string ParseMultipleEmails(string rawResponse)
    {
        StringBuilder formattedEmails = new StringBuilder();

        var emailMatches = Regex.Split(rawResponse, @"\*\s+\d+\s+FETCH");

        foreach (var emailData in emailMatches)
        {
            if (!string.IsNullOrWhiteSpace(emailData))
            {
                string parsedEmail = EmailParser.ParseEmail(emailData);
                formattedEmails.AppendLine(ConvertToHtml(parsedEmail));
                formattedEmails.AppendLine("<hr>");
            }
        }

        return formattedEmails.ToString();
    }

    private string ConvertToHtml(string emailBody)
    {
        // Convert new lines to <br> for HTML rendering
        emailBody = emailBody.Replace("\n", "<br>");

        // Convert [image: XYZ] placeholders into HTML <img> tags
        emailBody = Regex.Replace(emailBody, @"\[image:\s*(.*?)\]", "<img src='/email-images/$1' alt='$1' style='max-width:100px;'>");

        // Convert links into clickable <a> tags
        emailBody = Regex.Replace(emailBody, @"<(https?://[^>]+)>", "<a href='$1' target='_blank'>$1</a>");

        return emailBody;
    }
}

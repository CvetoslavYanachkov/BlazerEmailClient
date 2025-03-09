using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Models;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly EmailConfiguration _config;

    public SmtpEmailService(IOptions<EmailConfiguration> options)
    {
        _config = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_config.SmtpServer, _config.SmtpPort);

        using var stream = new SslStream(client.GetStream());
        await stream.AuthenticateAsClientAsync(_config.SmtpServer);

        using var reader = new StreamReader(stream, Encoding.ASCII);
        using var writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

        // Read initial server response
        await ReadResponseAsync(reader);

        await SendCommandAsync("EHLO localhost", writer, reader);

        // Start authentication
        await SendCommandAsync("AUTH LOGIN", writer, reader);

        // Send encoded email
        await SendCommandAsync(Convert.ToBase64String(Encoding.ASCII.GetBytes(_config.Email)), writer, reader, false);
        string response = await ReadResponseAsync(reader);
        if (!response.StartsWith("334"))
        {
            throw new Exception("Unexpected response after username: " + response);
        }

        // Send encoded password
        await SendCommandAsync(Convert.ToBase64String(Encoding.ASCII.GetBytes(_config.Password)), writer, reader, false);
        response = await ReadResponseAsync(reader);
        if (!response.StartsWith("235"))
        {
            throw new Exception("Authentication failed: " + response);
        }

        Console.WriteLine("Authentication successful!");

        await SendCommandAsync($"MAIL FROM:<{_config.Email}>", writer, reader);
        await SendCommandAsync($"RCPT TO:<{to}>", writer, reader);
        await SendCommandAsync("DATA", writer, reader);

        // Properly format email body
        string emailData = $"From: {_config.Email}\r\nTo: {to}\r\nSubject: {subject}\r\n\r\n{body}\r\n.\r\n";
        await SendCommandAsync(emailData, writer, reader);

        await SendCommandAsync("QUIT", writer, reader);
    }

    private async Task SendCommandAsync(string command, StreamWriter writer, StreamReader reader, bool readResponse = true)
    {
        writer.WriteLine(command);
        writer.Flush();
        Console.WriteLine("Sent: " + command);

        if (readResponse)
        {
            string response = await ReadResponseAsync(reader);
            Console.WriteLine("Response: " + response);
        }
    }

    private async Task<string> ReadResponseAsync(StreamReader reader)
    {
        StringBuilder response = new StringBuilder();
        string line;
        int maxAttempts = 20;

        while (maxAttempts-- > 0)
        {
            line = await reader.ReadLineAsync() ?? string.Empty;
            if (line == null)
                break;

            response.AppendLine(line);
            Console.WriteLine("Server: " + line);

            // Stop reading when last line of response is reached
            if (line.Length >= 4 && char.IsDigit(line[0]) && char.IsDigit(line[1]) && char.IsDigit(line[2]) && line[3] == ' ')
            {
                break;
            }
        }

        if (maxAttempts <= 0)
        {
            throw new TimeoutException("SMTP server did not respond properly.");
        }

        return response.ToString();
    }
}
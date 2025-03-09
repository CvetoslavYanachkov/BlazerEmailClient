using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Utils;

public class EmailParser
{
    public static string ParseEmail(string rawEmail)
    {
        try
        {
            StringBuilder formattedEmail = new StringBuilder();

            // Extract sender
            Match fromMatch = Regex.Match(rawEmail, @"From:\s*(.*?)\s*<(.+?)>");
            string sender = fromMatch.Success ? $"{fromMatch.Groups[1].Value} ({fromMatch.Groups[2].Value})" : "Unknown Sender";
            formattedEmail.AppendLine($"📩 **From:** {sender}");

            // Extract subject
            Match subjectMatch = Regex.Match(rawEmail, @"Subject:\s*(.+)");
            string subject = subjectMatch.Success ? subjectMatch.Groups[1].Value : "No Subject";
            formattedEmail.AppendLine($"📝 **Subject:** {subject}");

            // Extract body (handle quoted-printable)
            string body = ExtractBody(rawEmail);
            formattedEmail.AppendLine($"\n📄 **Message:**\n{body}");

            return formattedEmail.ToString();
        }
        catch (Exception ex)
        {
            return $"⚠️ Error parsing email: {ex.Message}";
        }
    }

    private static string ExtractBody(string rawEmail)
    {
        // Detect quoted-printable encoding and decode it
        Match quotedPrintableMatch = Regex.Match(rawEmail, @"Content-Transfer-Encoding:\s*quoted-printable([\s\S]+?)(?=--|\z)", RegexOptions.IgnoreCase);
        if (quotedPrintableMatch.Success)
        {
            return DecodeQuotedPrintable(quotedPrintableMatch.Groups[1].Value);
        }

        // If not quoted-printable, extract plain text
        Match plainTextMatch = Regex.Match(rawEmail, @"Content-Type:\s*text/plain;\s*charset=UTF-8([\s\S]+?)(?=--|\z)", RegexOptions.IgnoreCase);
        return plainTextMatch.Success ? plainTextMatch.Groups[1].Value.Trim() : "No message content.";
    }

    private static string DecodeQuotedPrintable(string input)
    {
        input = input.Replace("=\r\n", ""); // Remove soft line breaks
        return Regex.Replace(input, @"=([0-9A-F]{2})", match => ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString());
    }
}

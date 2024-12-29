using MailKit.Net.Smtp;
using MimeKit;
using Polly;
using System.Net.Mail;
using System.Net.Sockets;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BookFarm.Entities.mail
{
  public class EmailService
  {
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task<SendEmailResult> SendEmailAsync(string email, string subject, string body, bool isHtml = true, List<MimePart> attachments = null, List<string> cc = null, List<string> bcc = null)
    {
      // Create the email message using the provided parameters
      var message = CreateEmailMessage(email, subject, body, isHtml, attachments, cc, bcc);

      using (var client = new SmtpClient())
      {
        try
        {
          // Retrieve SMTP settings from the configuration
          var smtpHost = _configuration["Smtp:Host"];
          var smtpPort = int.Parse(_configuration["Smtp:Port"]);
          var smtpUser = _configuration["Smtp:Username"];
          var smtpPassword = _configuration["Smtp:Password"];
          var useSsl = bool.Parse(_configuration["Smtp:UseSsl"]);
          var ignoreCertificateErrors = bool.Parse(_configuration["Smtp:IgnoreCertificateErrors"] ?? "false");
          var secureOption = useSsl ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls;

          // Validate that SMTP host is properly configured
          if (string.IsNullOrEmpty(smtpHost))
          {
            throw new ArgumentException("SMTP Host is not configured correctly.");
          }

          // Set the client timeout (in milliseconds)
          client.Timeout = int.Parse(_configuration["Smtp:Timeout"] ?? "10000"); // Default to 10 seconds

          // Optionally ignore SSL certificate errors (useful for testing environments)
          if (ignoreCertificateErrors)
          {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
          }

          // Define retry policy with exponential backoff for resiliency
          var retryCount = int.Parse(_configuration["Email:RetryCount"] ?? "3");
          var retryPolicy = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

          // Execute the email sending process with the retry policy
          await retryPolicy.ExecuteAsync(async () =>
          {
            await client.ConnectAsync(smtpHost, smtpPort, secureOption); // Connect to SMTP server
            await client.AuthenticateAsync(smtpUser, smtpPassword); // Authenticate with SMTP server
            await client.SendAsync(message); // Send the email message
          });

          return new SendEmailResult { IsSuccess = true, Message = "Email sent successfully" };
        }
        catch (SocketException ex)
        {
          // Handle socket exceptions (e.g., network-related issues)
          Console.WriteLine($"Connection attempt failed: {ex.Message}");
          return new SendEmailResult { IsSuccess = false, Message = "Unable to connect to the SMTP host. Please check the host address and network connectivity." };
        }
        catch (TimeoutException ex)
        {
          // Handle timeout exceptions (e.g., SMTP server took too long to respond)
          Console.WriteLine($"Connection timed out: {ex.Message}");
          return new SendEmailResult { IsSuccess = false, Message = "The connection to the SMTP server timed out. Please check the network status or SMTP server availability." };
        }
        catch (Exception ex)
        {
          // Log any general exceptions that occur during email sending
          //_logger.Error(ex, "An error occurred while sending the email: {Message}", ex.Message);
          return new SendEmailResult { IsSuccess = false, Message = "An error occurred while sending the email." };
        }
        finally
        {
          // Ensure the client is disconnected properly
          if (client.IsConnected)
          {
            await client.DisconnectAsync(true);
          }
        }
      }
    }

    public async Task<bool> CheckSmtpServerHealthAsync()
    {
      using (var client = new SmtpClient())
      {
        try
        {
          // Retrieve SMTP settings from the configuration
          var smtpHost = _configuration["Smtp:Host"];
          var smtpPort = int.Parse(_configuration["Smtp:Port"]);
          var smtpUser = _configuration["Smtp:Username"];
          var smtpPassword = _configuration["Smtp:Password"];
          var useSsl = bool.Parse(_configuration["Smtp:UseSsl"]);
          var ignoreCertificateErrors = bool.Parse(_configuration["Smtp:IgnoreCertificateErrors"] ?? "false");
          var secureOption = useSsl ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls;

          // Validate that SMTP host is properly configured
          if (string.IsNullOrEmpty(smtpHost))
          {
            throw new ArgumentException("SMTP Host is not configured correctly.");
          }

          // Set the client timeout (in milliseconds)
          client.Timeout = int.Parse(_configuration["Smtp:Timeout"] ?? "10000"); // Default to 10 seconds

          // Optionally ignore SSL certificate errors (useful for testing environments)
          if (ignoreCertificateErrors)
          {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
          }

          // Define retry policy with exponential backoff for resiliency
          var retryPolicy = Policy
              .Handle<Exception>()
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

          // Execute the SMTP server health check with retry logic
          await retryPolicy.ExecuteAsync(async () =>
          {
            await client.ConnectAsync(smtpHost, smtpPort, secureOption); // Connect to SMTP server
          });
          await client.DisconnectAsync(true); // Disconnect after successful check
          return true; // SMTP server is healthy
        }
        catch (Exception ex)
        {
          // Log error during health check
          //_logger.Error(ex, "SMTP server health check failed: {Message}", ex.Message);
          return false;
        }
        finally
        {
          // Ensure the client is disconnected properly
          if (client.IsConnected)
          {
            await client.DisconnectAsync(true);
          }
        }
      }
    }

    private MimeMessage CreateEmailMessage(string email, string subject, string body, bool isHtml, List<MimePart> attachments, List<string> cc = null, List<string> bcc = null)
    {
      // Create a new email message instance
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Notification", _configuration["Smtp:From"])); // Set sender address
      message.To.Add(new MailboxAddress("", email)); // Set recipient address
      message.Subject = subject; // Set the email subject

      // Add CC and BCC addresses if provided
      if (cc != null)
      {
        foreach (var ccAddress in cc)
        {
          message.Cc.Add(new MailboxAddress("", ccAddress));
        }
      }

      if (bcc != null)
      {
        foreach (var bccAddress in bcc)
        {
          message.Bcc.Add(new MailboxAddress("", bccAddress));
        }
      }

      // Create the email body
      var bodyBuilder = new BodyBuilder
      {
        HtmlBody = isHtml ? body : null, // Set HTML body if specified
        TextBody = !isHtml ? body : null // Set plain text body if specified
      };

      // Add attachments if any are provided
      if (attachments != null)
      {
        foreach (var attachment in attachments)
        {
          bodyBuilder.Attachments.Add(attachment);
        }
      }

      message.Body = bodyBuilder.ToMessageBody(); // Set the email body

      return message; // Return the constructed email message
    }
  }

  public class SendEmailResult
  {
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
  }
}

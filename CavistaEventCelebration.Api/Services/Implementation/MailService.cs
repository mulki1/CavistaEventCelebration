using CavistaEventCelebration.Api.Models.EmailService;
using CavistaEventCelebration.Api.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CavistaEventCelebration.Api.Services.implementation
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly EmailConfiguration _emailConfig;
        public MailService(IOptions<MailSettings> options, EmailConfiguration emailConfig)
        {
            _mailSettings = options.Value;
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(MailData mailData)
        {
            try
            {
                var host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? _mailSettings.Host;
                var username = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? _mailSettings.UserName;
                var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? _mailSettings.Password;
                var fromEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? _mailSettings.EmailId;
                var emailMessage = new MimeMessage();
                var emailFrom = new MailboxAddress(_mailSettings.Name, fromEmail);
                emailMessage.From.Add(emailFrom);
                var emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = mailData.EmailSubject;
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = mailData.EmailBody;
                if (!string.IsNullOrWhiteSpace(mailData.EmailBody) &&
                    (mailData.EmailBody.Contains("<html", StringComparison.OrdinalIgnoreCase) ||
                     mailData.EmailBody.Contains("<body", StringComparison.OrdinalIgnoreCase) ||
                     mailData.EmailBody.Contains("</")))
                {
                    bodyBuilder.HtmlBody = mailData.EmailBody;
                }
                emailMessage.Body = bodyBuilder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(host, _mailSettings.Port, _mailSettings.UseSSL);
                smtp.Authenticate(username, password);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");

            }
        }

        public async Task SendEmailSmtp(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true;
                    client.CheckCertificateRevocation = false;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.SslOnConnect);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    var x = await client.SendAsync(mailMessage);
                    Console.WriteLine(x);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occured: {ex.Message}");
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

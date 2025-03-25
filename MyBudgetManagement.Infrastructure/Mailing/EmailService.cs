using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.EmailService;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration)
    {
        var emailConfig = configuration.GetSection("EmailSettings");
        _smtpServer = emailConfig["SmtpServer"];
        _smtpPort = int.Parse(emailConfig["SmtpPort"]);
        _smtpUsername = emailConfig["SmtpUsername"];
        _smtpPassword = emailConfig["SmtpPassword"];
        _enableSsl = bool.Parse(emailConfig["EnableSsl"]);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
{
    try
    {
        using var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = _enableSsl
        };

        var mailMessage = new MailMessage(_smtpUsername, to, subject, body)
        {
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(mailMessage);
        Console.WriteLine($"Email sent to {to} with subject: {subject}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending email to {to}: {ex.Message}");
        throw; // Ném lại exception để xử lý tiếp nếu cần
    }
}
}

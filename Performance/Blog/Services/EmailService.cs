using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Equipe Duda Junior",
        string fromEmail = "dudajr.net@outlook.com")
    {
        var smtpClient = new SmtpClient(Configuration.smtp.Host, Configuration.smtp.Port);

        smtpClient.Credentials = new NetworkCredential(Configuration.smtp.UserName, Configuration.smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        var mail = new MailMessage();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
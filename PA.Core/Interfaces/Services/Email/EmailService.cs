using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace PA.Core.Interfaces.Services.Email
{
	// Make sure the correspongind values are added in the secerects file
	public class EmailService : IEmailService
	{
		private readonly IConfiguration Configuration;

		public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Send(string to, string subject, string html, string? from = null)
		{
			// create message
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(from ?? Configuration["AppSettings:EmailFrom"]));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;
			email.Body = new TextPart(TextFormat.Html) { Text = html };

			// send email
			using var smtp = new SmtpClient();
			smtp.Connect(Configuration["AppSettings:SmtpHost"], int.Parse(Configuration["AppSettings:SmtpPort"]), SecureSocketOptions.StartTls);
			smtp.Authenticate(Configuration["AppSettings:SmtpUser"], Configuration["AppSettings:SmtpPass"]);
			smtp.Send(email);
			smtp.Disconnect(true);
		}
	}
}

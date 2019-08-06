using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RceServer.Core.Services.Implementation
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var apiKey = _configuration["SendGridApiKey"];
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage
			{
				From = new EmailAddress($"no-reply@{_configuration["Domain"]}", "Rce Server"),
				Subject = subject,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			await client.SendEmailAsync(msg);
		}

		public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
		{
			var message = $"<h4>Email confirmation link: </h4><p><a href=\"{callbackUrl}\">Link</a></p>";
			await SendEmailAsync(email, "Email confirmation", message);
		}

		public async Task SendResetPasswordAsync(string email, string callbackUrl)
		{
			var message = $"<h4>Password reset link: </h4><p><a href=\"{callbackUrl}\">Link</a></p>";
			await SendEmailAsync(email, "Password reset", message);
		}
	}
}

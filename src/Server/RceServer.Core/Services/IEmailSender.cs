using System.Threading.Tasks;

namespace RceServer.Core.Services
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
		Task SendEmailConfirmationAsync(string email, string callbackUrl);
		Task SendResetPasswordAsync(string email, string callbackUrl);
	}
}

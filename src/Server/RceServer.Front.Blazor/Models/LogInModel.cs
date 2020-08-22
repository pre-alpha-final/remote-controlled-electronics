using System.ComponentModel.DataAnnotations;

namespace RceServer.Front.Blazor.Models
{
	public class LogInModel
	{
		[Required, EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}

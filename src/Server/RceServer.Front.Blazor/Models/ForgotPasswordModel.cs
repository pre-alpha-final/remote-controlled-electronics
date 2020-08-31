using System.ComponentModel.DataAnnotations;

namespace RceServer.Front.Blazor.Models
{
	public class ForgotPasswordModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}

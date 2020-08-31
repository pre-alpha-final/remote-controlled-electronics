using System.ComponentModel.DataAnnotations;

namespace RceServer.Front.Blazor.Models
{
	public class ResetPasswordModel
	{
		public string UserId { get; set; }
		public string Code { get; set; }
		[Required]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&,.\\/\\-+=()])[ A-Za-z\\d@$!%*?&,.\\/\\-+=()]{1,}$", ErrorMessage = "Password must contain at least one of each: uppercase letter, lowercase letter, number, special character")]
		public string Password { get; set; }
		[Required]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&,.\\/\\-+=()])[ A-Za-z\\d@$!%*?&,.\\/\\-+=()]{1,}$", ErrorMessage = "Password must contain at least one of each: uppercase letter, lowercase letter, number, special character")]
		public string Password2 { get; set; }
	}
}

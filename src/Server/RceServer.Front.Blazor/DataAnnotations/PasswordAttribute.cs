using System.ComponentModel.DataAnnotations;

namespace RceServer.Front.Blazor.DataAnnotations
{
	public class PasswordAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			ErrorMessage = "foo";
			return false;
		}
	}
}

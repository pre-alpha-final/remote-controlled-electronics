using System.Threading.Tasks;

namespace RceRemoteSharpLib
{
	public class LogInService
	{
		public string User { get; set; }
		public string Password { get; set; }

		public async Task LogIn(string user, string password)
		{
			User = user;
			Password = password;
		}
	}
}

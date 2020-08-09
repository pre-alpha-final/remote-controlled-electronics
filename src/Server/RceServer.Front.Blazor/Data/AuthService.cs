using System;
using System.Threading.Tasks;

namespace RceServer.Front.Blazor.Data
{
	public class AuthService
	{
		public event Func<Task> OnChanged;
		public bool UserAuthenticated { get; set; }

		public AuthService()
		{
			Task.Run(Test);
		}

		public async Task Test()
		{
			while (true)
			{
				await Task.Delay(1000);
				UserAuthenticated = !UserAuthenticated;
				OnChanged?.Invoke();
			}
		}
	}
}

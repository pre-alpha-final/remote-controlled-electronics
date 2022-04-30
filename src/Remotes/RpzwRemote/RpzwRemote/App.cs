using RceRemoteSharpLib;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RpzwRemote
{
	internal class App
	{
		private readonly LogInService _logInService;
		private readonly ControlService _controlService;

		public App(LogInService logInService, ControlService controlService)
		{
			_logInService = logInService;
			_controlService = controlService;
		}

		public async Task Run()
		{
			while (true)
			{
				Console.WriteLine("Menu: [L]ogIn L[i]st [R]un [Q]uit");
				var key = Console.ReadKey(true);
				switch (key.KeyChar)
				{
					case 'l':
					case 'L':
						Console.Write("User: ");
						var user = Console.ReadLine();
						Console.Write("Password: ");
						var password = ReadLineHidden();
						await _logInService.LogIn(Settings.LoginUrl, user, password);
						break;
					case 'i':
					case 'I':
						var workers = await _controlService.GetList(Settings.GetMessagesUrl);
						break;
					case 'r':
					case 'R':
						break;
					case 'q':
					case 'Q':
						return;
					default:
						break;
				}
			}
		}

		private static string ReadLineHidden()
		{
			var input = new StringBuilder();
			while (true)
			{
				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.Enter)
				{
					break;
				}
				input.Append(key.KeyChar);
			}

			return input.ToString();
		}
	}
}

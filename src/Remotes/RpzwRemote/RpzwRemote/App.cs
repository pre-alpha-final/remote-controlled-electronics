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
		private readonly GpioService _gpioService;

		public App(LogInService logInService, ControlService controlService, GpioService gpioService)
		{
			_logInService = logInService;
			_controlService = controlService;
			_gpioService = gpioService;
		}

		public async Task Run()
		{
			while (true)
			{
				Console.WriteLine("Menu: [L]ogIn L[i]st [R]un [P]in test [Q]uit");
				var key = Console.ReadKey(true);
				switch (key.KeyChar)
				{
					case 'l':
					case 'L':
						Console.Write("User: ");
						var user = Console.ReadLine();
						Console.Write("Password: ");
						var password = ReadLineHidden();
						Console.WriteLine();
						await _logInService.LogIn(Settings.LoginUrl, user, password);
						break;
					case 'i':
					case 'I':
						try
						{
							var workers = await _controlService.GetWorkerList(Settings.GetMessagesUrl);
							foreach (var worker in workers)
							{
								Console.WriteLine($"{worker.Name}, ({worker.WorkerId})");
								foreach (var jobDescription in worker.JobDescriptions)
								{
									Console.WriteLine($"   {jobDescription.Name}");
								}
								Console.WriteLine();
							}
						}
						catch (Exception e) when (e.Message == "Unauthorized")
						{
							Console.WriteLine("Unauthorized");
						}
						break;
					case 'r':
					case 'R':
						Console.Write("Worker id: ");
						var workerId = Console.ReadLine();
						Console.Write("Job name: ");
						var jobName = Console.ReadLine();
						Console.Write("Job payload: ");
						var jobPayload = Console.ReadLine();
						try
						{
							jobPayload = jobPayload == "" ? "{}" : jobPayload;
							await _controlService.RunJob(Settings.RunJobUrl(workerId), jobName, jobPayload);
						}
						catch (Exception e) when (e.Message == "Unauthorized")
						{
							Console.WriteLine("Unauthorized");
						}
						Console.WriteLine();
						break;
					case 'p':
					case 'P':
						Console.WriteLine("Testing pins: 23, 24, 5, 6, 13");
						Console.WriteLine(" OOOOOOOXXOOOOOOOOOOO");
						Console.WriteLine("|OOOOOOOOOOOOOOXXXOOO");
						_gpioService.Run();
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

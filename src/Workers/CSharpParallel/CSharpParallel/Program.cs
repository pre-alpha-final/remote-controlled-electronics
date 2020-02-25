using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpParallel
{
	class Program
	{
		public static string UrlBase = "https://rceserver.azurewebsites.net";
		public static List<string> Owners = new List<string>
		{
			"demo@example.com"
		};

		static async Task Main(string[] args)
		{
			var jobRunnerStateMachine = new JobRunnerStateMachine();
			_ = Task.Run(jobRunnerStateMachine.Start);

			Console.WriteLine("Press any key to exit");
			Console.WriteLine();
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine("Exiting...");
			await jobRunnerStateMachine.Stop();
		}
	}
}

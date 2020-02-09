using System;
using System.Threading.Tasks;

namespace CSharpSequential
{
	class Program
	{
		public static string UrlBase = "http://localhost:3140";

		static async Task Main(string[] args)
		{
			var jobRunnerStateMachine = new JobRunnerStateMachine();
			jobRunnerStateMachine.Start();

			Console.WriteLine("Press any key to exit");
			Console.WriteLine();
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine("Exiting...");
			await jobRunnerStateMachine.Stop();
		}
	}
}

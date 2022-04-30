using Microsoft.Extensions.DependencyInjection;
using RceRemoteSharpLib;
using System.Threading.Tasks;

namespace RpzwRemote
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<App>();
			serviceCollection.AddSingleton<LogInService>();
			var serviceProvider = serviceCollection.BuildServiceProvider();

			await serviceProvider.GetService<App>().Run();
		}
	}
}

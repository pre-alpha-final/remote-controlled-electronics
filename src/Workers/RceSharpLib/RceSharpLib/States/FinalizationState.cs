using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RceSharpLib.States
{
	internal class FinalizationState : StateBase, IState
	{
		public FinalizationState(StateBase previousState)
			: base(previousState)
		{
		}

		public async Task Handle()
		{
			try
			{
				Console.WriteLine("Finalizing worker");

				var closeWorkerAddressSuffix = Consts.CloseWorkerAddressSuffix
					.Replace("WORKER_ID", WorkerId.ToString());
				var requestUri = $"{RceJobRunner.JobRunnerContext.BaseUrl}{closeWorkerAddressSuffix}";

				using (var client = new HttpClient())
				using (await client.PostAsync(requestUri, new StringContent(string.Empty)))
				{
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Finalizing worker failed: '{WorkerId}' '{e.Message}'");
			}
		}
	}
}

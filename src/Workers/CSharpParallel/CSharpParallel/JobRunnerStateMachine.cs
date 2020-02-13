using System;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpParallel.States;

namespace CSharpParallel
{
	public class JobRunnerStateMachine
	{
		public bool Stopped { get; set; }
		public IState State { get; set; }
		public Guid WorkerId { get; set; }

		public JobRunnerStateMachine()
		{
			State = new RegistrationState();
		}

		public async Task Start()
		{
			while (Stopped == false)
			{
				await State.Handle(this);
			}
		}

		public async Task Stop()
		{
			Stopped = true;
			await CloseWorker();
		}

		private async Task CloseWorker()
		{
			try
			{
				var closeWorkerAddressSuffix = Consts.CloseWorkerAddressSuffix
					.Replace("WORKER_ID", WorkerId.ToString());
				var requestUri = $"{Program.UrlBase}{closeWorkerAddressSuffix}";

				using (var client = new HttpClient())
				using (await client.PostAsync(requestUri, new StringContent(string.Empty)))
				{
				}
			}
			catch (Exception e)
			{
				// ignore
			}
		}
	}
}

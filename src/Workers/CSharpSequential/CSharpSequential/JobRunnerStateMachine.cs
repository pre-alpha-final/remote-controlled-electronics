using System;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpSequential.States;

namespace CSharpSequential
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

		public void Start()
		{
			Task.Run(async () =>
			{
				while (Stopped == false)
				{
					await State.Handle(this);
				}
			});
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

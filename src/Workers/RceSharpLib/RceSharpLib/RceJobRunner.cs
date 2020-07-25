using RceSharpLib.States;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RceSharpLib
{
	public class RceJobRunner
	{
		public IState State { get; set; }
		public bool Running { get; set; }
		public CancellationTokenSource CancellationTokenSource { get; set; }
		public JobRunnerContext JobRunnerContext { get; set; }

		public async Task Start()
		{
			if (State != null)
			{
				throw new InvalidOperationException("Still running");
			}

			CancellationTokenSource = new CancellationTokenSource();
			State = new RegistrationState(this);

			Running = true;
			while (Running)
			{
				await State.Handle();
			}

			State = new FinalizationState(State as StateBase);
		}

		public async Task Stop()
		{
			Running = false;
			CancellationTokenSource.Cancel();
			while (State?.GetType() != typeof(FinalizationState))
			{
				await Task.Delay(100);
			}
			await State.Handle();
			State = null;
		}
	}
}

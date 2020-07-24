using RceSharpLib.States;
using System;
using System.Threading.Tasks;

namespace RceSharpLib
{
	public class RceJobRunner
	{
		public IState State { get; set; }
		public bool Running { get; set; }
		public JobRunnerContext JobRunnerContext { get; set; }

		public async Task Start()
		{
			if (State != null)
			{
				throw new InvalidOperationException("Still running");
			}

			State = new RegistrationState(this);

			Running = true;
			while (Running)
			{
				await State.Handle();
			}

			State = new FinalizationState(State as StateBase);
			await State.Handle();
			State = null;
		}

		public void Stop()
		{
			Running = false;
		}
	}
}

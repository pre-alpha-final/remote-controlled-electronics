using System;
using System.Threading.Tasks;

namespace RceSharpLib.States
{
	internal class FailedState : StateBase, IState
	{
		private const int RetryDelayMs = 5000;
		private readonly string _failureReason;

		public FailedState(StateBase previousState, string failureReason)
			: base(previousState)
		{
			_failureReason = failureReason;
		}

		public async Task Handle()
		{
			Console.WriteLine($"Problem encountered: '{_failureReason}'");

			await Task.Delay(RetryDelayMs);
			RceJobRunner.State = new RegistrationState(RceJobRunner);
		}
	}
}

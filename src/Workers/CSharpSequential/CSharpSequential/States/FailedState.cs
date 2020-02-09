using System;
using System.Threading.Tasks;

namespace CSharpSequential.States
{
	public class FailedState : IState
	{
		private const int RetryDelayMs = 5000;
		private readonly string _failureReason;

		public FailedState(string failureReason)
		{
			_failureReason = failureReason;
		}

		public async Task Handle(JobRunnerStateMachine jobRunnerStateMachine)
		{
			Console.WriteLine($"Problem encountered: '{_failureReason}'");

			await Task.Delay(RetryDelayMs);
			jobRunnerStateMachine.State = new RegistrationState();
		}
	}
}

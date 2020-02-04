using System;
using System.Threading.Tasks;
using CSharpSingleThreaded.JobRunners;

namespace CSharpSingleThreaded.States
{
	public class RunJobState : IState
	{
		private readonly Job _job;

		public RunJobState(Job job)
		{
			_job = job;
		}

		public async Task Handle(JobRunnerStateMachine jobRunnerStateMachine)
		{
			try
			{
				switch (_job.JobName)
				{
					case "Count":
						await new CountJobRunner(_job).Run();
						break;

					default:
						await new JobRunnerBase(_job).FailJob($"No job named {_job.JobName}");
						return;
				}
			}
			catch (Exception e)
			{
				await new JobRunnerBase(_job).FailJob(e.Message);
			}

			jobRunnerStateMachine.State = new GetJobsState();
		}
	}
}

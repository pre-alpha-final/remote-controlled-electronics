using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpParallel.JobRunners;

namespace CSharpParallel.States
{
	public class RunJobsState : IState
	{
		private readonly List<Job> _jobs;

		public RunJobsState(List<Job> jobs)
		{
			_jobs = jobs;
		}

		public async Task Handle(JobRunnerStateMachine jobRunnerStateMachine)
		{
			foreach (var job in _jobs)
			{
				switch (job.JobName)
				{
					case "Count":
						_ = Task.Run(() => new CountJobRunner(job).Run());
						break;

					default:
						_ = Task.Run(() => new JobRunnerBase(job).FailJob($"No job named {job.JobName}"));
						return;
				}
			}

			jobRunnerStateMachine.State = new GetJobsState();
		}
	}
}

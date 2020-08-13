namespace RceServer.Front.Blazor.Infrastructure
{
	public static class TelemetryEvents
	{
		public const string UserLoggingIn = nameof(UserLoggingIn);
		public const string RegisteringWorker = nameof(RegisteringWorker);
		public const string SchedulingJob = nameof(SchedulingJob);
		public const string WorkerTakingJob = nameof(WorkerTakingJob);
	}
}

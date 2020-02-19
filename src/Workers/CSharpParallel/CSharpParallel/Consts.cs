namespace CSharpParallel
{
	public class Consts
	{
		public const string RegisterAddressSuffix = "/api/workers/register/";
		public const string GetJobsAddressSuffix = "/api/workers/WORKER_ID/jobs/";
		public const string UpdateJobAddressSuffix = "/api/workers/WORKER_ID/jobs/JOB_ID/update";
		public const string CompleteJobAddressSuffix = "/api/workers/WORKER_ID/jobs/JOB_ID/complete";
		public const string CloseWorkerAddressSuffix = "/api/workers/WORKER_ID/close";
	}
}

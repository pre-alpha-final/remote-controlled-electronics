namespace CSharpSingleThreaded
{
	public class Consts
	{
		public const string RegisterAddressSuffix = "/api/workers/register/";
		public const string GetJobAddressSuffix = "/api/workers/WORKER_ID/jobs/1/";
		public const string UpdateJobAddressSuffix = "/api/workers/WORKER_ID/jobs/JOB_ID/update";
		public const string CompleteJobAddressSuffix = "/api/workers/WORKER_ID/jobs/JOB_ID/complete";
		public const string CloseWorkerAddressSuffix = "/api/workers/WORKER_ID/close";
	}
}

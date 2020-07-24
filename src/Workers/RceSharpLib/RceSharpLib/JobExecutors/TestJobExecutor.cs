using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RceSharpLib.JobExecutors
{
	public class TestJobExecutor : JobExecutorBase
	{
		public TestJobExecutor(string baseUrl, RceJob rceJob)
			: base(baseUrl, rceJob)
		{
		}

		protected override RceJobDescription RceJobDescription => new RceJobDescription
		{
			Name = "_TEST_JOB",
			Description = new List<string>
			{
				"Test job"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override Task Execute()
		{
			throw new System.NotImplementedException();
		}
	}
}

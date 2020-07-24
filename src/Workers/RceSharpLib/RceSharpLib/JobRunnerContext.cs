using System;
using System.Collections.Generic;

namespace RceSharpLib
{
	public class JobRunnerContext
	{
		public string BaseUrl { get; set; }
		public List<string> Owners { get; set; }
		public Dictionary<string, Type> JobExecutorTypes { get; set; }
		public RegistrationModel RegistrationModel { get; set; }
	}
}

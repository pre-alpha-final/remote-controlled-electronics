using System;
using RceServer.Domain.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RceServer.Front.Blazor.Services
{
	public class RceDataService
	{
		public List<Worker> Workers { get; set; } = new List<Worker>
		{
			new Worker
			{
				WorkerId = Guid.NewGuid().ToString(),
				Name = "Test worker 1",
				Description = "Test worker 1 description",
				Base64Logo = "",
				JobDescriptions = new List<JobDescription>
				{
					new JobDescription
					{
						Name = "Worker 1 Job 1",
						Description = new List<string>
						{
							"Worker 1",
							"Job 1"
						},
						DefaultPayload = JObject.Parse("{}")
					},
					new JobDescription
					{
						Name = "Worker 1 Job 2",
						Description = new List<string>
						{
							"Worker 1",
							"Job 2"
						},
						DefaultPayload = JObject.Parse("{}")
					}
				}
			},
			new Worker
			{
				WorkerId = Guid.NewGuid().ToString(),
				Name = "",
				Description = "",
				Base64Logo = "",
				JobDescriptions = new List<JobDescription>
				{
					new JobDescription
					{
						Name = "",
						Description = new List<string>
						{
							"",
							""
						},
						DefaultPayload = JObject.Parse("{}")
					},
					new JobDescription
					{
						Name = "",
						Description = new List<string>(),
						DefaultPayload = JObject.Parse("{}")
					}
				}
			},
			new Worker
			{
				WorkerId = Guid.NewGuid().ToString(),
				Name = "Test worker 3",
				Description = "Test worker 3 description",
				Base64Logo = "",
				JobDescriptions = new List<JobDescription>
				{
					new JobDescription
					{
						Name = "Worker 3 Job 1",
						Description = new List<string>
						{
							"Worker 3",
							"Job 1"
						},
						DefaultPayload = JObject.Parse("{}")
					},
					new JobDescription
					{
						Name = "Worker 3 Job 2",
						Description = new List<string>
						{
							"Worker 3",
							"Job 2"
						},
						DefaultPayload = JObject.Parse("{}")
					}
				}
			}
		};
	}
}

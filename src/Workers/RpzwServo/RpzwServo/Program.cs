﻿using RceSharpLib;
using RpzwServo.Handlers;
using RpzwServo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RpzwServo
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var rceJobRunner = new RceJobRunnerBuilder()
				.SetBaseUrl("https://rceserver.azurewebsites.net")
				.SetWorkerName("Servo Controller")
				.SetWorkerDescription("Controller for FS90 servo")
				.SetWorkerBase64Logo("data:image/jpeg;base64,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")
				.SetOwners(new List<string> { "demo@example.com" })
				.SetRunInParallel(false)
				.AddJobHandlerType(typeof(StartHandler))
				.AddJobHandlerType(typeof(StopHandler))
				.Build();
			_ = Task.Run(rceJobRunner.Start);
			ServoService.Instance.Initialize();

			Console.WriteLine("Press any key to exit");
			Console.WriteLine();
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine("Exiting...");
			await rceJobRunner.Stop();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RpzwNeopixels.Neopixel
{
	public static class PythonRunner
	{
		public static void Run(IEnumerable<string> commands)
		{
			const string PythonExecutable = "python3";

			var ScriptFilename = $"{Guid.NewGuid()}.py";
			var scriptContents = new List<string>()
			{
				"import board",
				"import neopixel",
				"from time import sleep",
			};
			scriptContents.AddRange(commands);
			File.WriteAllLines(ScriptFilename, scriptContents);

			var processStartInfo = new ProcessStartInfo
			{
				FileName = PythonExecutable,
				Arguments = ScriptFilename,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
			using (var process = Process.Start(processStartInfo))
			using (var reader = process.StandardOutput)
			{
				string result = reader.ReadToEnd();
				Console.Write(result);
			}

			File.Delete(ScriptFilename);
		}
	}
}

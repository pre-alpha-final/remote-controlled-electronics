using System;

namespace RceSharpLib.States
{
	internal class StateBase
	{
		public RceJobRunner RceJobRunner { get; set; }
		public Guid WorkerId { get; set; }

		public StateBase(StateBase previousState)
		{
			RceJobRunner = previousState?.RceJobRunner;
			WorkerId = previousState?.WorkerId ?? Guid.Empty;
		}
	}
}

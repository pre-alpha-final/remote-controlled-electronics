using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RceServer.Core.Helpers;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class ClientService : IClientService
	{
		private const int PollTimeSeconds = 60;
		private const int PollDelaySeconds = 1;
		private readonly IWorkerRepository _workerRepository;

		public ClientService(IWorkerRepository workerRepository)
		{
			_workerRepository = workerRepository;
		}

		public async Task<IList<IRceMessage>> GetFeed(long timestamp)
		{
			for (var i = 0; i < PollTimeSeconds / PollDelaySeconds; i++)
			{
				var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				var messages = await _workerRepository.GetMessages(timestamp);
				messages = messages // prevent possible racing with db reads/writes
					.Where(e => e.MessageTimestamp < now - 100)
					.ToList();
				if (messages.Any())
				{
					RceMessageHelpers.Minimize(messages);
					return messages;
				}
				await Task.Delay(TimeSpan.FromSeconds(PollDelaySeconds));
			}

			return new List<IRceMessage>();
		}
	}
}

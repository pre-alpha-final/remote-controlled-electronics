using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RceServer.Domain.Models;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class ClientService : IClientService
	{
		private const int PollTime = 30;
		private readonly IWorkerRepository _workerRepository;

		public ClientService(IWorkerRepository workerRepository)
		{
			_workerRepository = workerRepository;
		}

		// TODO Return rcemesssages AddWorkerMessage, RemoveWorkerMessage, UpsertJobMessage, RemoveJobMessage
        // TODO minimize the feed
        // TODO if timestamp 0 or low, should correctly return "full status"
		public async Task<IList<IRceMessage>> GetFeed(long timestamp)
		{
			for (var i = 0; i < PollTime; i++)
			{
				var messages = await _workerRepository.GetMessages(timestamp);
				if (messages.Any())
				{
					return messages;
				}
				await Task.Delay(1000);
			}

			return new List<IRceMessage>();
		}
	}
}

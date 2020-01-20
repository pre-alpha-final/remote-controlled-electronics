using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Newtonsoft.Json;
using RceServer.Core.Hubs;
using RceServer.Data.Extensions;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Data
{
	public class MessageRepository : IMessageRepository
	{
		private const string DatabaseName = "mo10097_rce";
		private const string CollectionName = "messages";

		private readonly IMongoClient _mongoClient;
		private readonly IHubContext<RceHub, IRceHub> _rceHubContext;

		public MessageRepository(IMongoClient mongoClient, IHubContext<RceHub, IRceHub> rceHubContext)
		{
			_mongoClient = mongoClient;
			_rceHubContext = rceHubContext;
		}

		public async Task AddMessage(IRceMessage message)
		{
			if (message is IHasWorkerId hasWorkerId)
			{
				if (await IsDisconnected(hasWorkerId.WorkerId))
				{
					throw new Exception("Worker disconnected");
				}

				if (await IsRegistered(hasWorkerId.WorkerId) == false &&
					message is WorkerAddedMessage == false)
				{
					throw new Exception("Specified worker does not exist");
				}
			}

			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(CollectionName);
			await messagesCollection.InsertOneAsync(message.ToExpandoObject());
			await _rceHubContext.Clients.All.MessageReceived(message);
		}

		public async Task<IList<IRceMessage>> GetMessagesBefore(long timestamp)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(CollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Lt("MessageTimestamp", timestamp))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task<IList<IRceMessage>> GetMessagesAfter(long timestamp)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(CollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Gte("MessageTimestamp", timestamp))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task<IList<IRceMessage>> GetWorkerMessages(Guid workerId)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(CollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Eq("WorkerId", workerId.ToString()))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task RemoveMessages(IEnumerable<Guid> messages)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(CollectionName);
			await messagesCollection.DeleteManyAsync(
				Builders<dynamic>.Filter.In("MessageId", messages.Select(e => e.ToString())));
		}

		public async Task<bool> IsDisconnected(Guid workerId)
		{
			var workerMessages = await GetWorkerMessages(workerId);
			return workerMessages.Any(e => e is WorkerRemovedMessage);
		}

		public async Task<bool> IsRegistered(Guid workerId)
		{
			var workerMessages = await GetWorkerMessages(workerId);
			return workerMessages.Count != 0;
		}

		private IRceMessage Deserialize(dynamic message)
		{
			switch (message.MessageType)
			{
				case nameof(JobAddedMessage):
					return JsonConvert.DeserializeObject<JobAddedMessage>(JsonConvert.SerializeObject(message));

				case nameof(JobCompletedMessage):
					return JsonConvert.DeserializeObject<JobCompletedMessage>(JsonConvert.SerializeObject(message));

				case nameof(JobPickedUpMessage):
					return JsonConvert.DeserializeObject<JobPickedUpMessage>(JsonConvert.SerializeObject(message));

				case nameof(JobRemovedMessage):
					return JsonConvert.DeserializeObject<JobRemovedMessage>(JsonConvert.SerializeObject(message));

				case nameof(JobUpdatedMessage):
					return JsonConvert.DeserializeObject<JobUpdatedMessage>(JsonConvert.SerializeObject(message));

				case nameof(KeepAliveSentMessage):
					return JsonConvert.DeserializeObject<KeepAliveSentMessage>(JsonConvert.SerializeObject(message));

				case nameof(WorkerAddedMessage):
					return JsonConvert.DeserializeObject<WorkerAddedMessage>(JsonConvert.SerializeObject(message));

				case nameof(WorkerRemovedMessage):
					return JsonConvert.DeserializeObject<WorkerRemovedMessage>(JsonConvert.SerializeObject(message));

				default:
					return null;
			}
		}
	}
}

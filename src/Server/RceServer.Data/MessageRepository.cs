using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
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
		private const string OwnersCollectionName = "owners";
		private const string MessagesCollectionName = "messages";
		private const string OwnerFieldName = "Owner";
		private const string MessageIdFieldName = "MessageId";
		private const string WorkerIdFieldName = "WorkerId";
		private const string MessageTimestampFieldName = "MessageTimestamp";

		private readonly IMongoClient _mongoClient;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IHubContext<RceHub, IRceHub> _rceHubContext;

		public MessageRepository(IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor,
			IHubContext<RceHub, IRceHub> rceHubContext)
		{
			_mongoClient = mongoClient;
			_httpContextAccessor = httpContextAccessor;
			_rceHubContext = rceHubContext;
		}

		public async Task AddMessage(IRceMessage message)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);

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

				if (message is WorkerAddedMessage workerAddedMessage)
				{
					var ownersCollection = database.GetCollection<BsonDocument>(OwnersCollectionName);
					await ownersCollection.InsertManyAsync(workerAddedMessage.Owners.Select(e =>
						new BsonDocument
						{
							{ OwnerFieldName, e },
							{ WorkerIdFieldName, workerAddedMessage.WorkerId.ToString() }
						}));
				}

				var owners = await GetOwners(hasWorkerId.WorkerId) ?? new List<string>();
				await _rceHubContext.Clients.Users(owners).MessageReceived(message);
			}

			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			await messagesCollection.InsertOneAsync(message.ToExpandoObject());
		}

		public async Task<IList<IRceMessage>> GetMyMessages()
		{
			var username = _httpContextAccessor.HttpContext.User.Claims
				.FirstOrDefault(e => e.Type == "username")?.Value;
			if (string.IsNullOrWhiteSpace(username))
			{
				throw new Exception("Username not specified");
			}

			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			var workerIds = await GetOwnedWorkerIds(username);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.In(WorkerIdFieldName, workerIds))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task<IList<IRceMessage>> GetMessagesBefore(long timestamp)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Lt(MessageTimestampFieldName, timestamp))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task<IList<IRceMessage>> GetMessagesAfter(long timestamp)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Gte(MessageTimestampFieldName, timestamp))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task<IList<IRceMessage>> GetWorkerMessages(Guid workerId)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			var messages = await messagesCollection
				.Find(Builders<dynamic>.Filter.Eq(WorkerIdFieldName, workerId.ToString()))
				.ToListAsync();

			return messages.Select(Deserialize).ToList();
		}

		public async Task RemoveMessages(IEnumerable<Guid> messageIds)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var messagesCollection = database.GetCollection<dynamic>(MessagesCollectionName);
			await messagesCollection.DeleteManyAsync(
				Builders<dynamic>.Filter.In(MessageIdFieldName, messageIds.Select(e => e.ToString())));
		}

		public async Task RemoveOwnership(IEnumerable<Guid> workerIds)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var ownersCollection = database.GetCollection<dynamic>(OwnersCollectionName);
			await ownersCollection.DeleteManyAsync(
				Builders<dynamic>.Filter.In(WorkerIdFieldName, workerIds.Select(e => e.ToString())));
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

		private async Task<IReadOnlyList<string>> GetOwners(Guid workerId)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var ownersCollection = database.GetCollection<dynamic>(OwnersCollectionName);

			var workerOwners = await ownersCollection
				.Find(Builders<dynamic>.Filter.Eq(WorkerIdFieldName, workerId.ToString()))
				.ToListAsync();

			return workerOwners.Select(e => (string)e.Owner).ToList();
		}

		private async Task<List<string>> GetOwnedWorkerIds(string username)
		{
			var database = _mongoClient.GetDatabase(DatabaseName);
			var ownersCollection = database.GetCollection<dynamic>(OwnersCollectionName);

			var workerOwners = await ownersCollection
				.Find(Builders<dynamic>.Filter.Eq(OwnerFieldName, username))
				.ToListAsync();

			return workerOwners.Select(e => (string)e.WorkerId).ToList();
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

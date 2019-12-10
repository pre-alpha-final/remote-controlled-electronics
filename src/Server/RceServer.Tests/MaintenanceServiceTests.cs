using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using RceServer.Core.Services.Implementation;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;
using Xunit;

namespace RceServer.Tests
{
	public class MaintenanceServiceTests
	{
		private List<IRceMessage> MessageList { get; }
		private readonly IMessageRepository _messageRepositoryMock;
		private readonly MaintenanceService _maintenanceService;

		public MaintenanceServiceTests()
		{
			MessageList = GetMessageListStub();
			_messageRepositoryMock = Substitute.For<IMessageRepository>();
			_messageRepositoryMock.AddMessage(Arg.Any<IRceMessage>())
				.Returns(Task.CompletedTask)
				.AndDoes(e => MessageList.AddRange(e.Args().Cast<IRceMessage>()));
			_messageRepositoryMock.RemoveMessages(Arg.Any<IEnumerable<Guid>>())
				.Returns(Task.CompletedTask)
				.AndDoes(e => MessageList.RemoveAll(f => ((IEnumerable<Guid>)e.Args().First()).Contains(f.MessageId)));
			_maintenanceService = new MaintenanceService(_messageRepositoryMock);
		}

		[Fact]
		public async Task RemoveOldActivity_WhenHasNoOldActivity_ShouldNotRemoveMessages()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(new List<IRceMessage>());
			await _maintenanceService.RemoveOldActivity();

			Assert.Equal(16, MessageList.Count);
		}

		[Fact]
		public async Task RemoveOldActivity_WhenHasOldActivities_ShouldRemoveOldActivities()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(MessageList);
			await _maintenanceService.RemoveOldActivity();

			Assert.Equal(1, MessageList.Count);
			Assert.Equal(nameof(WorkerAddedMessage), MessageList.First().MessageType);
			Assert.Equal(new Guid("00000000-FFFF-0000-0000-000000000003"), ((WorkerAddedMessage)MessageList.First()).WorkerId);
		}

		[Fact]
		public async Task MarkDisconnectedWorkers_WhenHasNoDisconnectedWorkers_ShouldNotMarkDisconnectedWorkers()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(new List<IRceMessage>());
			_messageRepositoryMock.GetMessagesAfter(Arg.Any<long>()).Returns(MessageList);
			await _maintenanceService.MarkDisconnectedWorkers();

			Assert.Equal(16, MessageList.Count);
		}

		[Fact]
		public async Task MarkDisconnectedWorkers_WhenHasDisconnectedWorkers_ShouldMarkDisconnectedWorkers()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(MessageList);
			_messageRepositoryMock.GetMessagesAfter(Arg.Any<long>()).Returns(new List<IRceMessage>());
			await _maintenanceService.MarkDisconnectedWorkers();

			Assert.Equal(17, MessageList.Count);
			Assert.Equal(nameof(WorkerRemovedMessage), MessageList[16].MessageType);
			Assert.Equal(WorkerRemovedMessage.Statuses.ConnectionLost, ((WorkerRemovedMessage) MessageList[16]).ConnectionStatus);
			Assert.Equal(new Guid("00000000-FFFF-0000-0000-000000000003"), ((WorkerRemovedMessage) MessageList[16]).WorkerId);
		}

		private List<IRceMessage> GetMessageListStub()
		{
			return new List<IRceMessage>
			{
				new WorkerAddedMessage // Create worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000001"),
					MessageTimestamp = 1,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new WorkerAddedMessage // Create worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000002"),
					MessageTimestamp = 2,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerAddedMessage // Create worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000003"),
					MessageTimestamp = 3,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobAddedMessage // Create job 1 for worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000004"),
					MessageTimestamp = 4,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000001"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new JobAddedMessage // Create job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000005"),
					MessageTimestamp = 5,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new KeepAliveAssumedMessage // Worker 2 Keep Alive
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000006"),
					MessageTimestamp = 6,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new KeepAliveAssumedMessage // Worker 2 Keep Alive
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000007"),
					MessageTimestamp = 7,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new JobAddedMessage // Create job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000008"),
					MessageTimestamp = 8,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobAddedMessage // Create job 4 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000009"),
					MessageTimestamp = 9,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000004"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerRemovedMessage // Worker 1 fails
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000010"),
					MessageTimestamp = 10,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001"),
					ConnectionStatus = WorkerRemovedMessage.Statuses.ConnectionLost
				},
				new JobUpdatedMessage // Update job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000011"),
					MessageTimestamp = 11,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new JobUpdatedMessage // Update job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000012"),
					MessageTimestamp = 12,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobCompletedMessage // Complete job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000013"),
					MessageTimestamp = 13,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobUpdatedMessage // Update job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000014"),
					MessageTimestamp = 14,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerRemovedMessage // Worker 2 Ends
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000015"),
					MessageTimestamp = 15,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002"),
					ConnectionStatus = WorkerRemovedMessage.Statuses.ClosedByWorker
				},
				new JobRemovedMessage // Remove job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000016"),
					MessageTimestamp = 16,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
			};
		}
	}
}

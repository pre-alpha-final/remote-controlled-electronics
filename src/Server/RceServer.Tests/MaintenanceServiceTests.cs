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
			MessageList = MessageListStub.Get();
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

			Assert.Equal(18, MessageList.Count);
		}

		[Fact]
		public async Task RemoveOldActivity_WhenHasOldActivities_ShouldRemoveOldActivities()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(MessageList);
			await _maintenanceService.RemoveOldActivity();

			Assert.Equal(1, MessageList.Count);
			Assert.Equal(nameof(WorkerAddedMessage), MessageList.First().MessageType);
			Assert.Equal(new Guid("00000003-0000-0000-0000-000000000000"), ((WorkerAddedMessage)MessageList.First()).WorkerId);
		}

		[Fact]
		public async Task MarkDisconnectedWorkers_WhenHasNoDisconnectedWorkers_ShouldNotMarkDisconnectedWorkers()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(new List<IRceMessage>());
			_messageRepositoryMock.GetMessagesAfter(Arg.Any<long>()).Returns(MessageList);
			await _maintenanceService.MarkDisconnectedWorkers();

			Assert.Equal(18, MessageList.Count);
		}

		[Fact]
		public async Task MarkDisconnectedWorkers_WhenHasDisconnectedWorkers_ShouldMarkDisconnectedWorkers()
		{
			_messageRepositoryMock.GetMessagesBefore(Arg.Any<long>()).Returns(MessageList);
			_messageRepositoryMock.GetMessagesAfter(Arg.Any<long>()).Returns(new List<IRceMessage>());
			await _maintenanceService.MarkDisconnectedWorkers();

			Assert.Equal(19, MessageList.Count);
			Assert.Equal(nameof(WorkerRemovedMessage), MessageList[18].MessageType);
			Assert.Equal(WorkerRemovedMessage.Statuses.ConnectionLost, ((WorkerRemovedMessage) MessageList[18]).ConnectionStatus);
			Assert.Equal(new Guid("00000003-0000-0000-0000-000000000000"), ((WorkerRemovedMessage) MessageList[18]).WorkerId);
		}
	}
}

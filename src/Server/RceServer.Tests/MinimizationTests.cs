using System;
using System.Collections.Generic;
using RceServer.Core.Helpers;
using RceServer.Domain.Models.Messages;
using Xunit;

namespace RceServer.Tests
{
	public class MinimizationTests
	{
		private List<IRceMessage> MessageList { get; set; }

		public MinimizationTests()
		{
			MessageList = new List<IRceMessage>
			{
				new AddWorkerMessage // Create worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000001"),
					MessageTimestamp = 1,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new AddWorkerMessage // Create worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000002"),
					MessageTimestamp = 2,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new AddWorkerMessage // Create worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000003"),
					MessageTimestamp = 3,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new AddJobMessage // Create job 1 for worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000004"),
					MessageTimestamp = 4,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000001"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new AddJobMessage // Create job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000005"),
					MessageTimestamp = 5,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new AddJobMessage // Create job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000006"),
					MessageTimestamp = 6,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new AddJobMessage // Create job 4 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000007"),
					MessageTimestamp = 7,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000004"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new RemoveWorkerMessage // Worker 1 fails
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000008"),
					MessageTimestamp = 8,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001"),
					ConnectionStatus = RemoveWorkerMessage.Status.ConnectionLost
				},
				new UpdateJobMessage // Update job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000009"),
					MessageTimestamp = 9,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new CompleteJobMessage // Complete job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000010"),
					MessageTimestamp = 10,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new RemoveWorkerMessage // Worker 2 Ends
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000011"),
					MessageTimestamp = 11,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002"),
					ConnectionStatus = RemoveWorkerMessage.Status.ClosedByWorker
				}
			};
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenFullList_MinimizesList()
		{
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(3, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000010"), MessageList[index++].MessageId);
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenListWithoutWorkerOneCreated_MinimizesList()
		{
			MessageList.RemoveAt(0);
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(5, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000004"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000008"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000010"), MessageList[index++].MessageId);
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenListWithoutWorkersOneAndTwoCreated_MinimizesList()
		{
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(9, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000004"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000005"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000007"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000008"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000009"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000010"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000011"), MessageList[index++].MessageId);
		}
	}
}

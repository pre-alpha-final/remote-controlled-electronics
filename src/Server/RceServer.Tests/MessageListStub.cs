using System;
using System.Collections.Generic;
using RceServer.Domain.Models.Messages;

namespace RceServer.Tests
{
	public static class MessageListStub
	{
		public static List<IRceMessage> Get()
		{
			var index = 1;
			return new List<IRceMessage>
			{
				new WorkerAddedMessage // Create worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000001"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new WorkerAddedMessage // Create worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000002"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerAddedMessage // Create worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000003"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobAddedMessage // Create job 1 for worker 1
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000004"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000001"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new JobAddedMessage // Create job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000005"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new JobPickedUpMessage // Job 1 for worker 1 picked up
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000006"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000001"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001")
				},
				new JobPickedUpMessage // Job 2 for worker 2 picked up
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000007"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new KeepAliveSentMessage // Worker 2 Keep Alive
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000008"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new KeepAliveSentMessage // Worker 2 Keep Alive
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000009"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new JobAddedMessage // Create job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000010"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobAddedMessage // Create job 4 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000011"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000004"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerRemovedMessage // Worker 1 fails
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000012"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000001"),
					ConnectionStatus = WorkerRemovedMessage.Statuses.ConnectionLost
				},
				new JobUpdatedMessage // Update job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000013"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new JobUpdatedMessage // Update job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000014"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobCompletedMessage // Complete job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000015"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
				new JobUpdatedMessage // Update job 2 for worker 2
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000016"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000002"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002")
				},
				new WorkerRemovedMessage // Worker 2 Ends
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000017"),
					MessageTimestamp = index++,
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000002"),
					ConnectionStatus = WorkerRemovedMessage.Statuses.ClosedByWorker
				},
				new JobRemovedMessage // Remove job 3 for worker 3
				{
					MessageId = new Guid("00000000-0000-0000-0000-000000000018"),
					MessageTimestamp = index++,
					JobId = new Guid("FFFFFFFF-0000-0000-0000-000000000003"),
					WorkerId = new Guid("00000000-FFFF-0000-0000-000000000003")
				},
			};
		}
	}
}

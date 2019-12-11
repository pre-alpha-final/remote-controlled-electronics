using System;
using System.Collections.Generic;
using RceServer.Core.Helpers;
using RceServer.Domain.Models.Messages;
using Xunit;

namespace RceServer.Tests
{
	public class MinimizationTests
	{
		private List<IRceMessage> MessageList { get; }

		public MinimizationTests()
		{
			MessageList = MessageListStub.Get();
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenFullList_MinimizesList()
		{
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(1, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenListWithoutWorkerOneCreated_MinimizesList()
		{
			MessageList.RemoveAt(0);
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(4, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000004"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000012"), MessageList[index++].MessageId);
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenListWithoutWorkersOneAndTwoCreated_MinimizesList()
		{
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(10, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000004"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000005"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000007"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000009"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000011"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000012"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000016"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000017"), MessageList[index++].MessageId);
		}

		[Fact]
		public void RceMessageHelpers_WhenGivenListWithMissingMessages_MinimizesList()
		{
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			MessageList.RemoveAt(0);
			RceMessageHelpers.Minimize(MessageList);

			var index = 0;
			Assert.Equal(7, MessageList.Count);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000011"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000012"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000014"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000015"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000016"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000017"), MessageList[index++].MessageId);
			Assert.Equal(new Guid("00000000-0000-0000-0000-000000000018"), MessageList[index++].MessageId);
		}
	}
}

using System;
using Newtonsoft.Json;
using RceServer.Domain.Models.Messages;
using Xunit;

namespace RceServer.Tests
{
	public class MessageTests
	{
		[Fact]
		public void Message_WhenCreated_HasMessageIdSet()
		{
			var testMessage = new AddWorkerMessage();

			Assert.NotEqual(Guid.Empty, testMessage.MessageId);
		}

		[Fact]
		public void Message_WhenCreated_HasMessageTimestampSet()
		{
			var testMessage = new AddWorkerMessage();

			Assert.True(testMessage.MessageTimestamp > 0);
		}

		[Fact]
		public void Message_WhenCreated_HasMessageTypeSet()
		{
			var testMessage = new AddWorkerMessage();

			Assert.Equal("AddWorkerMessage", testMessage.MessageType);
		}

		[Fact]
		public void Message_WhenSerializedDeserialized_RetainsMessageId()
		{
			var testMessage = new AddWorkerMessage();
			var originalValue = testMessage.MessageId;
			var serializedMessage = JsonConvert.SerializeObject(testMessage);
			var deserializedMessage = JsonConvert.DeserializeObject<AddWorkerMessage>(serializedMessage);

			Assert.Equal(originalValue, deserializedMessage.MessageId);
		}

		[Fact]
		public void Message_WhenSerializedDeserialized_RetainsMessageTimestamp()
		{
			var testMessage = new AddWorkerMessage();
			var originalValue = testMessage.MessageTimestamp;
			var serializedMessage = JsonConvert.SerializeObject(testMessage);
			var deserializedMessage = JsonConvert.DeserializeObject<AddWorkerMessage>(serializedMessage);

			Assert.Equal(originalValue, deserializedMessage.MessageTimestamp);
		}

		[Fact]
		public void Message_WhenSerializedDeserialized_RetainsMessageType()
		{
			var testMessage = new AddWorkerMessage();
			var originalValue = testMessage.MessageType;
			var serializedMessage = JsonConvert.SerializeObject(testMessage);
			var deserializedMessage = JsonConvert.DeserializeObject<AddWorkerMessage>(serializedMessage);

			Assert.Equal(originalValue, deserializedMessage.MessageType);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Data
{
	// TODO make a persistent version
	public class InMemoryMessageRepository : IMessageRepository
	{
		private List<IRceMessage> RceMessages { get; set; }

		public Task AddMessage(IRceMessage message)
		{
			throw new NotImplementedException();
		}

		// TODO return deep copy until db is used
		public Task<IList<IRceMessage>> GetMessagesBefore(long timestamp)
		{
			throw new NotImplementedException();
		}

		// TODO return deep copy until db is used
		public Task<IList<IRceMessage>> GetMessagesAfter(long timestamp)
		{
			throw new NotImplementedException();
		}

		public Task RemoveMessages(IEnumerable<Guid> messages)
		{
			throw new NotImplementedException();
		}
	}
}

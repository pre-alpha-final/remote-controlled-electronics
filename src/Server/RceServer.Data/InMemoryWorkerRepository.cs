using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models;
using RceServer.Domain.Services;

namespace RceServer.Data
{
	// TODO make a persistent version
    // TODO add pruning
    // TODO add minimization
	public class InMemoryWorkerRepository : IWorkerRepository
	{
		private List<IRceMessage> RceMessages { get; set; }

		public Task AddMessage(IRceMessage rceMessage)
		{
			throw new System.NotImplementedException();
		}

		public Task<List<IRceMessage>> GetMessages(long timestamp)
		{
			throw new System.NotImplementedException();
		}
	}
}

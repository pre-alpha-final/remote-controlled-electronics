using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models;

namespace RceServer.Domain.Services
{
	public interface IWorkerRepository
	{
		Task AddMessage(IRceMessage rceMessage);
		Task<List<IRceMessage>> GetMessages(long timestamp);
	}
}

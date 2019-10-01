using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models;

namespace RceServer.Domain.Services
{
	public interface IClientService
	{
		Task<IList<IRceMessage>> GetFeed(long timestamp);
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Domain.Services
{
	public interface IClientService
	{
		Task<IList<IRceMessage>> GetState();
	}
}

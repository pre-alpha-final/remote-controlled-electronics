using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Core.Hubs
{
	public interface IRceHub
	{
		Task MessageReceived(IRceMessage message);
	}
}

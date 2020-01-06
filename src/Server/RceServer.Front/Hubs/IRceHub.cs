using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Front.Hubs
{
	public interface IRceHub
	{
		Task MessageReceived(IRceMessage message);
	}
}

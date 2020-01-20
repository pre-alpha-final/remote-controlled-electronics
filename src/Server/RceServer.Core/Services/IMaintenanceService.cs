using System.Threading.Tasks;

namespace RceServer.Core.Services
{
	public interface IMaintenanceService
	{
		void Start();
		Task RemoveOldActivity();
		Task MarkDisconnectedWorkers();
	}
}

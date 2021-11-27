using System.Threading.Tasks;

namespace RceSharpLib.States
{
	public interface IState
	{
		Task Handle();
	}
}

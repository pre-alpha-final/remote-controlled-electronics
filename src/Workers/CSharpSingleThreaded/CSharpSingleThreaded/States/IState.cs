using System.Threading.Tasks;

namespace CSharpSingleThreaded.States
{
	public interface IState
	{
		Task Handle(JobRunnerStateMachine jobRunnerStateMachine);
	}
}

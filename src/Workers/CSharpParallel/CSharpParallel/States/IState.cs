using System.Threading.Tasks;

namespace CSharpParallel.States
{
	public interface IState
	{
		Task Handle(JobRunnerStateMachine jobRunnerStateMachine);
	}
}

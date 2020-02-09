using System.Threading.Tasks;

namespace CSharpSequential.States
{
	public interface IState
	{
		Task Handle(JobRunnerStateMachine jobRunnerStateMachine);
	}
}

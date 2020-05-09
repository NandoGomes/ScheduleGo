using ScheduleGo.Shared.ScheduleGoContext.Commands;

namespace ScheduleGo.Shared.ScheduleGoContext.Handlers
{
	public interface ICommandHandler<TCommand, TCommandResult> where TCommand : ICommand where TCommandResult : CommandResult
	{
		TCommandResult Handle(TCommand command);
	}
}
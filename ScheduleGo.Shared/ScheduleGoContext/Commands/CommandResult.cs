using System.Net;

namespace ScheduleGo.Shared.ScheduleGoContext.Commands
{
	public class CommandResult
	{
		private HttpStatusCode _code;

		public CommandResult() => _code = HttpStatusCode.InternalServerError;
		public CommandResult(HttpStatusCode code) => _code = code;

		public HttpStatusCode Code() => _code;
	}
}
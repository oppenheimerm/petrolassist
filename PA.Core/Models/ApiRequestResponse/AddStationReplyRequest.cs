
namespace PA.Core.Models.ApiRequestResponse
{
	public class AddStationReplyRequest
	{
		public Station? Station { get; set; }
		public bool? Success { get; set; } = false;
		public string? ErrorMessage { get; set; }
	}
}

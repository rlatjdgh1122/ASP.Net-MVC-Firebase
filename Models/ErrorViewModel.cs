namespace ASP.Net_MVC_Firebase.Models
{
	public class ErrorViewModel		

	{
		public string? RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}

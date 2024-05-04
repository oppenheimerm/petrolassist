
namespace PA.Core.Models.ApiRequestResponse
{
    public class MetaDataRequest
    {
        public string MobileDeviceType { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public DateTime? Date { get; set;}


    }
}

using PA.Core.Helpers.Paging;

namespace PA.Core.Helpers.Paging
{
	public class PagingResponse<T> where T : class
	{
		public List<T>? Items { get; set; }
		public MetaData? MetaData { get; set; }
	}
}

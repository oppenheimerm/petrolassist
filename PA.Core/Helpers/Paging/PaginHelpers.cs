
namespace PA.Core.Helpers.Paging
{
	public enum StationSortOrder
	{
		Id,
		StationName,
		AddedDate,
		StationPostCode
	}

	public static class PaginHelpers
	{


		/// <summary>
		/// Helpers function to convert <see cref="StationSortOrder"/> from its interger value
		/// to it's enumeration value
		/// </summary>
		/// <param name="stationSortOrder"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static StationSortOrder GetStationSortOrder(int stationSortOrder)
		{
			StationSortOrder sortOrder = stationSortOrder switch
			{
				0 => StationSortOrder.Id,
				1 => StationSortOrder.StationName,
				2 => StationSortOrder.AddedDate,
				3 => StationSortOrder.StationPostCode,
				_ => throw new NotImplementedException(),
			};

			return sortOrder;
		}

		public static int GetStationSortOrderAsInt(StationSortOrder sortOrder) 
		{
			int sortOrderAsInt = sortOrder switch
			{ 
				StationSortOrder.Id => 0,
				StationSortOrder.StationName => 1,
				StationSortOrder.AddedDate => 2,
				StationSortOrder.StationPostCode => 3,
				_ => throw new NotImplementedException(),
			};
			return sortOrderAsInt;
		}
	}
}

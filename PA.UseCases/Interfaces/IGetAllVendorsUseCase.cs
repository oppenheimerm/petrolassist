
using PA.Core.Helpers.Paging;
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllVendorsUseCase
	{
		IQueryable<Vendor> Execute();

	}
}

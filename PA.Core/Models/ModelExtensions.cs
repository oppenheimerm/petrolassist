
using PA.Core.Helpers;
using PA.Core.Models.ApiRequestResponse;

namespace PA.Core.Models
{
	public static class ModelExtensions
	{
		public static Member ToMember(this RegisterRequest registerRequest)
		{
			if (registerRequest == null) throw new ArgumentNullException(nameof(registerRequest));
			else
			{
				return new Member
				{
					Id = Guid.NewGuid(),
					FirstName = StringHelpers.ConvertToTitleCase(registerRequest.FirstName),
					LasttName = StringHelpers.ConvertToTitleCase(registerRequest.LastName),
					EmailAddress = registerRequest.EmailAddress.ToLowerInvariant(),
					AcceptTerms = registerRequest.AcceptTerms,
					MobileNumber = registerRequest.MobileNumber,
					LastLogIn = DateTime.Now
				};
			}
		}
	}
}


using PA.Core.Models;

namespace PA.Datastore.EFCore.Authorisation.Interfaces
{
	public interface IJwtUtils
	{
		string GenerateJwtToken(Member user);
		Guid? ValidateJwtToken(string token);
		public Task<RefreshToken> GenerateRefreshTokenAsync(string ipAddress);
	}
}

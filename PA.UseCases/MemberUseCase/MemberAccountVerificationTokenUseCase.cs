
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
	public class MemberAccountVerificationTokenUseCase : IMemberVerifyEmailUseCase
	{
		private readonly IMembersRepository MembersRepository;

        public MemberAccountVerificationTokenUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

		public async Task<(bool Success, string Message, int httpCode)> ExecuteAsync(string token)
		{
			var response = await MembersRepository.VerifyAccountToken(token);
			return (response.Success, response.Message, response.httpCode);
		}
	}
}

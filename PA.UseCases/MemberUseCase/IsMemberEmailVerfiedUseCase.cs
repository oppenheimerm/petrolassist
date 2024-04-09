
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
	public class IsMemberEmailVerfiedUseCase : IIsMemberEmailVerfiedUseCase
	{
		private readonly IMembersRepository MembersRepository;
		public IsMemberEmailVerfiedUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

		public async Task<(bool Verified, bool hasAccount, Member? Member)> ExecuteAsync(string EmailAddress)
		{
			var response = await MembersRepository.MemberEmailVerified(EmailAddress);
			return response;
		}

	}
}

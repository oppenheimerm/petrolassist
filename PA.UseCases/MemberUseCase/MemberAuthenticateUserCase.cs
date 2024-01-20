using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
    public class MemberAuthenticateUserCase : IMemberAuthenticateUserCase
    {
        private readonly IMembersRepository MembersRepository;

        public MemberAuthenticateUserCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

        public async Task<AuthenticateResponse> ExecuteAsync(AuthenticateRequest model, string ipAddress)
        {
            var response = await MembersRepository.LogInAsync(model, ipAddress);
            return response;
        }
    }
}

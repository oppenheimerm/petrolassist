
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
    public class MemberGetByRefreshTokenUseCase : IMemberGetByRefreshTokenUseCase
    {
        private readonly IMembersRepository MembersRepository;

        public MemberGetByRefreshTokenUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

        public async Task<AuthenticateResponse> ExecuteAsync(string refreshToken, string ipAddress)
        {
            var response = await MembersRepository.RefreshTokenAsync(refreshToken, ipAddress);
            return response;
        }
    }
}

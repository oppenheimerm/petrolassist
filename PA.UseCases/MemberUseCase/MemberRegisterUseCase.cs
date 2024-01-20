
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
    public class MemberRegisterUseCase : IMemberRegisterUseCase
    {
        private readonly IMembersRepository MembersRepository;

        public MemberRegisterUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

        public async Task<(bool Success, string ErrorMessage)>ExecuteAsync(RegisterRequest request, string origin)
        {
            var response =  await MembersRepository.RegisterAsync(request, origin);
            return (response);
        }
    }
}

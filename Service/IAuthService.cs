using Diary.DTOs;

namespace Diary.Service
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>>Login(LoginRequestDTO loginRequestDTO);
        Task<ServiceResponse<RegisterResponseDTO>>Register(RegisterRequestDTO registerRequestDTO);
        Task<bool> IsUserExistByUserName(string userName);
    }
}
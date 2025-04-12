using Application.Request.Account;
using Application.Request.Patient;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(UserRegisterRequest userRequest, string roleId);
        Task<ApiResponse> ActivateAccountAsync(string accountId);
        Task<ApiResponse> LoginAsync(LoginRequest request);
        Task<ApiResponse> GoogleLoginAsync(GoogleLoginRequest request);
        Task<ApiResponse> VerifyEmailAsync(string userId, string verificationCode);

        //Task<ApiResponse> LoginForDriverAsync(LoginRequest request);
    }
}

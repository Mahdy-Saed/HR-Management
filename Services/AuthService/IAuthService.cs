using HR_Carrer.Authntication;
using HR_Carrer.Data;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.AuthDtos;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponce<AuthLogResDto>> login(AuthLogReqDto authLoginReq);

             Task<ServiceResponce<string>> logout(Guid id);

          Task<ServiceResponce<string>> Refresh(string refreshToken);

    }


    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;

        private readonly ITokenGenerater _TokenGenerater;

        private readonly IPasswordHasher _passwordHasher;


        public AuthService(IUserRepo userRepo,ITokenGenerater tokenGenerater,IPasswordHasher passwordHasher)
        {
            _userRepo=userRepo;
            _TokenGenerater=tokenGenerater;
            _passwordHasher=passwordHasher;
        }



        public async Task<ServiceResponce<AuthLogResDto>> login(AuthLogReqDto authLoginReq)
        {
            var res = new ServiceResponce<AuthLogResDto>();
            if (authLoginReq == null)
            {
                res.Data = null;
                res.Message = "Must enter the information";
                res.StatusCode = 400;
                return res;
            }

            var user = await _userRepo.GetByEmailAsync(authLoginReq.Email!);

            if (user is null ||! _passwordHasher.verify(authLoginReq.Password!, user.PasswordHash!))
            {
                res.Data = null;
                res.Message = "Email or password is incorrect";
                res.StatusCode = 401;
                return res;
            }
            
            var AccessToken = _TokenGenerater.CreateAccessToken(user);
            var RefreshToken = _TokenGenerater.CreateRefreshToken();

            res.Data = new AuthLogResDto
            {
                Id=user.Id,
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };


            user.RefreshToken= RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepo.UpdateAsync(user);

            return res;
        
        }

        public async Task<ServiceResponce<string>> logout(Guid id)
        {
            var res = new ServiceResponce<string>();


            var user = await _userRepo.GetByIdAsync(id);
            if ( user is null) { res.Data = null;
                res.Message = "User not found";
                res.StatusCode = 404;
                return res;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepo.UpdateAsync(user);
            res.Data = null;
            res.Message= "Logged out successfully";
            return res;

        }
        public async Task<ServiceResponce<string>> Refresh(string refreshToken)
        {
            var res = new ServiceResponce<string>();

            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

            if (user is null || !_TokenGenerater.ValidateRefreshToken(user, refreshToken))
            {
                res.Data = null;
                res.Message = "Invalid refresh token";
                res.StatusCode = 401;
                return res;
            }
            var newAccessToken = _TokenGenerater.CreateAccessToken(user);
             await _userRepo.UpdateAsync(user);
            res.Data = newAccessToken;
            res.Message =" Access token refresh Successfully"; 
            return res;


        }
    }
}

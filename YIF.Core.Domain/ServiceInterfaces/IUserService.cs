using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ResponseApiModel<UserApiModel>> GetUserById(string id);
        Task<ResponseApiModel<UserApiModel>> GetUserByEmail(string email);
        Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAllUsers();
        Task<ResponseApiModel<IEnumerable<UserApiModel>>> FindUser(Expression<Func<T, bool>> predicate);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> LoginUser(LoginApiModel loginModel);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> RegisterUser(RegisterApiModel registerModel);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> RefreshToken(TokenRequestApiModel tokenApiModel);
        Task<ImageApiModel> ChangeUserPhoto(ImageApiModel model, string userId);
        Task<UserProfileApiModel> GetUserProfileInfoById(string userId);
        Task<ResponseApiModel<UserProfileApiModel>> SetUserProfileInfoById(UserProfileApiModel model, string userId);
        Task<ResponseApiModel<ResetPasswordByEmailApiModel>> ResetPasswordByEmail(ResetPasswordByEmailApiModel model, HttpRequest request);

        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(string id);

        void Dispose();

        // =========================   For test authorize endpoint:   =========================
        Task<ResponseApiModel<RolesByTokenResponseApiModel>> GetCurrentUserRolesUsingAuthorize(string id);
        Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAdminsUsingAuthorize(string id);
    }
}

using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface IAuthService
{
    AuthModel Register(UserModel model);

    AuthModel Login(UserModel model);

    AuthModel UpdateToken(AuthModel model);
}

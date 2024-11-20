using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;

public interface IUserRepository
{
    void Add(UserModel model);

    void Ban(BannedModel model);

    void Unban(int id);

    void UnbanByBanId(int user_id, int ban_id);

    BannedModel? IsBan(int id);

    void BanMarks(int id, bool isBlock);

    List<UserModel> GetAll();

    UserModel? GetUserById(int id);

    UserModel? GetUserByUsername(string username);

    UserModel? GetUserByEmail(string email);

    void UpdateToken(UserModel model);

    void ChangePassword(int id, string password);
}

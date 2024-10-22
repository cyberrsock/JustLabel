using JustLabel.Models;
using System.Collections.Generic;

namespace JustLabel.Services.Interfaces;

public interface IUserService
{
    UserModel GetUserByID(int id);

    List<UserModel> GetUserByIDs(List<int> id);

    List<UserModel> GetUsers(int admin_id);

    void Ban(BannedModel model);

    void Unban(int id);

    void UnbanByBanId(int user_id, int ban_id);

    void BanMarks(int id, int admin_id, bool isBlock);

    int IsBanned(int id);
}

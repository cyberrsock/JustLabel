using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;

public interface ISchemeRepository
{
    void Add(SchemeModel model);

    void Update(SchemeModel model);

    void Delete(int id);

    SchemeModel? Get(int id);

    List<SchemeModel> GetAll();
}

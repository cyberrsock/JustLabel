using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface ISchemeService
{
    void Add(SchemeModel model);

    void Update(SchemeModel model);

    SchemeModel Get(int id);

    List<SchemeModel> Get();

    void Delete(int id);
}

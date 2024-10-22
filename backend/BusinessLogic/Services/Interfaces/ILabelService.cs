using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface ILabelService
{
    int Add(LabelModel model);

    List<LabelModel> Get();

    void Delete(int id);
}

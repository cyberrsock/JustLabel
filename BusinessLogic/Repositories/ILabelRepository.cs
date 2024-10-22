using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;

public interface ILabelRepository
{
    int Add(LabelModel model);

    List<LabelModel> Get();
    
    void Delete(int id);
}

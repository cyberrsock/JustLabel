using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;
public interface IDatasetRepository
{
    int Add(DatasetModel model);

    void Delete(int id);

    DatasetModel? Get(int id);

    List<DatasetModel> GetAll();
}

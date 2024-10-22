using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface IDatasetService
{
    int Create(DatasetModel model, List<ImageModel> images);

    (DatasetModel dataset, List<ImageModel> images) Get(int id);

    List<DatasetModel> Get();

    int WhichImage(int id);

    void Delete(int id);
}

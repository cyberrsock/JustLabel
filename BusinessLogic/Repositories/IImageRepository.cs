using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;
public interface IImageRepository
{
    void Add(ImageModel model);

    List<ImageModel> GetAll(int id);

    ImageModel? Get(int id);
}

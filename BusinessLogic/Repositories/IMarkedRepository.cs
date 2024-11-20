using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;

public interface IMarkedRepository
{
    void Create(MarkedModel model);

    void Update_Rects(MarkedModel model);

    void Update_Block(MarkedModel model);

    List<MarkedModel> Get_By_DatasetId(int id);

    List<MarkedModel> Get_By_SchemeId(int id);

    List<MarkedModel> Get_By_Dataset_and_SchemeId(int datasetId, int schemeId);

    MarkedModel Get(MarkedModel model);

    List<MarkedModel> GetAll();

    void Delete(int id);

    List<AggregatedModel> GetAggregatedData(int datasetId, int schemeId);
}

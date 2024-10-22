using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface IMarkedService
{
    void Create(MarkedModel model);

    void Update_Rects(MarkedModel model);

    void Update_Block(MarkedModel model);

    MarkedModel Get(MarkedModel model);

    public List<AggregatedModel> GetForAggr(int admin_id, int dataset_id, int scheme_id);

    List<MarkedModel> Get(int id, int admin_id);

    List<MarkedModel> GetAll(int admin_id);

    void Delete(int id);
}

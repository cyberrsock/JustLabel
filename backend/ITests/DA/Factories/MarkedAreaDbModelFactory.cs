using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class MarkedAreaDbModelFactory
{
    public static MarkedAreaDbModel Create(int markedId, int areaId)
    {
        return new MarkedAreaDbModel
        {
            MarkedId = markedId,
            AreaId = areaId
        };
    }
}
